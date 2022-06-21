using System.Runtime.InteropServices;

using Strawberry.Mathematics;
using Strawberry.Gpu;

// Create triangle mesh
var triangleMesh = new Mesh<Vertex>(3, 3);
triangleMesh.SetIndices(new uint[] { 0, 1, 2 });
triangleMesh.SetVertices(new Vertex[]{
    new Vertex(new Vector2(-1.0F, -1.0F), new Vector2(0.0F, 0.0F), new Vector4(1.0F, 0.0F, 0.0F, 1.0F)),
    new Vertex(new Vector2(+1.0F, -1.0F), new Vector2(1.0F, 0.0F), new Vector4(0.0F, 1.0F, 0.0F, 1.0F)),
    new Vertex(new Vector2(+0.0F, +1.0F), new Vector2(0.5F, 1.0F), new Vector4(0.0F, 0.0F, 1.0F, 1.0F)),
});

var geometry = new Mesh();
geometry.SetIndexParams(3); // todo: topology?
geometry.SetIndexBufferData<uint>(new uint[] { 0, 1, 2 });
// todo: geometry.SetVertexParams<V>(count) too?
// todo: geometry.SetVertices<V>(V[] vertices, ...);
geometry.SetVertexParams(3, VertexAttributeDescription.CreateFrom<Vertex>()); // note: option 1
geometry.SetVertexParams(3, new VertexAttributeDescription[] {                // note: option 2
    new VertexAttributeDescription(VertexAttribute.Position, ElementType.Float32, 3),
    new VertexAttributeDescription(VertexAttribute.UV, ElementType.Float32, 2),
    new VertexAttributeDescription(VertexAttribute.Color, ElementType.Float32, 4)
});
geometry.SetVertexBufferData<Vertex>(new[] {
    new Vertex(new Vector2(-1.0F, -1.0F), new Vector2(0.0F, 0.0F), new Vector4(1.0F, 0.0F, 0.0F, 1.0F)),
    new Vertex(new Vector2(+1.0F, -1.0F), new Vector2(1.0F, 0.0F), new Vector4(0.0F, 1.0F, 0.0F, 1.0F)),
    new Vertex(new Vector2(+0.0F, +1.0F), new Vector2(0.5F, 1.0F), new Vector4(0.0F, 0.0F, 1.0F, 1.0F)),
});

// TODO: Somehow create/acquire window
var window = new Window();
var gfx = window.Gpu; // TODO: window.Graphics and window.Compute?

// TODO: Should clear values be part of the framebuffer?
// TODO: Perhaps -> gpu.BeginPass(window.Framebuffer, clearColor, clearDepth, viewport)
gfx.SetFramebuffer(window.Framebuffer);

// Clear the attachments contents
gfx.ClearColor();
gfx.ClearDepth();

// ------------------------------------------------------------------------------------------------

// Setup compute context
var conwayContext = new ComputeContext("conway.comp");
var conwayBuffers = new GpuBuffer[2] {
    new GpuBuffer(1024 * 1024 / 4),
    new GpuBuffer(1024 * 1024 / 4)
};

// Perform 100 iterations
for (var i = 0; i < 100; i++)
{
    conwayContext.SetStorageBuffer(0, conwayBuffers[i % 2]);
    conwayContext.Dispatch(32, 32, 1);
    conwayContext.MemoryBarrier(); // ...?
}

// ------------------------------------------------------------------------------------------------

public sealed class Mesh<TVertex> where TVertex : unmanaged
{
    public readonly GpuBuffer VertexBuffer;

    public readonly GpuBuffer IndexBuffer;

    public readonly VertexBufferDescription VertexAttributeLayout;

    public readonly MeshTopology Topology;

    public readonly uint VertexCapacity;

    public readonly uint IndexCapacity;

    public Mesh(uint vertexCapacity, uint indexCapacity, MeshTopology topology = MeshTopology.Triangles)
    {
        // Get vertex attribute layout from type reflection
        VertexAttributeLayout = VertexBufferDescription.Create<TVertex>();

        // Create gpu memory buffers
        VertexBuffer = GpuBuffer.CreateStructured<TVertex>(vertexCapacity);
        IndexBuffer = GpuBuffer.CreateStructured<uint>(indexCapacity);

        // ...
        VertexCapacity = vertexCapacity;
        IndexCapacity = indexCapacity;

        // ...
        Topology = topology;
    }

    public void SetVertices(ReadOnlySpan<TVertex> vertices)
    {
        VertexBuffer.Update(vertices);
    }

    public void SetIndices(ReadOnlySpan<uint> indices)
    {
        IndexBuffer.Update(indices);
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct Vertex
{
    public Vector2 Position;
    public Vector2 UV;
    public Vector4 Color;

    public Vertex(Vector2 position, Vector2 uv, Vector4 color)
    {
        Position = position;
        UV = uv;
        Color = color;
    }
}
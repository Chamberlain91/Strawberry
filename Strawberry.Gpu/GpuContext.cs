namespace Strawberry.Gpu;

public abstract class GpuContext
{
    // todo: Perhaps pipelines could be folded into the `Draw` or `Dispatch` calls?

    // ------------------------------------------------------------------------
    // GRAPHICS CONTEXT
    // ------------------------------------------------------------------------

    public abstract void SetGraphicsPipeline(GraphicsPipeline pipeline);
    // shaders know actual attributes used via reflection

    // buffer 0
    // layout(location = 0) in vec3 a_position; 
    // layout(location = 1) in vec3 a_normal;
    // layout(location = 2) in vec2 a_uv;
    // buffer 1
    // layout(location = 3) in mat4 i_transform;
    // layout(location = 7) in vec4 i_color;

    // RENDER INPUT (MESH DATA)

    public abstract void SetVertexBuffer(Buffer buffer, VertexBufferDescription description, uint index = 0, uint offset = 0); // glBindVertexBuffer(...)
    public abstract void SetIndexBuffer(Buffer? buffer, ElementType elementType = ElementType.UnsignedInteger); // glBindBuffer(GL_ELEMENT_ARRAY_BUFFER, ...)

    // RENDER OUTPUT (RENDER ATTACHMENTS)

    // NOTE: Changing the framebuffer will clear scissor state
    public abstract void SetFramebuffer(Framebuffer framebuffer, Viewport? viewport = null);

    // NOTE: Lazily evaluated to combine flags, etc
    public abstract void ClearColor(float r = 0F, float g = 0F, float b = 0F, float a = 1F);
    public abstract void ClearDepth(float depth = 1F);

    public abstract void SetViewport(Viewport viewport); // update the viewport
    public abstract void SetScissor(Viewport? scissor);  // null to disable?

    // todo: Implement the ability to blit framebuffers

    public abstract void DrawIndirect(Buffer indirectBuffer, uint offset, int indirectCount = 1); // multiple glDrawElementsIndirect (or glMultiDrawElementsIndirect)
    public abstract void Draw(int vertexCount, uint offset = 0, int instances = 1); // glDrawElements

    // ------------------------------------------------------------------------
    // COMPUTE CONTEXT
    // ------------------------------------------------------------------------

    public abstract void SetComputePipeline(ComputePipeline pipeline);

    public abstract void Dispatch(int numGroupX, int numGroupY = 1, int numGroupZ = 1);

    // ------------------------------------------------------------------------
    // SHARED CONTEXT
    // ------------------------------------------------------------------------

    // todo: C# source generators to somehow enforce type safety and assignment of these resources?

    public abstract void SetUniformBuffer(uint index, Buffer buffer); // glBindBufferRange
    public abstract void SetUniformBuffer(uint index, Buffer buffer, uint offset, uint size);

    public abstract void SetStorageBuffer(uint index, Buffer buffer); // glBindBufferRange
    public abstract void SetStorageBuffer(uint index, Buffer buffer, uint offset, uint size);

    public abstract void SetSampler(uint index, TextureSampler sampler, Texture2D texture); // glActiveTexture + glBindTexture
    public abstract void SetSampler(uint index, TextureSampler sampler, Texture2DArray texture, int layer);
    public abstract void SetSampler(uint index, TextureSampler sampler, TextureCube texture, TextureCubeFace face);
    public abstract void SetSampler(uint index, TextureSampler sampler, Texture3D texture, int layer);

    public abstract void SetImage(uint index, Texture2D texture, int mipLevel = 0); // glBindImageTexture
    public abstract void SetImage(uint index, Texture2DArray texture, int layer, int mipLevel = 0);
    public abstract void SetImage(uint index, TextureCube texture, TextureCubeFace face, int mipLevel = 0);
    public abstract void SetImage(uint index, Texture3D texture, int layer, int mipLevel = 0);

    // [SYNCHRONIZATION]

    public abstract void MemoryBarrier(MemoryBarrier barrier);
}

[Flags]
public enum MemoryBarrier : uint
{
    // todo: remainder of the barrier bits
    All
}

public enum ElementType : uint
{
    U16,
    U32
}

public enum MeshTopology : uint
{
    Triangles,
    Lines,
    Points
}

public abstract class GpuPipeline
{
    // todo: implement IDisposable explicit w/ dispose pattern
}

public abstract class ComputePipeline : GpuPipeline
{
    public ComputeShader Shader { get; }
}

public abstract class GraphicsPipeline : GpuPipeline
{
    public GraphicsShader Shader { get; }

    // "how to assemble render primitives"
    public MeshTopology Topology;

    // "how to read vertex attributes from vertex buffers"
    public VertexLayout Layout;

    // glVertexBindingDivisor(buffer, divisor)
    // for each attribute in buffer:
    //   glEnableVertexAttribArray(attribute)
    //   glVertexAttribBinding(attribute, buffer)
    //   glVertexAttribFormat(attribute, ...)

    // TODO: Culling Mode

    // TODO: Depth Enable
    // TODO: Depth Compare Mode
    // TODO: Depth Write Mask

    // TODO: Color Blending Mode
    // TODO: Color Write Mask
}

public enum CullingMode : byte
{
    None,
    Front,
    Back
}

public enum BlendMode : byte
{
    Opaque,   // blending disabled
    Add,      // ...?
    Subtract, // ...?
    Multiply  // ...?
}

[Flags]
public enum MaskRGBA : byte
{
    R = 1 << 0,
    G = 1 << 1,
    B = 1 << 2,
    A = 1 << 3,

    RGBA = R | G | B | A,
}

public abstract class GpuShader : IDisposable
{
    // todo: implement IDisposable explicit w/ dispose pattern

    // todo: shader reflection?
}

public abstract class GraphicsShader : GpuShader
{
    // note: in Javelin, this internally tracked relevant VAOs to dispose with it
}

public abstract class ComputeShader : GpuShader
{
    public readonly int LocalSizeX;

    public readonly int LocalSizeY;

    public readonly int LocalSizeZ;
}

public abstract class GpuPiplineResources // todo: proper concept
{
    // todo: buffer bindings (uniform, storage)
    // todo: image bindings (samplers, images) 
}

public enum CompareFunc
{
    LessEqual,    // x <= y
    Less,         // x < y
    GreaterEqual, // x >= y
    Greater,      // x > y
    Equal,        // x == y
    NotEqual,     // x != y
    Always,       // true
    Never         // false
}

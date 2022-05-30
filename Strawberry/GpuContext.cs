namespace Strawberry;

public abstract class GpuContext
{
    // ------------------------------------------------------------------------
    // GRAPHICS CONTEXT
    // ------------------------------------------------------------------------

    public abstract void SetGraphicsPipeline(GraphicsPipeline pipeline);

    // RENDER INPUT (MESH DATA)

    public abstract void SetIndexBuffer(Buffer? buffer, ElementType elementType = ElementType.U32); // glVertexArrayElementBuffer
    public abstract void SetVertexBuffer(uint index, VertexBuffer buffer, uint offset = 0); // glVertexArrayVertexBuffer

    // RENDER OUTPUT (RENDER ATTACHMENTS)

    // note: Changing the framebuffer will clear scissor state
    public abstract void SetFramebuffer(Framebuffer framebuffer, Viewport? viewport = null);

    // note: Lazily evaluated to combine flags, etc
    public abstract void ClearColor(float r, float g, float b, float a = 1F);
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

    public abstract void Dispatch(int numGroupX, int numGroupY, int numGroupZ);

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

public enum ElementType : uint { U16, U32 }

public enum MeshTopology : uint
{
    Triangles,
    Lines,
    Points
}

public abstract class GpuPipeline { }

public abstract class ComputePipeline : GpuPipeline { }

public abstract class GraphicsPipeline : GpuPipeline
{
    public MeshTopology topology;
    // todo: input attribute state
    // todo: raster state (culling)
    // todo: depth state (enable, mask, compare)
    // todo: color blend state (blending, mask)
}

public abstract class GpuPiplineResources
{
    // todo: buffer bindings (uniform, storage)
    // todo: image bindings (samplers, images) 
}

public enum MemoryBarrier : uint
{
    // todo: remainder
    All
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

namespace Strawberry.Gpu;

public abstract class Buffer : IDisposable
{
    protected Buffer(uint capacity)
    {
        Capacity = capacity;
    }

    public uint Capacity { get; }

    // todo: implement IDisposable explicit w/ dispose pattern

    // todo: perhaps also ArraySegment<T> if T[] fails to implicitly convert to ReadOnlySpan<T>

    public abstract void Update<T>(ReadOnlySpan<T> data, int offset = 0) where T : unmanaged;

    // todo: map/unmap?
}

public sealed class VertexBuffer : Buffer
{
    public readonly VertexBufferDescription Description;

    public VertexBuffer(uint capacity, VertexBufferDescription description)
        : base(capacity)
    {
        Description = description;
    }
}

public readonly struct VertexBufferDescription
{
    // specifies the vertex structure as a whole
    public readonly IReadOnlyList<VertexAttributeDescription> Attributes;

    // specifies the byte distance between vertices
    public readonly uint Stride;

    // specifies if this buffer is read per-vertex or per-instance
    public readonly bool PerInstance;
}

public readonly struct VertexAttributeDescription
{
    // specifies the byte offset into the vertex structure
    public readonly uint Offset;

    public readonly uint Location; // + ElementIndex (ie, 4x4 matrix is "4 elements")

    public readonly uint Format; // todo: format enum 

    // public readonly bool IsNormalized;
}

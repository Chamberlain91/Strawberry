namespace Strawberry;

public abstract class Buffer
{
    protected Buffer(uint capacity)
    {
        Capacity = capacity;
    }

    public uint Capacity { get; }

    // todo: map/unmap, subdata
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

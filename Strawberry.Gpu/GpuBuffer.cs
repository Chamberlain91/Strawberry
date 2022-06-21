using System.Reflection;
using System.Runtime.InteropServices;

namespace Strawberry.Gpu;

public sealed class GpuBuffer : IDisposable
{
    public GpuBuffer(uint capacity)
    {
        Capacity = capacity;
    }

    public uint Capacity { get; }

    // todo: implement IDisposable explicit w/ dispose pattern

    // todo: perhaps also ArraySegment<T> if T[] fails to implicitly convert to ReadOnlySpan<T>

    public abstract void Update<T>(ReadOnlySpan<T> data, int offset = 0) where T : unmanaged;

    // todo: map/unmap?

    public static GpuBuffer CreateStructured<T>(uint count) where T : unmanaged
    {
        return new GpuBuffer((uint)(count * Marshal.SizeOf<T>()));
    }
}

// vertex buffers    -> describe graphics input  (1+ buffers + attributes)
// graphics pipeline -> describes how to draw    (shader program, cull, blend, depth, etc)
// frame buffers     -> describe graphics output (1* color textures + 1? depth textures)

public sealed class VertexBufferDescription
{
    // specifies the structure as a whole
    public readonly IReadOnlyList<VertexAttributeDescription> Attributes;

    // specifies the byte distance between vertices
    public readonly uint Stride;

    // specifies if this buffer should be read per-vertex or per-instance
    public readonly bool PerInstance;

    public VertexBufferDescription(IReadOnlyList<VertexAttributeDescription> attributes, bool perInstance = false)
    {
        PerInstance = perInstance;
        Attributes = attributes;
        Stride = ComputeStride(attributes);
    }

    public static VertexBufferDescription Create<T>(bool perInstance = false) where T : unmanaged
    {
        var attributes = GenerateAttributes<T>();

        return new VertexBufferDescription(attributes, perInstance);
    }

    private static VertexAttributeDescription[] GenerateAttributes<T>() where T : unmanaged
    {
        throw new NotImplementedException();
    }

    private static uint ComputeStride(IReadOnlyList<VertexAttributeDescription> attributes)
    {
        // TODO: Validate attributes fully pack space?
        throw new NotImplementedException();
    }
}

public readonly struct VertexAttributeDescription
{
    // Vertex Format
    public readonly ElementType Type;  // ex: Float32
    public readonly uint VectorLength; // ex: 3
    public readonly uint VectorCount;  // ex: 1
    public readonly bool IsNormalized; // ex: false

    // NOTE: Attributes of certain types (ex, mat4) can span multiple attribute locations...
    public readonly VertexAttribute Attribute; // + ElementIndex (ie, 4x4 matrix is "4 elements")

    // Usually zero...
    public readonly uint BufferIndex;

    public VertexAttributeDescription(VertexAttribute attribute, ElementType type, uint vectorLength, uint vectorCount = 1, bool isNormalized = false, uint bufferIndex = 0)
    {
        Type = type;
        VectorLength = vectorLength;
        VectorCount = vectorCount;
        IsNormalized = isNormalized;
        Attribute = attribute;
        BufferIndex = bufferIndex;
    }

    internal static VertexAttributeDescription Create(FieldInfo field, uint bufferIndex)
    {
        var attr = field.GetCustomAttribute<VertexDescriptionAttribute>();
        if (attr == null)
        {
            // TODO: Validate the field is defined on a blittable-struct?
            throw new InvalidOperationException("This field must have must have a VertexAttributeAttribute.");
        }

        // TODO: Or perhaps do name matching?
        throw new NotImplementedException();
    }

    public static VertexAttributeDescription[] CreateFrom<T>() where T : unmanaged
    {
        throw new NotImplementedException();
    }
}

[AttributeUsage(AttributeTargets.Field)]
public sealed class VertexDescriptionAttribute : Attribute
{
    public bool IsNormalized { get; init; }

    public uint Location { get; init; }

    public VertexDescriptionAttribute()
    {
        // does nothing
    }

    public VertexDescriptionAttribute(VertexAttribute attribute)
    {
        Location = (uint)attribute;
    }
}

public enum ElementType : uint
{
    Byte,
    UnsignedByte,
    Short,
    UnsignedShort,
    Integer,
    UnsignedInteger,
    Float16,
    Float32
}

public enum IndexElementType : uint
{
    UnsignedInt,
    UnsignedShort
}

public abstract class Mesh
{
    // Mutate Vertex Buffer
    public abstract void SetVertexParams(uint vertexCount, IReadOnlyList<VertexAttributeDescription> attributes);                       // allocate relevant buffers?
    public abstract void SetVertexBufferData<T>(Span<T> data, uint bufferOffset = 0, uint bufferIndex = 0) where T : unmanaged; // popilate relevant buffer?

    // Mutate Index Buffer
    public abstract void SetIndexParams(uint indexCount, IndexElementType indexType = IndexElementType.UnsignedInt);
    public abstract void SetIndexBufferData<T>(Span<T> data, uint bufferOffset = 0, uint bufferIndex = 0) where T : unmanaged;

    // Aceess Vertex Buffer
    public abstract GpuBuffer GetVertexBuffer(uint bufferIndex = 0); // unity: reference can become "invalid" with "SetVertexBufferData"
    public abstract uint GetVertexBufferStride(uint bufferIndex = 0);

    // Access Vertex Attributes
    public abstract int VertexAttributeCount { get; }
    public abstract bool HasVertexAttribute(VertexAttribute attribute);
    public abstract VertexAttributeDescription GetVertexAttributeDescription(VertexAttribute attribute);
    public abstract VertexAttributeDescription GetVertexAttributeDescription(int index);
    public abstract int GetVertexAttributeBufferIndex(VertexAttribute attribute);
    public abstract int GetVertexAttributeOffset(VertexAttribute attribute);

    // ...?
    public abstract void ComputeNormals();
    public abstract void ComputeTangents();
    public abstract void ComputeBounds();
}

public enum VertexAttribute : uint
{
    Position = 0,     // vec3
    Normal = 1,       // vec3
    Tangent = 2,      // vec3
    Color = 3,        // vec4
    UV = 4,           // vec2
    LightmapUV = 5,   // vec2
    BlendWeight = 6,  // vec4
    BlendIndices = 7, // vec4
    // todo: extra
}

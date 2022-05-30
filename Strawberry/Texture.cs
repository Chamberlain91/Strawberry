namespace Strawberry;

public abstract class Texture
{
    public TextureFormat Format { get; }

    public int MipCount { get; }

    internal Texture(int width, int height, int depth, TextureFormat format, bool enableMips)
    {
        throw new NotImplementedException();
    }

    public static bool IsColorRenderable(TextureFormat format)
    {
        // determines if can be attached to a framebuffer as a color attachment
        throw new NotImplementedException();
    }

    public static bool IsDepthRenderable(TextureFormat format)
    {
        // determines if can be attached to a framebuffer as the depth attachment
        throw new NotImplementedException();
    }

    public static bool IsCompressed(TextureFormat format)
    {
        // ETC2
        throw new NotImplementedException();
    }
}

public sealed class Texture2D : Texture
{
    public Texture2D(int width, int height, TextureFormat format, bool enableMips = true)
        : base(width, height, 1, format, enableMips)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }

    // todo: ReadOnlySpan<T>

    public void Update<T>(Span<T> data) where T : unmanaged
    {
        Update(0, 0, Width, Height, data);
    }

    public abstract void Update<T>(int offsetX, int offsetY, int width, int height, Span<T> data) where T : unmanaged;
}

public sealed class Texture2DArray : Texture
{
    public Texture2DArray(int width, int height, int count, TextureFormat format, bool enableMips = true)
        : base(width, height, count, format, enableMips)
    {
        Width = width;
        Height = height;
        Count = count;
    }

    public int Width { get; }

    public int Height { get; }

    public int Count { get; }

    public void Update<T>(int layer, Span<T> data) where T : unmanaged
    {
        Update(0, 0, Width, Height, layer, data);
    }

    public abstract void Update<T>(int offsetX, int offsetY, int width, int height, int layer, Span<T> data) where T : unmanaged;
}

public sealed class Texture3D : Texture
{
    public Texture3D(int width, int height, int depth, TextureFormat format, bool enableMips = true)
        : base(width, height, depth, format, enableMips)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }

    public int Width { get; }

    public int Height { get; }

    public int Depth { get; }

    public void Update<T>(Span<T> data) where T : unmanaged
    {
        Update(0, 0, 0, Width, Height, Depth, data);
    }

    public abstract void Update<T>(int offsetX, int offsetY, int offsetZ, int width, int height, int depth, Span<T> data) where T : unmanaged;
}

public sealed class TextureCube : Texture
{
    public TextureCube(int size, TextureFormat format, bool enableMips = true)
        : base(size, size, 1, format, enableMips)
    {
        Width = size;
    }

    public int Width { get; }

    public int Height => Width;

    public void Update<T>(TextureCubeFace face, Span<T> data) where T : unmanaged
    {
        Update(0, 0, Width, Height, face, data);
    }

    public abstract void Update<T>(int offsetX, int offsetY, int width, int height, TextureCubeFace face, Span<T> data) where T : unmanaged;
}

public sealed class TextureSampler
{
    // texture filtering
    public TextureFilter Filter = TextureFilter.Linear;

    // texture wrapping
    public TextureWrap WrapS = TextureWrap.Repeat;
    public TextureWrap WrapT = TextureWrap.Repeat;
    public TextureWrap WrapR = TextureWrap.Repeat;

    // depth texture comparison
    public bool EnableDepthComparison = true;
    public CompareFunc DepthComparison = CompareFunc.LessEqual;
}

public enum TextureFilter
{
    Nearest,
    Linear
}

public enum TextureFormat
{
    // standard color formats

    R8,
    RG8,
    RGB8,
    RGBA8,

    // todo: determine if to use integer formats
    // (R,RG,RGB,RGBA)(8,16,32)(,U,I)

    // standard depth formats

    Depth16,
    Depth24,
    Depth32F,

    // standard compressed formats?

    ETC2_RGB8,
    ETC2_RGB8_A1,
    ETC2_RGBA8,

    ETC2_SRGB8,
    ETC2_SRGB8_A1,
    ETC2_SRGB8_A8,

    // 16-bit float (when supported)

    R16F,
    RG16F,
    RGB16F,
    RGBA16F,

    // 32-bit float (when supported)

    R32F,
    RG32F,
    RGB32F,
    RGBA32F,

    // special formats

    RGB565,  // 16-bit encoding
    RGB5_A1, // 16-bit encoding
    RGBA4,   // 16-bit encoding

    SRGBA8, // sRGB (jpeg, png, etc)
}

public enum TextureWrap
{
    Repeat,
    MirrorRepeat,
    Clamp,
}

public enum TextureCubeFace : uint
{
    PositiveX,
    NegativeX,
    PositiveY,
    NegativeY,
    PositiveZ,
    NegativeZ,
}

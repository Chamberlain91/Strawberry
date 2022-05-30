namespace Strawberry;

public abstract class Texture
{
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

    public static bool IsStencilRenderable(TextureFormat format)
    {
        // determines if can be attached to a framebuffer as the stencil attachment
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

}

public sealed class Texture2DMS : Texture // ...?
{

}

public sealed class Texture2DArray : Texture
{

}

public sealed class Texture3D : Texture
{

}

public sealed class TextureCube : Texture
{

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
    // todo: determine preferable subset to use
    // (R,RG,RGB,RGBA)(8,16,32)(,U,I), etc
}

public enum TextureWrap
{
    Repeat,
    MirrorRepeat,
    Clamp,
}

public enum TextureCubeFace
{
    PositiveX,
    NegativeX,
    PositiveY,
    NegativeY,
    PositiveZ,
    NegativeZ,
}

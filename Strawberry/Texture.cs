namespace Strawberry;

public enum TextureFilter
{
    Nearest,
    Linear
}

public enum TextureWrap
{
    Repeat,
    MirrorRepeat,
    Clamp,
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

public enum TextureFormat
{
    // todo: determine preferable subset to use
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

public struct Viewport
{
    // todo: or Rectangle?!
}

public abstract class Graphics
{
    public abstract GraphicsScope BeginGraphics(Framebuffer? framebuffer, Viewport? viewport = null);
    public abstract ComputeScope BeginCompute();


    private void test()
    {
        using var g0 = BeginGraphics(null, null);
        
        g0.Draw(...);

        using var g1 = BeginGraphics(framebuffer, null);
        
        g0.Draw(...);
    }
}

public ref struct GraphicsScope
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public ref struct ComputeScope
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
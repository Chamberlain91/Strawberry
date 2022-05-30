namespace Strawberry.Gpu;

public sealed class Framebuffer : IDisposable
{
    public FramebufferAttachment[] ColorAttachments { get; }

    public FramebufferAttachment? DepthAttachment { get; }

    public Framebuffer(Span<FramebufferAttachment> colorAttachments, FramebufferAttachment? depthAttachment = null)
    {
        // todo: validate colorAttachments are actually color renderable
        // todo: validate depthAttachment is actually depth format
        throw new NotImplementedException();
    }

    // todo: implement IDisposable explicit w/ dispose pattern
}

public readonly struct FramebufferAttachment
{
    public FramebufferAttachment(Texture2D texture, int mipLevel = 0)
        : this((Texture)texture, 0, mipLevel)
    { }

    public FramebufferAttachment(Texture2DArray texture, int layer, int mipLevel = 0)
        : this((Texture)texture, layer, mipLevel)
    { }

    public FramebufferAttachment(TextureCube texture, TextureCubeFace face, int mipLevel = 0)
        : this((Texture)texture, (int)face, mipLevel)
    { }

    public FramebufferAttachment(Texture3D texture, int layer, int mipLevel = 0)
        : this((Texture)texture, layer, mipLevel)
    { }

    private FramebufferAttachment(Texture texture, int layer, int mipLevel)
    {
        Texture = texture;
        MipLevel = mipLevel;
        Layer = layer;
    }

    public readonly Texture Texture;

    public readonly int MipLevel;

    public readonly int Layer; // Cast to TextureCubeFace if TextureCube
}

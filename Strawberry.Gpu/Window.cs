namespace Strawberry.Gpu;

public abstract class Window
{
    public Framebuffer Framebuffer { get; }

    public GpuContext Gpu { get; }
    
    // TODO: Display Mode Abstraction?
    // TODO: Monitor Abstraction?
    // TODO: Input Abstraction?

    // TODO: Window Events (Size Change, Focus Change)

    // TODO: VSync?

    // TODO: Keyboard Events (Down, Up, Repeat, Text, IsSupported)
    // TODO: Mouse Events    (Down, Up, Move, Wheel, IsSupported)
    // TODO: Touch Events    (Down, Up, Move, IsSupported)

    // DESKTOP FEATURES (GLFW)

    // TODO: State (Normal, Maximize, Minimize)
    // TODO: Fullscreen (Set Display, Mode)

    // TODO: Window Style (Icon, Text, Borderless)
    // TODO: Cursor Style (Icon, Show, Hide, Grab Cursor, Coordinate Mode)
    // TODO: Clipboard, Path Drop
}
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VertexEngine.Common.Rendering;

public static class Viewport
{
    private static readonly Dictionary<FrameBuffer, Vector2i> Sizes = new()
    {
        [FrameBuffer.Default] = Vector2i.Zero
    };

    public static FrameBuffer ActiveFrameBuffer { get; private set; } = FrameBuffer.Default;

    public static event EventHandler<ViewportSizeChangedEventArgs>? SizeChanged;

    public static Vector2i Size
    {
        get => GetSize(ActiveFrameBuffer);
        set => SetSize(ActiveFrameBuffer, value);
    }

    public static Vector2i GetSize(FrameBuffer frameBuffer) => Sizes.GetValueOrDefault(frameBuffer);

    public static void SetSize(FrameBuffer frameBuffer, Vector2i size)
    {
        if (GetSize(frameBuffer) == size) return;

        Sizes[frameBuffer] = size;

        if (frameBuffer == ActiveFrameBuffer)
            GL.Viewport(0, 0, size.X, size.Y);

        SizeChanged?.Invoke(null, new ViewportSizeChangedEventArgs(frameBuffer, size));
    }

    internal static void OnFramebufferBound(FrameBuffer frameBuffer)
    {
        ActiveFrameBuffer = frameBuffer;

        var size = GetSize(frameBuffer);
        if (size != Vector2i.Zero)
            GL.Viewport(0, 0, size.X, size.Y);
    }
}

using OpenTK.Mathematics;

namespace VertexEngine.Common.Rendering;

public class ViewportSizeChangedEventArgs(FrameBuffer frameBuffer, Vector2i size) : EventArgs
{
    public FrameBuffer FrameBuffer { get; } = frameBuffer;

    public Vector2i Size { get; } = size;
}

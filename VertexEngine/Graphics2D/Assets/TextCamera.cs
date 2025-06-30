using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Rendering;

namespace VertexEngine.Graphics2D.Assets;

public class TextCamera : Camera
{
    public TextCamera()
    {
        UpdateView(Viewport.Size);
    }

    private void UpdateView(Vector2i size)
    {
        ViewMatrix = Matrix4.CreateOrthographicOffCenter(0, size.X, size.Y, 0, 0, -1);
    }
}
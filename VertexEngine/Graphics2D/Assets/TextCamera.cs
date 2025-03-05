using OpenTK.Mathematics;
using VertexEngine.Common.Assets;

namespace VertexEngine.Graphics2D.Assets;

public class TextCamera : Camera
{
    public TextCamera()
    {
        UpdateView(GameWindow.CurrentWindowSize);
    }

    private void UpdateView(Vector2i size)
    {
        ViewMatrix = Matrix4.CreateOrthographicOffCenter(0, size.X, size.Y, 0, 0, -1);
    }
}
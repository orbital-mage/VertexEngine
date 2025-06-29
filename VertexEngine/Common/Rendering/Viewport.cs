using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VertexEngine.Common.Rendering;

public static class Viewport
{
    private static Vector2i size;

    public static Vector2i Size
    {
        get => size;
        set
        {
            GL.Viewport(0, 0, value.X, value.Y);
            size = value;
        }
    }
}
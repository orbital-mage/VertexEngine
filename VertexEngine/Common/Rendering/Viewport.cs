using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VertexEngine.Common.Rendering;

public static class Viewport
{
    private static Vector2i size;

    public static event EventHandler? SizeChanged;

    public static Vector2i Size
    {
        get => size;
        set
        {
            if (size == value) return;

            GL.Viewport(0, 0, value.X, value.Y);
            size = value;
            SizeChanged?.Invoke(null, EventArgs.Empty);
        }
    }
}
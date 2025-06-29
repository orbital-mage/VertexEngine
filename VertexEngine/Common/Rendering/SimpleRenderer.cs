using OpenTK.Mathematics;
using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering;

public class SimpleRenderer(IElement root, Vector2i viewportSize) : TreeRenderer(root)
{
    public virtual FrameBuffer FrameBuffer { get; set; } = FrameBuffer.Default;

    public virtual Vector2i ViewportSize { get; set; } = viewportSize;

    public override void Draw()
    {
        Use();
        base.Draw();
    }

    public void Use()
    {
        FrameBuffer.Bind();
        Viewport.Size = ViewportSize;
    }
}
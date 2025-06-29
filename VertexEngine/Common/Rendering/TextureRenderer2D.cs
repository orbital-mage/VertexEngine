using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering;

public class TextureRenderer2D : SimpleRenderer
{
    private Vector2i viewportSize;
    private readonly FrameBuffer frameBuffer = FrameBuffer.Create();

    public TextureRenderer2D(IElement root, Vector2i viewportSize) : base(root, viewportSize)
    {
        this.viewportSize = viewportSize;

        Texture = Texture2D.Empty(viewportSize);
        frameBuffer.AttachTexture(Texture, FramebufferAttachment.ColorAttachment0);
    }

    public override FrameBuffer FrameBuffer => frameBuffer;

    public override Vector2i ViewportSize
    {
        get => viewportSize;
        set
        {
            viewportSize = value;
            Texture = Texture2D.Empty(ViewportSize);
        }
    }

    public Texture2D Texture { get; private set; }
}
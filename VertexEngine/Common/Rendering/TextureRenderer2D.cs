using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering;

public class TextureRenderer2D : SimpleRenderer
{
    private int numberOfSamples = 1;
    private Vector2i viewportSize;
    private readonly FrameBuffer frameBuffer = FrameBuffer.Create();
    private FrameBuffer? msaaFrameBuffer;
    private RenderBufferObject? msaaColorBuffer;
    private RenderBufferObject? msaaDepthBuffer;

    public TextureRenderer2D(IElement root, Vector2i viewportSize) : base(root, viewportSize)
    {
        this.viewportSize = viewportSize;

        Texture = Texture2D.Empty(viewportSize);
        frameBuffer.AttachTexture(Texture, FramebufferAttachment.ColorAttachment0);
    }

    public override FrameBuffer FrameBuffer =>
        NumberOfSamples > 1 && msaaFrameBuffer != null ? msaaFrameBuffer : frameBuffer;

    public override Vector2i ViewportSize
    {
        get => viewportSize;
        set
        {
            if (viewportSize == value) return;

            viewportSize = value;
            Texture = Texture2D.Empty(viewportSize);
            frameBuffer.AttachTexture(Texture, FramebufferAttachment.ColorAttachment0);
            UpdateMsaaFrameBuffer();
        }
    }

    public Texture2D Texture { get; private set; }

    public int NumberOfSamples
    {
        get => numberOfSamples;
        set
        {
            if (numberOfSamples == value) return;

            numberOfSamples = value;
            UpdateMsaaFrameBuffer();
        }
    }

    public override void Draw()
    {
        base.Draw();

        if (NumberOfSamples > 1 && msaaFrameBuffer != null)
            msaaFrameBuffer.BlitTo(frameBuffer, viewportSize);
    }
    
    private void UpdateMsaaFrameBuffer()
    {
        if (numberOfSamples <= 1)
        {
            msaaFrameBuffer = null;
            msaaColorBuffer = null;
            msaaDepthBuffer = null;
            return;
        }

        GL.Enable(EnableCap.Multisample);

        msaaFrameBuffer ??= FrameBuffer.Create();
        msaaColorBuffer ??= new RenderBufferObject();
        msaaDepthBuffer ??= new RenderBufferObject();

        msaaColorBuffer.SetRenderBufferStorage(RenderbufferStorage.Rgba8, viewportSize, numberOfSamples);
        msaaDepthBuffer.SetRenderBufferStorage(RenderbufferStorage.Depth24Stencil8, viewportSize, numberOfSamples);

        msaaFrameBuffer.AttachRenderBuffer(msaaColorBuffer, FramebufferAttachment.ColorAttachment0);
        msaaFrameBuffer.AttachRenderBuffer(msaaDepthBuffer, FramebufferAttachment.DepthStencilAttachment);
        msaaFrameBuffer.CheckStatus();
    }
}

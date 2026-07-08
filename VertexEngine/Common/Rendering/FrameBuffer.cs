using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Graphics3D.Assets;

namespace VertexEngine.Common.Rendering
{
    public class FrameBuffer
    {
        public static readonly FrameBuffer Default = new(0);

        public static FrameBuffer Create()
        {
            return new FrameBuffer(GL.GenFramebuffer());
        }

        private readonly int handle;

        private FrameBuffer(int handle)
        {
            this.handle = handle;
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
            Viewport.OnFramebufferBound(this);
        }

        public void AttachTexture(Texture texture, FramebufferAttachment attachment)
        {
            switch (texture)
            {
                case Texture2D texture2D:
                    AttachTexture(texture2D, attachment);
                    break;
                case Cubemap cubemap:
                    AttachTexture(cubemap, attachment);
                    break;
            }
        }

        public void AttachTexture(Texture2D texture, FramebufferAttachment attachment)
        {
            Bind();
            texture.Bind();
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,
                attachment,
                TextureTarget.Texture2D,
                GL.GetInteger(GetPName.TextureBinding2D),
                0);
        }

        public void AttachTexture(Cubemap cubemap, FramebufferAttachment attachment)
        {
            Bind();
            cubemap.Bind();
            GL.FramebufferTexture(FramebufferTarget.Framebuffer,
                attachment,
                // TextureTarget.TextureCubeMap,
                GL.GetInteger(GetPName.TextureBindingCubeMap),
                0);
        }

        public void AttachRenderBuffer(RenderBufferObject renderBuffer, FramebufferAttachment attachment)
        {
            Bind();
            renderBuffer.Bind();
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer,
                attachment,
                RenderbufferTarget.Renderbuffer,
                GL.GetInteger(GetPName.RenderbufferBinding));
        }

        public void BlitTo(FrameBuffer target,
            Vector2i size,
            ClearBufferMask mask = ClearBufferMask.ColorBufferBit,
            BlitFramebufferFilter filter = BlitFramebufferFilter.Nearest)
        {
            GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, handle);
            GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, target.handle);
            GL.BlitFramebuffer(0, 0, size.X, size.Y, 0, 0, size.X, size.Y, mask, filter);
            
            target.Bind();
        }

        public void SetDrawBuffer(DrawBufferMode mode)
        {
            Bind();
            GL.DrawBuffer(mode);
        }

        public void SetReadBuffer(ReadBufferMode mode)
        {
            Bind();
            GL.ReadBuffer(mode);
        }

        public void CheckStatus()
        {
            Bind();

            if (GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer) != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception("Frame buffer status bad");
            }
        }
    }
}
using OpenTK.Graphics.OpenGL;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Graphics3D.Assets;

namespace VertexEngine.Common.Rendering
{
    public class FrameBuffer
    {
        public static void BindDefault()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        private readonly int handle = GL.GenFramebuffer();

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, handle);
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
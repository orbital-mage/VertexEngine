using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace EngineTK.Common.Rendering
{
    public class RenderBufferObject
    {
        private readonly int handle;

        public RenderBufferObject()
        {
            handle = GL.GenRenderbuffer();
        }

        public RenderBufferObject(RenderbufferStorage storage, Vector2i size)
        {
            handle = GL.GenRenderbuffer();
            SetRenderBufferStorage(storage, size);
        }

        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, handle);
        }

        public void SetRenderBufferStorage(RenderbufferStorage storage, Vector2i size)
        {
            Bind();
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, size.X, size.Y);
        }
    }
}
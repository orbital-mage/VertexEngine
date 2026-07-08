using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VertexEngine.Common.Rendering
{
    public class RenderBufferObject
    {
        private readonly int handle;

        public RenderBufferObject()
        {
            handle = GL.GenRenderbuffer();
        }

        public RenderBufferObject(RenderbufferStorage storage, Vector2i size, int samples = 1)
        {
            handle = GL.GenRenderbuffer();
            SetRenderBufferStorage(storage, size, samples);
        }

        public void Bind()
        {
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, handle);
        }

        public void SetRenderBufferStorage(RenderbufferStorage storage, Vector2i size, int samples = 1)
        {
            Bind();
            
            if (samples > 1)
                GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, samples, storage, size.X, size.Y);
            else
                GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, storage, size.X, size.Y);
        }
    }
}
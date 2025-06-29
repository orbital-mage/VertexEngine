using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Rendering;

namespace VertexEngine.Graphics3D.Assets.Lights.Shadows
{
    public abstract class ShadowMap
    {
        private const string ShadowMapUniform = "shadowMap.map";

        private Texture texture;

        protected ShadowMap(Texture texture)
        {
            this.texture = texture;
            
            FrameBuffer.SetDrawBuffer(DrawBufferMode.None);
            FrameBuffer.SetReadBuffer(ReadBufferMode.None);
        }

        public FrameBuffer FrameBuffer { get; } = FrameBuffer.Create();

        public virtual Dictionary<string, object> Uniforms => new()
        {
            // { ShadowMapUniform, Texture }
        };

        public abstract Shader Shader { get; }

        public Texture Texture
        {
            get => texture;
            set
            {
                texture = value;
                FrameBuffer.AttachTexture(texture, FramebufferAttachment.DepthAttachment);
            }
        }

        protected abstract Vector2i Size { get; }

        public virtual void Bind()
        {
            FrameBuffer.Bind();
            Viewport.Size = Size;
        }
    }
}
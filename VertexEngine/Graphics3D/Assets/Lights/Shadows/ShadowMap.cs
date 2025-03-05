﻿using OpenTK.Graphics.OpenGL;
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

        protected ShadowMap()
        {
            FrameBuffer.SetDrawBuffer(DrawBufferMode.None);
            FrameBuffer.SetReadBuffer(ReadBufferMode.None);
        }

        public FrameBuffer FrameBuffer { get; } = new();

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
            GL.Viewport(0, 0, Size.X, Size.Y);
        }
    }
}
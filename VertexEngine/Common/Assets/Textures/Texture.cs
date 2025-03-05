using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Utils;
using GLPixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace VertexEngine.Common.Assets.Textures
{
    public abstract class Texture : IIndexedAsset
    {
        private readonly int handle;

        protected abstract TextureTarget Type { get; }

        protected Texture()
        {
            handle = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture31);
            Bind();
        }

        public void Use(Dictionary<Type, IAsset> assets, int unit)
        {
            Use(unit);
        }

        public void Use(int unit)
        {
            GL.ActiveTexture(TextureUnit.Texture0 + unit);
            Bind();
        }

        public void Bind()
        {
            GL.BindTexture(Type, handle);
        }

        public void SetMinMagFilter(TextureMinFilter minFilter, TextureMagFilter magFilter)
        {
            Bind();

            GL.TexParameter(Type, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(Type, TextureParameterName.TextureMinFilter, (int)magFilter);
        }

        public void SetWrapMode(TextureWrapMode wrapMode)
        {
            Bind();

            GL.TexParameter(Type, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(Type, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(Type, TextureParameterName.TextureWrapR, (int)wrapMode);
        }

        public int CompareTo(object? obj)
        {
            return GetHashCode().CompareTo(obj?.GetHashCode());
        }

        public override string ToString()
        {
            return $"{GetType().Name}[{handle.ToString()}]";
        }

        protected static ImageResult LoadImageResultFromFile(string file, bool flipY = true)
        {
            StbImage.stbi_set_flip_vertically_on_load(flipY ? 1 : 0);
            return ResourceUtils.ReadImageResource(file);
        }

        protected static void LoadImageData(ImageResult image, TextureTarget target)
        {
            var data = image.Data;

            GL.TexImage2D(target,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                GLPixelFormat.Rgba,
                PixelType.UnsignedByte,
                data);
        }

        protected static void LoadEmptyImageData(
            Vector2i size,
            TextureTarget target,
            PixelInternalFormat internalFormat,
            GLPixelFormat format,
            PixelType type)
        {
            GL.TexImage2D(target,
                0,
                internalFormat,
                size.X,
                size.Y,
                0,
                format,
                type,
                IntPtr.Zero);
        }

        protected static void LoadSubImageData(Vector2i offset, Vector2i size, byte[] data, TextureTarget target,
            GLPixelFormat format,
            PixelType type)
        {
            GL.TexSubImage2D(target,
                0,
                offset.X,
                offset.Y,
                size.X,
                size.Y,
                format,
                type,
                data);
        }
    }
}
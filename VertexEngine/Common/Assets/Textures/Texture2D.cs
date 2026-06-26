using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;

namespace VertexEngine.Common.Assets.Textures
{
    public class Texture2D : Texture
    {
        private readonly PixelFormat format;
        private readonly PixelType type;

        public Vector2i Size { get; }

        protected override TextureTarget Type => TextureTarget.Texture2D;

        public static Texture2D LoadFromFile(string file)
        {
            return new Texture2D(LoadImageResultFromFile(file));
        }

        public static Texture2D Empty(Vector2i size,
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba,
            PixelFormat format = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte,
            TextureMinFilter minFilter = TextureMinFilter.Nearest,
            TextureMagFilter magFilter = TextureMagFilter.Nearest,
            TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            bool generateMipmap = true)
        {
            return new Texture2D(size, internalFormat, format, type, minFilter, magFilter, wrapMode, generateMipmap);
        }

        private Texture2D(ImageResult image)
        {
            Size = new Vector2i(image.Width, image.Height);

            LoadImageData(image, TextureTarget.Texture2D);
            SetTextureParameters();
        }

        private Texture2D(Vector2i size,
            PixelInternalFormat internalFormat,
            PixelFormat format,
            PixelType type,
            TextureMinFilter minFilter,
            TextureMagFilter magFilter,
            TextureWrapMode wrapMode,
            bool generateMipmap)
        {
            this.format = format;
            this.type = type;
            Size = size;

            LoadEmptyImageData(size, TextureTarget.Texture2D, internalFormat, format, type);
            SetTextureParameters(minFilter, magFilter, wrapMode, generateMipmap);
        }

        public void SetBorderColor(Vector4 color)
        {
            Bind();

            GL.TexParameter(TextureTarget.Texture2D,
                TextureParameterName.TextureBorderColor,
                [color.X, color.Y, color.Z, color.Y]);
        }

        public void SetSubImageData(Vector2i offset, Vector2i size, byte[] data)
        {
            Bind();

            LoadSubImageData(offset, size, data, TextureTarget.Texture2D, format, type);
        }

        private static void SetTextureParameters(
            TextureMinFilter minFilter = TextureMinFilter.Nearest,
            TextureMagFilter magFilter = TextureMagFilter.Nearest,
            TextureWrapMode wrapMode = TextureWrapMode.Clamp,
            bool generateMipmap = true)
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)magFilter);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);

            if (generateMipmap)
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
    }
}
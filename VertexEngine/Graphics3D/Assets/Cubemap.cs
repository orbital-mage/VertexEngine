using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using StbImageSharp;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Utils;

namespace VertexEngine.Graphics3D.Assets
{
    public class Cubemap : Texture
    {
        private static readonly string[] Faces =
        {
            "right",
            "left",
            "top",
            "bottom",
            "front",
            "back"
        };

        protected override TextureTarget Type => TextureTarget.TextureCubeMap;

        public static Cubemap FromDirectory(string directory)
        {
            var files = ResourceUtils.GetResources(directory);
            return FromFiles(Faces
                .Select(face => files.First(file => file.Contains(face))));
        }

        public static Cubemap FromFiles(IEnumerable<string> files)
        {
            return new Cubemap(files
                .Take(6)
                .Select(file => LoadImageResultFromFile(file, false))
                .ToArray());
        }

        public static Cubemap Empty(Vector2i size,
            PixelInternalFormat internalFormat = PixelInternalFormat.Rgba,
            PixelFormat format = PixelFormat.Rgba,
            PixelType type = PixelType.UnsignedByte)
        {
            return new Cubemap(Enumerable.Repeat(size, 6).ToArray(),
                internalFormat,
                format,
                type);
        }

        private Cubemap(IReadOnlyList<ImageResult> faces)
        {
            if (faces.Count != 6)
                throw new ArgumentException("Cubemap requires exactly 6 faces");

            for (var i = 0; i < 6; i++)
                LoadImageData(faces[i], TextureTarget.TextureCubeMapPositiveX + i);

            SetTextureParameters();
        }

        private Cubemap(IReadOnlyList<Vector2i> sizes,
            PixelInternalFormat internalFormat,
            PixelFormat format,
            PixelType type)
        {
            if (sizes.Count != 6)
                throw new ArgumentException("Cubemap requires exactly 6 faces");

            for (var i = 0; i < 6; i++)
                LoadEmptyImageData(sizes[i], 
                    TextureTarget.TextureCubeMapPositiveX + i, 
                    internalFormat, 
                    format, 
                    type);

            SetTextureParameters();
        }

        private static void SetTextureParameters()
        {
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Linear);

            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT,
                (int)TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR,
                (int)TextureWrapMode.ClampToEdge);
        }
    }
}
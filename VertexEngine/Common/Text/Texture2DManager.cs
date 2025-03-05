using System.Drawing;
using FontStashSharp.Interfaces;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Utils;

namespace VertexEngine.Common.Text;

public class Texture2DManager : ITexture2DManager
{
    public object CreateTexture(int width, int height) => Texture2D.Empty(new Vector2i(width, height));

    public Point GetTextureSize(object texture)
    {
        if (texture is not Texture2D texture2D) throw new ArgumentException("texture is not a Texture2D");

        return texture2D.Size.ToPoint();
    }

    public void SetTextureData(object texture, Rectangle bounds, byte[] data)
    {
        if (texture is not Texture2D texture2D) throw new ArgumentException("texture is not a Texture2D");

        texture2D.SetSubImageData(bounds.Location.ToVector2I(), bounds.Size.ToVector2I(), data);
    }
}
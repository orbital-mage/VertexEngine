using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Common.Elements.Interfaces
{
    public interface ITextureElement : IElement
    {
        public AssetSet<Texture> Textures { get; }
    }
}
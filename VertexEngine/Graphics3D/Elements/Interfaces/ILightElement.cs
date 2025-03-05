using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Elements;
using VertexEngine.Graphics3D.Assets.Lights;

namespace VertexEngine.Graphics3D.Elements.Interfaces
{
    public interface ILightElement : IElement
    {
        public AssetSet<Light> Lights { get; }
    }
}
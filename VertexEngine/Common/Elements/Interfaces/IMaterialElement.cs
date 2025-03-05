using VertexEngine.Common.Assets.Materials;

namespace VertexEngine.Common.Elements.Interfaces
{
    public interface IMaterialElement : ITextureElement
    {
        public IMaterial Material { get; }
    }
}
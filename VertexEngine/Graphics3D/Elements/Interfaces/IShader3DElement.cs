using VertexEngine.Common.Elements.Interfaces;

namespace VertexEngine.Graphics3D.Elements.Interfaces
{
    public interface IShader3DElement : IMaterialElement, ILightElement
    {
        public string VertexShader { get; }
        public string FragmentShader { get; }
    }
}
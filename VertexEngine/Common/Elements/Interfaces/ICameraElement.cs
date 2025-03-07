using VertexEngine.Common.Assets;

namespace VertexEngine.Common.Elements.Interfaces
{
    public interface ICameraElement : IElement
    {
        public Camera? Camera { get; }
    }

    public interface ICameraElement<T> : ICameraElement where T : Camera
    {
        Camera? ICameraElement.Camera => Camera;

        public new T? Camera { get; set; }
    }
}
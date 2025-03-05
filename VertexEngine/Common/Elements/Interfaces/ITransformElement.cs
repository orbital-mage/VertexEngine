using VertexEngine.Common.Assets;

namespace VertexEngine.Common.Elements.Interfaces
{
    public interface ITransformElement : IElement
    {
        public Transform Transform { get; }
    }

    public interface ITransformElement<out T> : ITransformElement where T : Transform
    {
        Transform ITransformElement.Transform => GlobalTransform;

        public T LocalTransform { get; }
        public T GlobalTransform { get; }
    }
}
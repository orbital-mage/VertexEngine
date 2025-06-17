using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Graphics2D.Assets;

namespace VertexEngine.Graphics2D.Elements.Interfaces;

public interface ITransformElement2D : ITransformElement<Transform2D>
{
    public bool UseScreenTransform { get; }
}
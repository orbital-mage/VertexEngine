using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering
{
    public interface IRenderTreeBranch
    {
        public void AddElement(IElement element);
        public void RemoveElement(IElement element);
        public void Draw();
        public void CollectGarbage();
        public bool IsEmpty();
    }
}
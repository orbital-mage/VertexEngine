using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering
{
    public class ElementSet : IRenderTreeBranch
    {
        private readonly HashSet<IElement> elementSet = new();

        public void AddElement(IElement element)
        {
            elementSet.Add(element);
        }

        public void RemoveElement(IElement element)
        {
            elementSet.Remove(element);
        }

        public void Draw()
        {
            foreach (var element in elementSet)
                element.Draw();
        }

        public void CollectGarbage()
        {
        }

        public bool IsEmpty()
        {
            return elementSet.Count == 0;
        }
    }
}
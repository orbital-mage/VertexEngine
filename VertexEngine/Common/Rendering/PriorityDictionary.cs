using VertexEngine.Common.Assets;
using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering
{
    public class PriorityDictionary(Func<IElement, IAsset?>[] assetGetters) : RenderTreeDictionary<int>
    {
        private readonly SortedSet<int> priorityOrder = [];

        public override void AddElement(IElement element)
        {
            base.AddElement(element);

            priorityOrder.Add(element.Priority);
        }

        public override void RemoveElement(IElement element)
        {
            var key = KeyReference[element];
            base.RemoveElement(element);

            if (!Dictionary.ContainsKey(key) || Dictionary[key].IsEmpty())
                priorityOrder.Remove(key);
        }

        public override void CollectGarbage()
        {
            base.CollectGarbage();

            foreach (var priority in priorityOrder.ToArray())
            {
                if (!Dictionary.ContainsKey(priority) || Dictionary[priority].IsEmpty())
                    priorityOrder.Remove(priority);
            }
        }

        public override void Draw()
        {
            foreach (var priority in priorityOrder)
                Dictionary[priority].Draw();
        }

        protected override int GetKey(IElement element)
        {
            return element.Priority;
        }

        protected override void InitBranch(int priority)
        {
            Dictionary[priority] = new AssetDictionary(assetGetters);
        }
    }
}
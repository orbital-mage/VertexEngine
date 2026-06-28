using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering
{
    public abstract class RenderTreeDictionary<T> : IRenderTreeBranch where T : notnull
    {
        private const int BranchThreshold = 100;

        protected readonly Dictionary<T, IRenderTreeBranch> Dictionary = new();
        protected readonly Dictionary<IElement, T> KeyReference = new();
        private readonly HashSet<T> emptyBranches = [];

        public virtual void AddElement(IElement element)
        {
            var key = GetKey(element);

            if (key == null) return;

            if (!Dictionary.ContainsKey(key))
                InitBranch(key);

            Dictionary[key].AddElement(element);
            KeyReference[element] = key;
            emptyBranches.Remove(key);
        }

        public virtual void RemoveElement(IElement element)
        {
            var key = KeyReference[element];
            Dictionary[key]?.RemoveElement(element);

            if (Dictionary[key].IsEmpty())
                emptyBranches.Add(key);

            KeyReference.Remove(element);
        }

        public abstract void Draw();

        public virtual void CollectGarbage()
        {
            if (emptyBranches.Count >= BranchThreshold)
            {
                foreach (var key in emptyBranches)
                {
                    Dictionary.Remove(key);
                }

                emptyBranches.Clear();
            }

            foreach (var branch in Dictionary.Values)
                branch.CollectGarbage();
        }

        public bool IsEmpty()
        {
            return Dictionary.Count <= emptyBranches.Count;
        }

        protected abstract T? GetKey(IElement element);

        protected abstract void InitBranch(T key);
    }
}
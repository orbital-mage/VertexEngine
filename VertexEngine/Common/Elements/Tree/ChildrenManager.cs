namespace VertexEngine.Common.Elements.Tree
{
    public class ChildrenManager(IElement element, Action<IElement> propagateAction, Action<IElement> dePropagateAction)
    {
        private readonly HashSet<IElement> children = [];
        private IElement? parent;

        public IEnumerable<IElement> Children => children;

        public IElement? Parent
        {
            get => parent;
            set
            {
                if (value != null) element.AddToParent(value);
            }
        }

        public void AddChild(IElement childToAdd)
        {
            if (children.Contains(childToAdd)) return;
            if (childToAdd.Parent != null) childToAdd.RemoveFromParent();

            children.Add(childToAdd);
            childToAdd.AddToParent(element);
            propagateAction.Invoke(childToAdd);
        }

        public void RemoveChild(IElement childToRemove)
        {
            if (!children.Contains(childToRemove)) return;

            children.Remove(childToRemove);
            childToRemove.RemoveFromParent();
            dePropagateAction.Invoke(childToRemove);
        }

        public void AddToParent(IElement newParent)
        {
            if (parent == newParent) return;

            parent = newParent;
            parent.AddChild(element);
        }

        public void RemoveFromParent()
        {
            if (parent == null) return;

            var oldParent = parent;
            parent = null;
            oldParent.RemoveChild(element);
        }
    }
}
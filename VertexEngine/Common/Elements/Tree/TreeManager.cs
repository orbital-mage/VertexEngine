namespace VertexEngine.Common.Elements.Tree
{
    public abstract class TreeManager<T> where T : IElement
    {
        protected T Element { get; }

        protected TreeManager(T element)
        {
            Element = element;
        }

        public abstract void Propagate(IElement child);
        public abstract void DePropagate(IElement child);

        protected void Propagate(Action<T> propagateAction)
        {
            foreach (var child in Element.Children)
                Propagate(child, propagateAction);
        }

        protected void Propagate<TArgs>(TArgs args, Action<T, TArgs> propagateAction)
        {
            foreach (var child in Element.Children)
                Propagate(child, args, propagateAction);
        }

        protected TAsset GetAsset<TAsset>(Func<T, TAsset> getter, TAsset defaultValue)
        {
            return Element.Parent is T parent ? getter.Invoke(parent) : defaultValue;
        }

        protected static void Propagate(IElement child, Action<T> propagateAction)
        {
            if (child is not T t) return;
            propagateAction.Invoke(t);
        }

        protected static void Propagate<TArgs>(IElement child, TArgs args, Action<T, TArgs> propagateAction)
        {
            if (child is not T t) return;
            propagateAction.Invoke(t, args);
        }
    }
}
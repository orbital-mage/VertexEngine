using VertexEngine.Common.Assets.Rendering;

namespace VertexEngine.Common.Elements.Tree
{
    public class RenderOptionsManager(IElement element) : TreeManager<IElement>(element)
    {
        private RenderOptions? renderOptions;

        public event EventHandler OptionsChanged;

        public RenderOptions? RenderOptions
        {
            get => renderOptions;
            set
            {
                if (renderOptions != null) renderOptions.OptionsChanged -= OnOptionsChanged;
                renderOptions = value;
                if (renderOptions != null) renderOptions.OptionsChanged += OnOptionsChanged;

                OnOptionsChanged();
            }
        }

        public override void Propagate(IElement child)
        {
            Propagate(child, PropagateAction);
        }

        public override void DePropagate(IElement child)
        {
            Propagate(child, DePropagateAction);
        }

        private void OnOptionsChanged(object sender, EventArgs args)
        {
            OnOptionsChanged();
        }

        private void OnOptionsChanged()
        {
            Propagate(PropagateAction);
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void PropagateAction(IElement child)
        {
            if (RenderOptions == null) return;
            child.RenderOptions ??= RenderOptions;
        }

        private static void DePropagateAction(IElement child)
        {
            child.RenderOptions = null;
        }
    }
}
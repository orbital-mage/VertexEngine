using VertexEngine.Common.Assets;
using VertexEngine.Common.Elements.Interfaces;

namespace VertexEngine.Common.Elements.Tree
{
    public class CameraManager<T> : TreeManager<ICameraElement<T>> where T : Camera
    {
        private T camera;

        public event EventHandler CameraChanged;

        public CameraManager(ICameraElement<T> element) : base(element)
        {
        }

        public T Camera
        {
            get => camera;
            set
            {
                camera = value;
                Propagate(PropagateAction);
                CameraChanged?.Invoke(this, EventArgs.Empty);
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

        private void PropagateAction(ICameraElement<T> child)
        {
            child.Camera = Camera;
        }

        private static void DePropagateAction(ICameraElement<T> child)
        {
            child.Camera = null;
        }
    }
}
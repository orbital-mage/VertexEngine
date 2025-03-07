using OpenTK.Mathematics;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics3D.Assets;

namespace VertexEngine.Graphics3D.Elements.Tree
{
    public class Transform3DManager : TreeManager<ITransformElement<Transform3D>>
    {
        private Transform3D localTransform = new();
        private Transform3D globalTransform = new();

        public event EventHandler? TransformChanged;

        public Transform3DManager(ITransformElement<Transform3D> element) : base(element)
        {
            localTransform.TransformChanged += OnLocalChange;
            globalTransform.TransformChanged += OnGlobalChange;
        }

        public Transform3D LocalTransform
        {
            get => localTransform;
            set
            {
                localTransform.TransformChanged -= OnLocalChange;
                localTransform = value;
                localTransform.TransformChanged += OnLocalChange;
                OnLocalChange();
            }
        }

        public Transform3D GlobalTransform
        {
            get => globalTransform;
            set
            {
                globalTransform.TransformChanged -= OnGlobalChange;
                globalTransform = value;
                globalTransform.TransformChanged += OnGlobalChange;
                OnGlobalChange();
            }
        }

        public override void Propagate(IElement child)
        {
            Propagate(child, GlobalTransform.Matrix, PropagateAction);
        }

        public override void DePropagate(IElement child)
        {
            Propagate(child, DePropagateAction);
        }

        private void OnLocalChange(object? sender, EventArgs args)
        {
            OnLocalChange();
        }

        private void OnLocalChange()
        {
            GlobalTransform.Matrix = LocalTransform.Matrix * GetParentTransform();

            Propagate(GlobalTransform.Matrix, PropagateAction);
        }

        private void OnGlobalChange(object? sender, EventArgs args)
        {
            OnGlobalChange();
        }

        private void OnGlobalChange()
        {
            if (GlobalTransform.Matrix == LocalTransform.Matrix * GetParentTransform()) return;

            LocalTransform.Matrix = GlobalTransform.Matrix * GetParentTransform().Inverted();

            TransformChanged?.Invoke(this, EventArgs.Empty);
        }

        private Matrix4 GetParentTransform()
        {
            return GetAsset(
                parent => parent.GlobalTransform.Matrix,
                Matrix4.Identity);
        }

        private static void PropagateAction(ITransformElement<Transform3D> child, Matrix4 globalMatrix)
        {
            child.GlobalTransform.Matrix = child.LocalTransform.Matrix * globalMatrix;
        }

        private static void DePropagateAction(ITransformElement<Transform3D> child)
        {
            child.GlobalTransform.Matrix = child.LocalTransform.Matrix;
        }
    }
}
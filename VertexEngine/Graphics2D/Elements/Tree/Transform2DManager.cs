using OpenTK.Mathematics;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics2D.Assets;

namespace VertexEngine.Graphics2D.Elements.Tree
{
    public class Transform2DManager : TreeManager<ITransformElement<Transform2D>>
    {
        private Transform2D localTransform = new();
        private Transform2D globalTransform = new();

        public event EventHandler TransformChanged;

        public Transform2DManager(ITransformElement<Transform2D> element) : base(element)
        {
            localTransform.TransformChanged += OnLocalChange;
            globalTransform.TransformChanged += OnGlobalChange;
        }

        public Transform2D LocalTransform
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

        public Transform2D GlobalTransform
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
            Propagate(child, (GlobalTransform.Matrix, LocalTransform.Size), PropagateAction);
        }

        public override void DePropagate(IElement child)
        {
            Propagate(child, DePropagateAction);
        }

        private void OnLocalChange(object sender, EventArgs args)
        {
            OnLocalChange();
        }

        private void OnLocalChange()
        {
            GlobalTransform.Matrix = LocalTransform.Matrix * GetParentMatrix();
            LocalTransform.SetContainerSize(GetParentSize());

            Propagate((GlobalTransform.Matrix, LocalTransform.Size), PropagateAction);
        }

        private void OnGlobalChange(object sender, EventArgs args)
        {
            OnGlobalChange();
        }

        private void OnGlobalChange()
        {
            if (!IsGlobalChanged())
            {
                Propagate((GlobalTransform.Matrix, LocalTransform.Size), PropagateAction);
                return;
            }

            LocalTransform.Matrix = GlobalTransform.Matrix * GetParentMatrix().Inverted();
            LocalTransform.SetContainerSize(GetParentSize());

            TransformChanged?.Invoke(this, EventArgs.Empty);
        }

        private static void PropagateAction(ITransformElement<Transform2D> child,
            (Matrix4 globalMatrix, Vector2i localSize) args)
        {
            child.LocalTransform.SetContainerSize(args.localSize);
            child.GlobalTransform.Matrix = child.LocalTransform.Matrix * args.globalMatrix;
        }

        private static void DePropagateAction(ITransformElement<Transform2D> child)
        {
            child.LocalTransform.SetContainerSize(null);
            child.GlobalTransform.Matrix = child.LocalTransform.Matrix;
        }

        private bool IsGlobalChanged()
        {
            return GlobalTransform.Matrix != LocalTransform.Matrix * GetParentMatrix();
        }

        private Matrix4 GetParentMatrix()
        {
            return GetAsset(parent => parent.GlobalTransform.Matrix,
                Matrix4.Identity);
        }

        private Vector2i? GetParentSize()
        {
            return GetAsset<Vector2i?>(parent => parent.LocalTransform.Size, null);
        }
    }
}
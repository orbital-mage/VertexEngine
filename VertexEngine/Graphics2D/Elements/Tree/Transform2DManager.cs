using OpenTK.Mathematics;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics2D.Assets;
using VertexEngine.Graphics2D.Elements.Interfaces;

namespace VertexEngine.Graphics2D.Elements.Tree
{
    public class Transform2DManager : TreeManager<ITransformElement2D>
    {
        private Transform2D localTransform = new();
        private Transform2D globalTransform = new();

        public event EventHandler? TransformChanged;

        public Transform2DManager(ITransformElement2D element) : base(element)
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

        public bool UseScreenTransform { get; set; }

        public override void Propagate(IElement child)
        {
            Propagate(child, GlobalTransform, PropagateAction);
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
            if (UseScreenTransform)
                GlobalTransform.Matrix = CalculateGlobalScreenTransform(LocalTransform, GetParentTransform());
            else
                GlobalTransform.Matrix = LocalTransform.Matrix * (GetParentTransform()?.Matrix ?? Matrix4.Identity);

            Propagate(GlobalTransform, PropagateAction);
        }

        private void OnGlobalChange(object? sender, EventArgs args)
        {
            OnGlobalChange();
        }

        private void OnGlobalChange()
        {
            if (!IsGlobalChanged())
            {
                Propagate(GlobalTransform, PropagateAction);
                return;
            }

            if (UseScreenTransform)
                LocalTransform.Matrix = CalculateLocalScreenTransform(GlobalTransform, GetParentTransform());
            else
                LocalTransform.Matrix =
                    GlobalTransform.Matrix * (GetParentTransform()?.Matrix.Inverted() ?? Matrix4.Identity);

            TransformChanged?.Invoke(this, EventArgs.Empty);
        }

        private bool IsGlobalChanged()
        {
            if (!UseScreenTransform)
                return GlobalTransform.Matrix !=
                       LocalTransform.Matrix * (GetParentTransform()?.Matrix ?? Matrix4.Identity);

            return GlobalTransform.Matrix != CalculateGlobalScreenTransform(LocalTransform, GetParentTransform());
        }

        private Transform2D? GetParentTransform()
        {
            return GetAsset(parent => parent.GlobalTransform, null);
        }

        private static void PropagateAction(ITransformElement2D child, Transform2D parentTransform)
        {
            child.GlobalTransform.Matrix = child.UseScreenTransform
                ? CalculateGlobalScreenTransform(child.LocalTransform, parentTransform)
                : child.LocalTransform.Matrix * parentTransform.Matrix;
        }

        private static void DePropagateAction(ITransformElement2D child)
        {
            child.GlobalTransform.Matrix = child.UseScreenTransform
                ? CalculateGlobalScreenTransform(child.LocalTransform, null)
                : child.LocalTransform.Matrix;
        }

        private static Matrix4 CalculateGlobalScreenTransform(Transform2D local, Transform2D? parent)
        {
            return new Transform2D
            {
                Position = local.Position + (parent?.Position ?? Vector2i.Zero),
                Rotation = local.Rotation + (parent?.Rotation ?? 0),
                Size = local.Size,
            }.Matrix;
        }

        private static Matrix4 CalculateLocalScreenTransform(Transform2D global, Transform2D? parent)
        {
            return new Transform2D
            {
                Position = global.Position - (parent?.Position ?? Vector2i.Zero),
                Rotation = global.Rotation - (parent?.Rotation ?? 0),
                Size = global.Size,
            }.Matrix;
        }
    }
}
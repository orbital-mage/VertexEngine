using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements.Tree;

namespace VertexEngine.Common.Elements
{
    public class Element : IElement
    {
        private readonly ChildrenManager childrenManager;
        private readonly RenderOptionsManager renderOptionsManager;

        private VertexObject? vertexObject;
        private Shader? shader;
        private int priority;

        public event EventHandler<TreeChangedArgs>? TreeChanged;
        public event EventHandler? AssetsChanged;

        public Element() : this(null, null)
        {
        }

        public Element(VertexObject? vertexObject, Shader? shader)
        {
            VertexObject = vertexObject;
            Shader = shader;

            childrenManager = new ChildrenManager(this, PropagateAssets, DePropagateAssets);
            renderOptionsManager = new RenderOptionsManager(this);

            renderOptionsManager.OptionsChanged += (_, _) => OnAssetChanged();
        }

        public IEnumerable<IElement> Children => childrenManager.Children;

        public virtual IElement? Parent
        {
            get => childrenManager.Parent;
            set => childrenManager.Parent = value;
        }

        public VertexObject? VertexObject
        {
            get => vertexObject;
            set
            {
                vertexObject = value;
                OnAssetChanged();
            }
        }

        public Shader? Shader
        {
            get => shader;
            set
            {
                shader = value;
                OnAssetChanged();
            }
        }

        public RenderOptions? RenderOptions
        {
            get => renderOptionsManager.RenderOptions;
            set => renderOptionsManager.RenderOptions = value;
        }

        public int Priority
        {
            get => priority;
            set
            {
                priority = value;
                OnAssetChanged();
            }
        }

        public void AddChild(IElement child)
        {
            childrenManager.AddChild(child);

            child.TreeChanged += OnTreeChanged;
            OnTreeChanged(TreeChangedArgs.Added(child));
        }

        public void AddChildren(IEnumerable<IElement> children)
        {
            var addedElements = children as IElement[] ?? children.ToArray();

            foreach (var child in addedElements)
            {
                childrenManager.AddChild(child);
                child.TreeChanged += OnTreeChanged;
            }

            OnTreeChanged(TreeChangedArgs.Added(addedElements));
        }

        public void AddChildren(params IElement[] children)
        {
            AddChildren(children as IEnumerable<IElement>);
        }

        public void RemoveChild(IElement child)
        {
            childrenManager.RemoveChild(child);

            child.TreeChanged -= OnTreeChanged;
            OnTreeChanged(TreeChangedArgs.Removed(child));
        }

        void IElement.AddToParent(IElement newParent)
        {
            childrenManager.AddToParent(newParent);
        }

        void IElement.RemoveFromParent()
        {
            childrenManager.RemoveFromParent();
        }

        public virtual void Draw()
        {
            VertexObject?.Draw();
        }

        protected virtual void PropagateAssets(IElement child)
        {
            renderOptionsManager.Propagate(child);
        }

        protected virtual void DePropagateAssets(IElement child)
        {
            renderOptionsManager.DePropagate(child);
        }

        protected void OnAssetChanged()
        {
            AssetsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnTreeChanged(object? sender, TreeChangedArgs args)
        {
            OnTreeChanged(args);
        }

        private void OnTreeChanged(TreeChangedArgs args)
        {
            TreeChanged?.Invoke(this, args);
        }
    }
}
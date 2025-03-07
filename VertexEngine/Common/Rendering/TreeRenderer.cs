using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Graphics3D.Elements.Interfaces;

namespace VertexEngine.Common.Rendering
{
    public class TreeRenderer(IElement root)
    {
        public static void SetViewportSize(Vector2i size)
        {
            GL.Viewport(0, 0, size.X, size.Y);
        }

        private static readonly Func<IElement, IAsset?>[] AssetGetters =
        {
            element => element.RenderOptions?.Asset ?? GameWindow.CurrentRenderOptions.Asset,
            element => element.Shader,
            Get<ICameraElement>(element => element.Camera),
            element => element.VertexObject,
            Get<ITextureElement>(element => element.Textures.Asset),
            Get<ILightElement>(element => element.Lights.Asset),
            Get<IMaterialElement>(element => element.Material.Asset),
            Get<ITransformElement>(element => element.Transform)
        };

        private readonly HashSet<IElement> changedElements = [];
        private HashSet<IElement> elementSet = [];
        private IElement root = root;
        private bool wasTreeChanged;

        protected virtual IRenderTreeBranch ElementTree { get; } = new PriorityDictionary(AssetGetters);

        public IElement Root
        {
            get => root;
            set
            {
                root = value;
                root.TreeChanged += (_, _) => wasTreeChanged = true;
                wasTreeChanged = true;
            }
        }

        public Vector4? BackgroundColor { get; set; } = Vector4.One;

        public virtual void Draw()
        {
            if (BackgroundColor.HasValue)
            {
                GL.ClearColor(BackgroundColor.Value.X,
                    BackgroundColor.Value.Y,
                    BackgroundColor.Value.Z,
                    BackgroundColor.Value.W);
                GL.Clear(ClearBufferMask.ColorBufferBit |
                         ClearBufferMask.DepthBufferBit |
                         ClearBufferMask.StencilBufferBit);
            }

            ElementTree.Draw();
        }

        public void UpdateTree()
        {
            HandleTreeUpdate();
            HandleAssetUpdate();
            HandleGarbageCollection();
        }

        private void HandleTreeUpdate()
        {
            if (!wasTreeChanged) return;

            var newElementSet = IElement.FlattenTree(root).ToHashSet();

            var removedElements = elementSet.Except(newElementSet);
            var addedElements = newElementSet.Except(elementSet);

            foreach (var element in removedElements)
                RemoveElement(element);

            foreach (var element in addedElements)
                AddElement(element);

            elementSet = newElementSet;

            wasTreeChanged = false;
        }

        private void HandleAssetUpdate()
        {
            foreach (var element in changedElements)
            {
                ElementTree.RemoveElement(element);
                ElementTree.AddElement(element);
            }

            changedElements.Clear();
        }

        private void HandleGarbageCollection()
        {
            ElementTree.CollectGarbage();
        }

        private void AddElement(IElement element)
        {
            ElementTree.AddElement(element);
            element.AssetsChanged += OnElementChanged;
        }

        private void RemoveElement(IElement element)
        {
            ElementTree.RemoveElement(element);
            element.AssetsChanged -= OnElementChanged;
        }

        private void OnElementChanged(object? sender, EventArgs _)
        {
            if (sender is IElement element) changedElements.Add(element);
        }

        protected static Func<IElement, IAsset?> Get<T>(Func<T, IAsset?> getter) where T : IElement
        {
            return element => element is T t ? getter.Invoke(t) : IAsset.Empty;
        }
    }
}
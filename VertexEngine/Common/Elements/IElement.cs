using System.Runtime.CompilerServices;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Rendering;

[assembly:InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace VertexEngine.Common.Elements
{
    public interface IElement
    {
        public event EventHandler<TreeChangedArgs> TreeChanged;
        public event EventHandler AssetsChanged;

        public IEnumerable<IElement> Children { get; }
        public IElement? Parent { get; }

        public Shader? Shader { get; set; }
        public VertexObject? VertexObject { get; set; }
        public RenderOptions? RenderOptions { get; set; }
        
        /// <summary>
        /// The rendering priority for the element. Lower priorities are rendered first.
        /// Elements with the same priority are not sorted.
        /// </summary>
        public int Priority { get; }

        public void AddChild(IElement child);
        public void AddChildren(IEnumerable<IElement> children);
        public void AddChildren(params IElement[] children);
        public void RemoveChild(IElement child);
        internal void AddToParent(IElement parent);
        internal void RemoveFromParent();

        public void Draw();

        public static IEnumerable<IElement> FlattenTree(IElement element)
        {
            return element.Children
                .SelectMany(FlattenTree)
                .Append(element);
        }
    }
}
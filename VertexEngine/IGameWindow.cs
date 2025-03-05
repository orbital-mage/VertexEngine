using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements;

namespace VertexEngine
{
    public interface IGameWindow
    {
        public IElement Root { get; set; }
        public RenderOptions RenderOptions { get; }
        
        public Vector4? BackgroundColor { set; get; }

        public Vector2i Size { get; set; }
        public KeyboardState KeyboardState { get; }
        public MouseState MouseState { get; }

        public void Close();
    }
}
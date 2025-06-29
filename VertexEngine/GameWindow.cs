using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Rendering;
using VertexEngine.Common.Resources;

namespace VertexEngine
{
    public abstract class GameWindow : OpenTK.Windowing.Desktop.GameWindow, IGameWindow
    {
        public static GameWindow CurrentWindow { get; private set; } = null!;
        public static Vector2i CurrentWindowSize => CurrentWindow.Size;
        public static RenderOptions CurrentRenderOptions => CurrentWindow.RenderOptions;
        public static KeyboardState CurrentKeyboardState => CurrentWindow.KeyboardState;
        public static MouseState CurrentMouseState => CurrentWindow.MouseState;

        private IElement root;
        private readonly SimpleRenderer renderer;

        public Vector4? BackgroundColor
        {
            get => renderer.BackgroundColor;
            set => renderer.BackgroundColor = value;
        }

        public RenderOptions RenderOptions { get; } = new();

        public IElement Root
        {
            get => root;
            set
            {
                root = value;
                renderer.Root = root;
            }
        }

        protected GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            root = new Element();
            renderer = new SimpleRenderer(root, Size);

            Caches.ProjectName = Title;
            BackgroundColor = new Vector4(0.2f, 0.3f, 0.3f, 1f);
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            UseScreenRenderer();
            
            CurrentWindow = this;
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            CurrentWindow = this;

            OnDraw(args);

            SwapBuffers();
        }

        protected virtual void OnDraw(FrameEventArgs args)
        {
            renderer.Draw();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
            CurrentWindow = this;
            renderer.UpdateTree();
        }

        protected override void OnResize(ResizeEventArgs args)
        {
            base.OnResize(args);

            renderer.ViewportSize = Size;
        }

        protected void UseScreenRenderer()
        {
            renderer.Use();
        }
    }
}
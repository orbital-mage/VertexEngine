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
        public static IGameWindow CurrentWindow { get; private set; }
        public static Vector2i CurrentWindowSize => CurrentWindow.ClientSize;
        public static RenderOptions CurrentRenderOptions => CurrentWindow.RenderOptions;
        public static KeyboardState CurrentKeyboardState => CurrentWindow.KeyboardState;
        public static MouseState CurrentMouseState => CurrentWindow.MouseState;

        private readonly TreeRenderer renderer = new();
        private IElement root;

        public Vector4? BackgroundColor
        {
            get => renderer.BackgroundColor;
            set => renderer.BackgroundColor = value;
        }

        public RenderOptions RenderOptions => Root.RenderOptions;

        public IElement Root
        {
            get => root;
            set
            {
                root = value;
                root.RenderOptions ??= new RenderOptions();
                renderer.Root = root;
            }
        }

        protected GameWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            Caches.ProjectName = Title;
            Root = new Element();
            renderer.Root = Root;
            BackgroundColor = new Vector4(0.2f, 0.3f, 0.3f, 1f);
        }
        
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Viewport(0, 0, Size.X, Size.Y);

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

        protected override void OnResize(ResizeEventArgs e)
        {
            TreeRenderer.SetViewportSize(ClientSize);
        }
    }
}
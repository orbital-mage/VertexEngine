using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Rendering;
using VertexEngine.Common.Resources;
using VertexEngine.Common.Utils;

namespace VertexEngine
{
    public abstract class GameWindow : OpenTK.Windowing.Desktop.GameWindow, IGameWindow
    {
        public static IGameWindow CurrentWindow { get; private set; } = DummyWindow.Instance;
        public static Vector2i CurrentWindowSize => CurrentWindow.Size;
        public static RenderOptions CurrentRenderOptions => CurrentWindow.RenderOptions;
        public static KeyboardState CurrentKeyboardState => CurrentWindow.KeyboardState;
        public static MouseState CurrentMouseState => CurrentWindow.MouseState;

        private IElement root;
        private readonly TreeRenderer renderer;

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
            renderer = new TreeRenderer(root);

            Caches.ProjectName = Title;
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
            TreeRenderer.SetViewportSize(Size);
        }
    }

    internal class DummyWindow : IGameWindow
    {
        public static readonly DummyWindow Instance = new();

        private DummyWindow()
        {
        }

        public IElement Root
        {
            get => throw new UninitializedException();
            set => throw new UninitializedException();
        }

        public RenderOptions RenderOptions => throw new UninitializedException();

        public Vector4? BackgroundColor
        {
            get => throw new UninitializedException();
            set => throw new UninitializedException();
        }

        public Vector2i Size
        {
            get => throw new UninitializedException();
            set => throw new UninitializedException();
        }

        public KeyboardState KeyboardState => throw new UninitializedException();
        public MouseState MouseState => throw new UninitializedException();

        public void Close()
        {
            throw new UninitializedException();
        }
    }
}
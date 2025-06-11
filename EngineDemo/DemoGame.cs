using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VertexEngine.Common.Utils;
using VertexEngine.Graphics3D.Assets.Cameras;
using VertexEngine.Graphics3D.Elements;
using GameWindow = VertexEngine.GameWindow;

namespace EngineDemo;

public class DemoGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    : GameWindow(gameWindowSettings, nativeWindowSettings)
{
    protected override void OnLoad()
    {
        base.OnLoad();

        var element = new Element3D(Shapes.Square);

        var camera = new PerspectiveCamera(Vector3.UnitZ, (float)Size.X / Size.Y);
        element.Camera = camera;

        Root.AddChild(element);
    }
}
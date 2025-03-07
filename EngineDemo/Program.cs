using EngineDemo;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;

var settings = new NativeWindowSettings
{
    Size = new Vector2i(1600, 900),
    Title = "Vertex Engine Demo",
};

using var game = new DemoGame(GameWindowSettings.Default, settings);

game.Run();
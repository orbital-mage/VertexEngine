# Vertex Engine

The Vertex Engine is a graphical engine for games and other graphical programs.
It is written in C#, using the OpenTK toolkit, a C# implementation of OpenGL.

The engine provides high-level functionality for 3D and 2D graphics, to allow for quick prototyping with little overhead, as well as allowing for extending the lower-level functionality for more complex use-cases.

## Compatibility
The engine has been tested to work in Windows and Linux environments.

## Getting Started
* Install the [NuGet package](https://www.nuget.org/packages/VertexEngine/) in your project.
* Create a new class a new class to extend `VertexEngine.GameWindow`:
  
  ```csharp
  public class MyGame : GameWindow
  {
    public Tetris(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings) : base(gameWindowSettings, nativeWindowSettings)
    {
    }
  }
  ```
* Override the `OnLoad()` method, and create a new `Element` when the window first opens:

  ```csharp
  protected override void OnLoad()
  {
    base.OnLoad();

    var element = new Element3D(Shapes.Cube);
  }
  ```
* A default 3d element need a `Camera` in order to render to the screen:
  ```csharp
  var camera = new PerspectiveCamera(Vector3.UnitZ, aspectRatio);
  element.Camera = camera;
  ```
* Now add the element to the window's Root element:
  ```csharp
  Root.AddChild(element);
  ```
* Now initialize the game window in the project's `Main()` method, and run it:
  ```csharp
  var settings = new NativeWindowSettings
  {
      Size = new Vector2i(1600, 900),
      Title = "Vertex Engine Demo",
  };
  
  using var game = new MyGame(GameWindowSettings.Default, settings);
  
  game.Run();
  ```

When you run your project, you should now see a new window with a white cube (or a square, from this perspective) in the center.

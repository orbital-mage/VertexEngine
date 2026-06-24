using FontStashSharp;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Utils;
using VertexEngine.Graphics2D.Elements;
using VertexEngine.Graphics3D.Assets.Cameras;
using VertexEngine.Graphics3D.Elements;
using GameWindow = VertexEngine.GameWindow;

namespace EngineDemo;

public class DemoGame(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
    : GameWindow(gameWindowSettings, nativeWindowSettings)
{
    private Element3D? rotatingCube;
    private float rotation;

    public static NativeWindowSettings CreateWindowSettings() => new()
    {
        Size = new Vector2i(1000, 1000),
        Title = "Vertex Engine Demo — MSAA 4x",
        NumberOfSamples = 4,
    };

    protected override void OnLoad()
    {
        base.OnLoad();

        BackgroundColor = new Vector4(0.12f, 0.14f, 0.18f, 1f);

        var aspectRatio = (float)Size.X / Size.Y;
        var camera = new PerspectiveCamera(new Vector3(0f, 0.5f, 5f), aspectRatio)
        {
            Yaw = -90f,
            Pitch = -8f,
        };

        rotatingCube = new Element3D(Shapes.Cube)
        {
            Camera = camera,
        };
        rotatingCube.Material[MaterialUniforms.Diffuse3] = new Vector3(0.82f, 0.88f, 0.96f);
        rotatingCube.LocalTransform.Rotation =
            Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(25f)) *
            Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(15f));

        var sphere = new Element3D(Shapes.Sphere)
        {
            Camera = camera,
        };
        sphere.Material[MaterialUniforms.Diffuse3] = new Vector3(0.95f, 0.55f, 0.45f);
        sphere.LocalTransform.Position = new Vector3(-2.2f, -0.4f, 0f);
        sphere.LocalTransform.Scale = new Vector3(0.75f);

        var fontSystem = CreateFontSystem();

        var title = CreateText(fontSystem, "MSAA 4x enabled — compare edge smoothness", 28, new Vector2i(500, 48));
        var subtitle = CreateText(fontSystem, "Rotated 2D shapes and 3D geometry highlight anti-aliasing", 18,
            new Vector2i(500, 88), new Vector3(0.75f, 0.8f, 0.85f));

        Root.AddChildren(
            rotatingCube,
            sphere,
            CreateShape(new Vector2i(180, 820), new Vector2i(220, 36), 12f, new Vector3(0.35f, 0.75f, 0.95f)),
            CreateShape(new Vector2i(820, 820), new Vector2i(220, 36), -18f, new Vector3(0.95f, 0.45f, 0.55f)),
            CreateShape(new Vector2i(500, 900), new Vector2i(360, 28), 4f, new Vector3(0.55f, 0.95f, 0.65f)),
            CreateShape(new Vector2i(120, 500), new Vector2i(140, 140), 45f, new Vector3(0.98f, 0.82f, 0.35f)),
            CreateShape(new Vector2i(880, 500), new Vector2i(140, 140), -33f, new Vector3(0.72f, 0.55f, 0.98f)),
            title,
            subtitle);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);

        if (rotatingCube is null) return;

        rotation += (float)args.Time * 25f;
        rotatingCube.LocalTransform.Rotation =
            Quaternion.FromAxisAngle(Vector3.UnitY, MathHelper.DegreesToRadians(rotation)) *
            Quaternion.FromAxisAngle(Vector3.UnitX, MathHelper.DegreesToRadians(15f));
    }

    private static Element2D CreateShape(Vector2i position, Vector2i size, float rotationDegrees, Vector3 color)
    {
        var shape = new Element2D(Shapes.Square)
        {
            UseScreenTransform = true,
            Priority = 1,
            Material =
            {
                [MaterialUniforms.Color] = color
            },
            LocalTransform =
            {
                Position = position,
                Size = size,
                Rotation = rotationDegrees
            }
        };
        
        return shape;
    }

    private static TextElement CreateText(
        FontSystem fontSystem,
        string text,
        int fontSize,
        Vector2i centerPosition,
        Vector3? color = null)
    {
        var element = new TextElement
        {
            FontSystem = fontSystem,
            FontSize = fontSize,
            Text = text,
            Priority = 2,
            Color = color ?? Vector3.One,
            LocalTransform =
            {
                Position = centerPosition
            }
        };
        
        return element;
    }

    private static FontSystem CreateFontSystem()
    {
        var fontSystem = new FontSystem();
        foreach (var path in new[]
                 {
                     "/usr/share/fonts/truetype/dejavu/DejaVuSans.ttf",
                     "/usr/share/fonts/TTF/DejaVuSans.ttf",
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf"),
                     Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "Arial.ttf"),
                 })
        {
            if (!File.Exists(path)) continue;

            fontSystem.AddFont(File.ReadAllBytes(path));
            return fontSystem;
        }

        throw new FileNotFoundException(
            "No system font found for demo text. Install DejaVu Sans or Arial, or add a .ttf to the demo project.");
    }
}

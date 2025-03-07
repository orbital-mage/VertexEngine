using FontStashSharp;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Common.Text;
using VertexEngine.Common.Utils;
using VertexEngine.Graphics2D.Assets;
using VertexEngine.Graphics2D.Elements.Tree;

namespace VertexEngine.Graphics2D.Elements;

public class TextElement : Element, ITextElement, ITransformElement<Transform2D>, ICameraElement<TextCamera>
{
    private static readonly Shader TextShader = Shader.FromFiles("~/Text/shader.frag", "~/Text/shader.vert");

    private readonly Transform2DManager transformManager;
    private readonly MaterialManager materialManager;
    private readonly CameraManager<TextCamera> cameraManager;

    private readonly FontRenderer fontRenderer;

    private string text = "";
    private FontSystem? fontSystem;
    private int fontSize = 12;

    public TextElement() : base(null, TextShader)
    {
        transformManager = new Transform2DManager(this);
        materialManager = new MaterialManager(this);
        cameraManager = new CameraManager<TextCamera>(this);

        Camera = new TextCamera();

        transformManager.TransformChanged += (_, _) => OnAssetChanged();
        materialManager.MaterialChanged += (_, _) => OnAssetChanged();
        cameraManager.CameraChanged += (_, _) => OnAssetChanged();

        fontRenderer = new FontRenderer();

        RenderOptions = new RenderOptions
        {
            BlendingOptions = BlendingOptions.Basic
        };
        
        Material[MaterialUniforms.Color] = Vector3.One;
    }

    public string Text
    {
        get => text;
        set
        {
            text = value;
            RenderText();
        }
    }

    public FontSystem? FontSystem
    {
        get => fontSystem;
        set
        {
            fontSystem = value;
            RenderText();
        }
    }

    public int FontSize
    {
        get => fontSize;
        set
        {
            fontSize = value;
            RenderText();
        }
    }

    public Vector3 Color
    {
        get => Material[MaterialUniforms.Color] is Vector3 color ? color : Vector3.One;
        set => Material[MaterialUniforms.Color] = value;
    }

    public Transform2D LocalTransform
    {
        get => transformManager.LocalTransform;
        set => transformManager.LocalTransform = value;
    }

    public Transform2D GlobalTransform
    {
        get => transformManager.GlobalTransform;
        set => transformManager.GlobalTransform = value;
    }

    public AssetSet<Texture> Textures => materialManager.Textures;

    public IMaterial Material
    {
        get => materialManager.Material;
        set => materialManager.Material = value;
    }

    public TextCamera? Camera
    {
        get => cameraManager.Camera;
        set => cameraManager.Camera = value;
    }

    private void RenderText()
    {
        if (FontSystem is null) return;

        var font = FontSystem.GetFont(FontSize);
        var position = GameWindow.CurrentWindowSize / 2;
        var origin = font.MeasureString(text);

        LocalTransform.SetElementSize(origin.ToOpenTkVector2().ToVector2I());
        LocalTransform.Size = origin.ToOpenTkVector2().ToVector2I();

        font.DrawText(fontRenderer, Text, position.ToSystemVector2(), FSColor.White, origin: origin);

        var (vertexObject, atlas) = fontRenderer.CreateAssets();

        VertexObject = vertexObject;
        Material[MaterialUniforms.FontAtlas] = atlas;
    }
}
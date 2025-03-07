using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Common.Resources;
using VertexEngine.Graphics3D.Assets.Lights;
using VertexEngine.Graphics3D.Elements.Interfaces;

namespace VertexEngine.Graphics3D.Elements.Tree
{
    public class Shader3DManager : TreeManager<IShader3DElement>
    {
        public static readonly Shader DefaultShader = Shader.FromFiles(DefaultFragmentShader, DefaultVertexShader);

        private const string DefaultVertexShader = "~/_3D/shader.vert";
        private const string DefaultFragmentShader = "~/_3D/shader.frag";

        private static readonly (string, string)[] MaterialDefinitions =
        {
            (MaterialUniforms.Diffuse3, ShaderDefinitions.Diffuse3),
            (MaterialUniforms.Diffuse4, ShaderDefinitions.Diffuse4),
            (MaterialUniforms.DiffuseTexture, ShaderDefinitions.DiffuseTexture),
            (MaterialUniforms.Specular3, ShaderDefinitions.Specular3),
            (MaterialUniforms.Specular4, ShaderDefinitions.Specular4),
            (MaterialUniforms.SpecularTexture, ShaderDefinitions.SpecularTexture),
            (MaterialUniforms.Reflection, ShaderDefinitions.Reflection),
            (MaterialUniforms.Reflection3, ShaderDefinitions.Reflection3),
            (MaterialUniforms.Reflection4, ShaderDefinitions.Reflection4),
            (MaterialUniforms.ReflectionTexture, ShaderDefinitions.ReflectionTexture),
            (MaterialUniforms.Refraction, ShaderDefinitions.Refraction),
            (MaterialUniforms.Refraction3, ShaderDefinitions.Refraction3),
            (MaterialUniforms.Refraction4, ShaderDefinitions.Refraction4),
            (MaterialUniforms.RefractionTexture, ShaderDefinitions.RefractionTexture),
        };

        private readonly MaterialManager materialManager;
        private readonly LightsManager lightsManager;

        private ShaderBuilder builder = Shader.Builder();

        private string vertexShader = DefaultVertexShader;
        private string fragmentShader = DefaultFragmentShader;

        public event EventHandler? ShaderChanged;

        public Shader3DManager(IShader3DElement element) : base(element)
        {
            materialManager = new MaterialManager(element);
            lightsManager = new LightsManager(element);

            materialManager.MaterialChanged += OnMaterialChanged;
            lightsManager.LightsChanged += OnLightsChanges;
        }

        public IMaterial Material
        {
            get => materialManager.Material;
            set => materialManager.Material = value;
        }

        public AssetSet<Texture> Textures { get; } = new();

        public AssetSet<Light> Lights
        {
            get => lightsManager.Lights;
            set => lightsManager.Lights = value;
        }

        public string VertexShader
        {
            get => vertexShader;
            set
            {
                vertexShader = value;
                UpdateShader();
            }
        }

        public string FragmentShader
        {
            get => fragmentShader;
            set
            {
                fragmentShader = value;
                UpdateShader();
            }
        }

        public bool IsShaderValid { get; private set; }

        public override void Propagate(IElement child)
        {
            materialManager.Propagate(child);
            lightsManager.Propagate(child);
        }

        public override void DePropagate(IElement child)
        {
            materialManager.DePropagate(child);
            lightsManager.DePropagate(child);
        }

        private void OnMaterialChanged(object? sender, MaterialChangeArgs args)
        {
            if (args.AreTexturesUpdated) UpdateTextures();
            if (args.IsFootPrintChanged) UpdateShader();
        }

        private void OnLightsChanges(object? sender, LightsChangedArgs args)
        {
            if (args.TexturesChanged) UpdateTextures();
            UpdateShader();
        }

        private void UpdateTextures()
        {
            Textures.Clear();
            Textures.AddRange(materialManager.Textures);
            Textures.AddRange(lightsManager.Textures);
        }

        private void UpdateShader()
        {
            builder = Shader.Builder();

            DefineSources();
            DefineLights();
            DefineMaterial();

            try
            {
                Element.Shader = builder.Build();
                IsShaderValid = true;
                ShaderChanged?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception)
            {
                IsShaderValid = false;
            }
        }

        private void DefineSources()
        {
            if (VertexShader != null)
                builder.SetVertexShader(VertexShader);
            if (FragmentShader != null)
                builder.SetFragmentShader(FragmentShader);
        }

        private void DefineLights()
        {
            builder.AddDefinition(ShaderDefinitions.PositionalLights, Lights.Count(PositionalLight.SetName))
                .AddDefinition(ShaderDefinitions.DirectionalLights, Lights.Count(DirectionalLight.SetName))
                .AddDefinition(ShaderDefinitions.PointLights, Lights.Count(PointLight.SetName))
                .AddDefinition(ShaderDefinitions.SpotLights, Lights.Count(SpotLight.SetName));
        }

        private void DefineMaterial()
        {
            foreach (var (uniform, definition) in MaterialDefinitions)
                if (Material.Values.ContainsKey(uniform))
                    builder.AddDefinition(definition);
        }
    }
}
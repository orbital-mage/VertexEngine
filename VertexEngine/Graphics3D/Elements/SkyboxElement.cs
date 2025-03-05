using OpenTK.Graphics.OpenGL;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Common.Utils;
using VertexEngine.Graphics3D.Assets;
using VertexEngine.Graphics3D.Assets.Cameras;
using ShaderType = VertexEngine.Common.Assets.Shader;

namespace VertexEngine.Graphics3D.Elements
{
    public class SkyboxElement : Element,
        ICameraElement<PerspectiveCamera>,
        IMaterialElement
    {
        private const string CubemapUniform = "cubemap";

        private static readonly ShaderType CubemapShader =
            ShaderType.FromFiles("~/_3D/Cubemap/cubemap.frag", "~/_3D/Cubemap/cubemap.vert");

        private static readonly RenderOptions SkyboxRenderOptions = new()
        {
            DepthTestingOptions = new DepthTestingOptions
            {
                Enabled = true,
                MaskEnabled = true,
                DepthFunction = DepthFunction.Lequal
            }
        };

        private readonly CameraManager<PerspectiveCamera> cameraManager;
        private readonly MaterialManager materialManager;

        public SkyboxElement(Cubemap cubemap) : base(Shapes.Cube, CubemapShader)
        {
            cameraManager = new CameraManager<PerspectiveCamera>(this);
            materialManager = new MaterialManager(this);

            cameraManager.CameraChanged += (_, _) => OnAssetChanged();
            materialManager.MaterialChanged += (_, _) => OnAssetChanged();

            Material[CubemapUniform] = cubemap;
            RenderOptions = SkyboxRenderOptions;
        }

        public PerspectiveCamera Camera
        {
            get => cameraManager.Camera;
            set => cameraManager.Camera = value;
        }

        public AssetSet<Texture> Textures => materialManager.Textures;
        public IMaterial Material => materialManager.Material;
    }
}
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics3D.Assets;
using VertexEngine.Graphics3D.Assets.Cameras;
using VertexEngine.Graphics3D.Assets.Lights;
using VertexEngine.Graphics3D.Elements.Interfaces;
using VertexEngine.Graphics3D.Elements.Tree;

namespace VertexEngine.Graphics3D.Elements
{
    public class Element3D : Element,
        ITransformElement<Transform3D>,
        ICameraElement<Camera3D>,
        IShader3DElement
    {
        private readonly CameraManager<Camera3D> cameraManager;
        private readonly Transform3DManager transformManager;
        private readonly Shader3DManager shaderManager;

        public Element3D() : this(VertexObject.Empty)
        {
        }

        public Element3D(VertexObject vertexObject) : base(vertexObject, Shader3DManager.DefaultShader)
        {
            cameraManager = new CameraManager<Camera3D>(this);
            transformManager = new Transform3DManager(this);
            shaderManager = new Shader3DManager(this);

            cameraManager.CameraChanged += (_, _) => OnAssetChanged();
            transformManager.TransformChanged += (_, _) => OnAssetChanged();
            shaderManager.ShaderChanged += (_, _) => OnAssetChanged();
        }

        public string VertexShader
        {
            get => shaderManager.VertexShader;
            set => shaderManager.VertexShader = value;
        }

        public string FragmentShader
        {
            get => shaderManager.FragmentShader;
            set => shaderManager.FragmentShader = value;
        }

        public bool IsShaderValid => shaderManager.IsShaderValid;

        public Transform3D LocalTransform
        {
            get => transformManager.LocalTransform;
            set => transformManager.LocalTransform = value;
        }

        public Transform3D GlobalTransform
        {
            get => transformManager.GlobalTransform;
            set => transformManager.GlobalTransform = value;
        }

        public Camera3D? Camera
        {
            get => cameraManager.Camera;
            set => cameraManager.Camera = value;
        }

        public AssetSet<Texture> Textures => shaderManager.Textures;

        public IMaterial Material
        {
            get => shaderManager.Material;
            set => shaderManager.Material = value;
        }

        public AssetSet<Light> Lights
        {
            get => shaderManager.Lights;
            set => shaderManager.Lights = value;
        }

        protected override void PropagateAssets(IElement child)
        {
            base.PropagateAssets(child);

            cameraManager.Propagate(child);
            transformManager.Propagate(child);
            shaderManager.Propagate(child);
        }

        protected override void DePropagateAssets(IElement child)
        {
            base.DePropagateAssets(child);

            cameraManager.DePropagate(child);
            transformManager.DePropagate(child);
            shaderManager.DePropagate(child);
        }
    }
}
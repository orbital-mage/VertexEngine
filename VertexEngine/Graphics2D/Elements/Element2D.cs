using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics2D.Assets;
using VertexEngine.Graphics2D.Elements.Tree;

namespace VertexEngine.Graphics2D.Elements
{
    public class Element2D : Element,
        IMaterialElement,
        ITransformElement<Transform2D>,
        ICameraElement<Camera2D>
    {
        private static readonly Shader ColorShader = Shader.FromFiles("~/_2D/color.frag", "~/_2D/shader.vert");
        
        private readonly Transform2DManager transformManager;
        private readonly MaterialManager materialManager;
        private readonly CameraManager<Camera2D> cameraManager;

        public Element2D() : this(null, null)
        {
        }

        public Element2D(VertexObject? vertexObject) : this(vertexObject, ColorShader)
        {
        }

        public Element2D(VertexObject? vertexObject, Shader? shader) : base(vertexObject, shader)
        {
            transformManager = new Transform2DManager(this);
            materialManager = new MaterialManager(this);
            cameraManager = new CameraManager<Camera2D>(this);

            Camera = new Camera2D();

            RenderOptions = new RenderOptions
            {
                FaceCullingOptions = FaceCullingOptions.Off
            };

            transformManager.TransformChanged += (_, _) => OnAssetChanged();
            materialManager.MaterialChanged += (_, _) => OnAssetChanged();
            cameraManager.CameraChanged += (_, _) => OnAssetChanged();
        }

        public AssetSet<Texture> Textures => materialManager.Textures;

        public IMaterial Material
        {
            get => materialManager.Material;
            set => materialManager.Material = value;
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

        public Camera2D? Camera
        {
            get => cameraManager.Camera;
            set => cameraManager.Camera = value;
        }

        protected override void PropagateAssets(IElement child)
        {
            base.PropagateAssets(child);

            transformManager.Propagate(child);
            materialManager.Propagate(child);
            cameraManager.Propagate(child);
        }

        protected override void DePropagateAssets(IElement child)
        {
            base.DePropagateAssets(child);

            transformManager.DePropagate(child);
            materialManager.DePropagate(child);
            cameraManager.DePropagate(child);
        }
    }
}
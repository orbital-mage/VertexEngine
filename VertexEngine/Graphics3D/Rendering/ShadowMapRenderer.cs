using OpenTK.Graphics.OpenGL;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;
using VertexEngine.Common.Rendering;
using VertexEngine.Graphics3D.Assets.Lights;

namespace VertexEngine.Graphics3D.Rendering
{
    public class ShadowMapRenderer : TreeRenderer
    {
        private static readonly Func<IElement, IAsset?>[] AssetGetters =
        {
            element => element.VertexObject,
            Get<ITransformElement>(element => element.Transform)
        };

        private static readonly RenderOptions Options = new()
        {
            DepthTestingOptions = DepthTestingOptions.On,
            FaceCullingOptions = FaceCullingOptions.Off,
            BlendingOptions = BlendingOptions.Off
        };

        private readonly Dictionary<Type, IAsset> currentAssets = new();

        protected override IRenderTreeBranch ElementTree { get; }

        public ShadowMapRenderer(IElement root) : base(root)
        {
            BackgroundColor = null;
            ElementTree = new AssetDictionary(AssetGetters, currentAssets);
        }

        public HashSet<Light> Lights { get; } = [];

        public override void Draw()
        {
            Options.Asset.Use();

            var lightGroups = Lights.Where(light => light.ShadowMap != null).GroupBy(light => light.ShadowMap!.Shader);

            foreach (var lightGroup in lightGroups)
            {
                currentAssets[typeof(Shader)] = lightGroup.Key;
                lightGroup.Key.Use();

                foreach (var light in lightGroup)
                {
                    light.ShadowMap!.Bind();

                    GL.Clear(ClearBufferMask.DepthBufferBit);
                    base.Draw();
                }
            }
        }
    }
}
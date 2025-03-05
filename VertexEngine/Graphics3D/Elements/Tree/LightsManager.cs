using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Tree;
using VertexEngine.Graphics3D.Assets.Lights;
using VertexEngine.Graphics3D.Elements.Interfaces;

namespace VertexEngine.Graphics3D.Elements.Tree
{
    public class LightsManager : TreeManager<ILightElement>
    {
        private AssetSet<Light> lights = [];

        public event EventHandler<LightsChangedArgs> LightsChanged;

        public LightsManager(ILightElement element) : base(element)
        {
            lights.AssetsChanged += (_, args) => OnLightChange(args.BeforeChange);
        }

        public AssetSet<Light> Lights
        {
            get => lights;
            set
            {
                var before = lights.Assets;
                lights = value;
                lights.AssetsChanged += (_, args) => OnLightChange(args.BeforeChange);
                OnLightChange(before);
            }
        }

        public AssetSet<Texture> Textures { get; } = new();

        public override void Propagate(IElement child)
        {
            Propagate(child, filteredChild => filteredChild.Lights.AddRange(Lights));
        }

        public override void DePropagate(IElement child)
        {
            Propagate(child, filteredChild => filteredChild.Lights.RemoveRange(Lights));
        }

        private void OnLightChange(IEnumerable<Light> beforeChange)
        {
            Propagate(child =>
            {
                child.Lights.RemoveRange(beforeChange);
                child.Lights.AddRange(lights);
            });

            var texturesChanged = UpdateShadowMapTextures();

            LightsChanged?.Invoke(this, new LightsChangedArgs(texturesChanged));
        }

        private bool UpdateShadowMapTextures()
        {
            var textures = GetShadowMapTextures();

            if (Textures.Assets.SequenceEqual(textures)) return false;

            Textures.Clear();
            Textures.AddRange(textures);
            return true;
        }

        private Texture[] GetShadowMapTextures()
        {
            return lights
                .Select(light => light.ShadowMap?.Texture)
                .Where(texture => texture != null)
                .ToArray();
        }
    }
}
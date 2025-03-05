using EngineTK.Common.Rendering;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Elements;

namespace VertexEngine.Common.Rendering
{
    public class AssetDictionary(Func<IElement, IAsset>[] assetGetters, Dictionary<Type, IAsset> currentAssets)
        : RenderTreeDictionary<IAsset>
    {
        public AssetDictionary(params Func<IElement, IAsset>[] assetGetters) :
            this(assetGetters, new Dictionary<Type, IAsset>())
        {
        }

        public override void Draw()
        {
            foreach (var (asset, branch) in Dictionary)
            {
                asset.Use(currentAssets);
                currentAssets[asset.GetType()] = asset;

                branch.Draw();

                currentAssets.Remove(asset.GetType());
            }
        }

        protected override IAsset GetKey(IElement element)
        {
            return assetGetters[0].Invoke(element);
        }

        protected override void InitBranch(IAsset asset)
        {
            if (assetGetters.Length > 1)
                Dictionary[asset] = new AssetDictionary(assetGetters.Skip(1).ToArray(), currentAssets);
            else
                Dictionary[asset] = new ElementSet();
        }
    }
}
namespace VertexEngine.Common.Assets.Sets
{
    public class AssetSetChangedArgs<T>(IEnumerable<T> beforeChange) : EventArgs where T : IIndexedAsset
    {
        public IEnumerable<T> BeforeChange { get; } = beforeChange;
    }
}
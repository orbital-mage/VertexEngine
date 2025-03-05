namespace VertexEngine.Common.Assets.Sets
{
    public interface IIndexedAsset : IComparable
    {
        public object Grouping => 0;

        public void Use(Dictionary<Type, IAsset> assets, int unit);
    }
}
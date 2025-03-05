namespace VertexEngine.Common.Assets
{
    public interface IAsset
    {
        public static readonly IAsset Empty = new EmptyAsset();

        public void Use(Dictionary<Type, IAsset> currentAssets);

        public static T? GetAsset<T>(Dictionary<Type, IAsset> assets) where T : IAsset
        {
            return assets.ContainsKey(typeof(T)) ? (T)assets[typeof(T)] : default;
        }

        public class EmptyAsset : IAsset
        {
            public void Use(Dictionary<Type, IAsset> currentAssets)
            {
            }

            public override string ToString()
            {
                return GetType().Name;
            }
        }
    }
}
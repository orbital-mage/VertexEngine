using System.Collections;
using System.Collections.Immutable;

namespace VertexEngine.Common.Assets.Sets
{
    public class AssetSet<T> : IEnumerable<T> where T : IIndexedAsset
    {
        private readonly HashSet<T> assets = [];
        private readonly Dictionary<object, int> groupCount = new();

        public event EventHandler<AssetSetChangedArgs<T>> AssetsChanged;

        public IEnumerable<T> Assets => assets;
        public ImmutableAssetSet<T> Asset { get; private set; } = new(ImmutableHashSet<T>.Empty);

        public void Add(T? asset)
        {
            if (asset == null) return;

            assets.Add(asset);

            if (groupCount.TryGetValue(asset.Grouping, out var value))
                groupCount[asset.Grouping] = ++value;
            else
                groupCount[asset.Grouping] = 1;

            OnAssetsChanged();
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var asset in range)
                Add(asset);
        }

        public void Remove(T? asset)
        {
            if (asset == null || !assets.Contains(asset)) return;

            assets.Remove(asset);

            groupCount[asset.Grouping]--;

            OnAssetsChanged();
        }

        public void RemoveRange(IEnumerable<T> range)
        {
            foreach (var asset in range)
                Remove(asset);
        }

        public void Clear()
        {
            assets.Clear();
            OnAssetsChanged();
        }

        public int Count(object group)
        {
            return groupCount.GetValueOrDefault(group, 0);
        }

        private void OnAssetsChanged()
        {
            Asset = new ImmutableAssetSet<T>(Assets);
            var args = new AssetSetChangedArgs<T>(Assets);

            AssetsChanged?.Invoke(this, args);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return assets.GetEnumerator();
        }

        public override string ToString()
        {
            return $"AssetSet<{typeof(T).Name}>[{assets.Count}]";
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
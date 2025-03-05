using System.Collections.Immutable;

namespace VertexEngine.Common.Assets.Sets
{
    public sealed class ImmutableAssetSet<T> : IAsset where T : IIndexedAsset
    {
        private readonly IReadOnlyDictionary<object, ImmutableSortedSet<T>> resourceDictionary;

        public ImmutableHashSet<T> Assets { get; }

        public ImmutableAssetSet(IEnumerable<T> assets)
        {
            Assets = assets.ToImmutableHashSet();
            resourceDictionary = Assets
                .GroupBy(resource => resource.Grouping)
                .ToDictionary(
                    group => group.Key,
                    group => group.ToImmutableSortedSet());
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            foreach (var (_, resourceSubSet) in resourceDictionary)
            {
                for (var i = 0; i < resourceSubSet.Count; i++)
                    resourceSubSet[i].Use(currentAssets, i);
            }
        }

        public int GetUnit(T resource)
        {
            return resourceDictionary[resource.Grouping].IndexOf(resource);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            if (Assets.IsEmpty) return 0;

            var hash = new HashCode();

            foreach (var resource in Assets)
                hash.Add(resource);

            return hash.ToHashCode();
        }
    }
}
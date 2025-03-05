namespace VertexEngine.Common.Assets;

public class UniformAsset(Dictionary<string, object> uniforms) : IAsset
{
    private static readonly KeyValueComparer KeyValueComparer = new();

    private readonly Dictionary<string, object> uniforms = new(uniforms);

    public void Use(Dictionary<Type, IAsset> currentAssets)
    {
        UniformUtils.SetUniforms(currentAssets, uniforms);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((UniformAsset)obj);
    }

    public override int GetHashCode()
    {
        if (uniforms.Count == 0) return 0;

        var hash = new HashCode();

        foreach (var (name, value) in uniforms)
        {
            hash.Add(name);
            hash.Add(value);
        }

        return hash.ToHashCode();
    }

    private bool Equals(UniformAsset other)
    {
        return uniforms.SequenceEqual(other.uniforms, KeyValueComparer);
    }
}

internal class KeyValueComparer : IEqualityComparer<KeyValuePair<string, object>>
{
    public bool Equals(KeyValuePair<string, object> left, KeyValuePair<string, object> right)
    {
        return Equals(left.Key, right.Key) && Equals(left.Value, right.Value);
    }

    public int GetHashCode(KeyValuePair<string, object> pair)
    {
        return HashCode.Combine(pair.Key.GetHashCode(), pair.Value.GetHashCode());
    }
}
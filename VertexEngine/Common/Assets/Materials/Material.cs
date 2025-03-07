using System.Collections;
using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Common.Assets.Materials
{
    public class Material : IMaterial
    {
        public Dictionary<string, object> Values { get; } = new();
        public IEnumerable<Texture> TextureValues { get; private set; } = [];

        public UniformAsset Asset { get; private set; }

        public event EventHandler<MaterialChangeArgs>? ValuesChanged;

        public Material()
        {
            Asset = new UniformAsset([]);
        }

        public Material(IEnumerable<KeyValuePair<string, object>> values)
        {
            foreach (var (name, value) in values)
                Values[name] = value;

            TextureValues = Values.Values.OfType<Texture>();
            Asset = new UniformAsset(Values);
        }

        public void Use(Dictionary<Type, IAsset> currentResources)
        {
            Asset.Use(currentResources);
        }

        public void Add(string name, object value)
        {
            SetValue(name, value);
        }

        public object this[string name]
        {
            get => Values[name];
            set => SetValue(name, value);
        }

        private void SetValue(string name, object value)
        {
            if (Values.TryGetValue(name, out var curr) && Equals(curr, value)) return;

            var changeArgs = GetChangeArgs(name, value);
            Values[name] = value;
            OnValuesChanged(changeArgs);
        }

        private void OnValuesChanged(MaterialChangeArgs args)
        {
            Asset = new UniformAsset(Values);

            if (args.AreTexturesUpdated)
                TextureValues = Values.Values.OfType<Texture>();

            ValuesChanged?.Invoke(this, args);
        }

        private MaterialChangeArgs GetChangeArgs(string name, object value)
        {
            return MaterialChangeArgs.Get(IsFootPrintChanged(name, value), AreTexturesUpdated(value));
        }

        private bool IsFootPrintChanged(string name, object value)
        {
            return !Values.ContainsKey(name) || Values[name].GetType() != value.GetType();
        }

        private static bool AreTexturesUpdated(object value)
        {
            return value is Texture;
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"Material{{{string.Join(", ", Values.Keys)}}}";
        }
    }
}
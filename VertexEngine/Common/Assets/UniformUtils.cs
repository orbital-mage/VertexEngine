using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Common.Assets
{
    internal static class UniformUtils
    {
        private static readonly Dictionary<(string array, string name, int unit), string> IndexedUniformDictionary =
            new();

        public static void SetUniforms(Dictionary<Type, IAsset> assets, Dictionary<string, object> uniforms)
        {
            var shader = IAsset.GetAsset<Shader>(assets);

            if (shader == null) return;

            foreach (var (name, value) in uniforms)
            {
                if (shader.Uniforms.Contains(name))
                    shader.SetUniform(name, GetValue(value, assets));
            }
        }

        public static void SetIndexedUniforms(
            Dictionary<Type, IAsset> assets,
            string arrayName,
            int unit,
            Dictionary<string, object> uniforms)
        {
            var shader = IAsset.GetAsset<Shader>(assets);

            if (shader == null) return;

            foreach (var (name, value) in uniforms)
            {
                var fullName = GetFullName(arrayName, name, unit);
                if (shader.Uniforms.Contains(fullName))
                    shader.SetUniform(fullName, GetValue(value, assets));
            }
        }

        private static object GetValue(object value, Dictionary<Type, IAsset> currentResources)
        {
            return value switch
            {
                Texture texture => IAsset.GetAsset<ImmutableAssetSet<Texture>>(currentResources)?.GetUnit(texture) ?? 0,
                _ => value
            };
        }

        private static string GetFullName(string arrayName, string name, int unit)
        {
            var key = (arrayName, name, unit);

            if (!IndexedUniformDictionary.ContainsKey(key))
                IndexedUniformDictionary[key] = $"{arrayName}[{unit}].{name}";

            return IndexedUniformDictionary[key];
        }
    }
}
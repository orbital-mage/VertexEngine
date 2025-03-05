using VertexEngine.Common.Assets;

namespace VertexEngine.Common.Resources
{
    public static class ShaderDefinitions
    {
        public const string PositionalLights = "LIGHTS";
        public const string DirectionalLights = "DIR_LIGHTS";
        public const string PointLights = "POINT_LIGHTS";
        public const string SpotLights = "SPOTLIGHTS";

        public const string Reflection = "REFLECTION";
        public const string Refraction = "REFRACTION";
        public const string Diffuse3 = "DIFFUSE_3";
        public const string Specular3 = "SPECULAR_3";
        public const string Reflection3 = "REFLECTION_3";
        public const string Refraction3 = "REFRACTION_3";
        public const string Diffuse4 = "DIFFUSE_4";
        public const string Specular4 = "SPECULAR_4";
        public const string Reflection4 = "REFLECTION_4";
        public const string Refraction4 = "REFRACTION_4";
        public const string DiffuseTexture = "DIFFUSE_TEXTURE";
        public const string SpecularTexture = "SPECULAR_TEXTURE";
        public const string ReflectionTexture = "REFLECTION_TEXTURE";
        public const string RefractionTexture = "REFRACTION_TEXTURE";
    }

    public class ShaderBuilder
    {
        private const string DefaultValue = "0";

        private static readonly Dictionary<int, Shader> CompiledShaders = new();

        private readonly Dictionary<string, string> definitions = new();
        private string? fragmentShaderPath;
        private string? vertexShaderPath;
        private string? geometryShaderPath;

        public static ShaderBuilder Builder()
        {
            return new ShaderBuilder();
        }

        public ShaderBuilder SetFragmentShader(string path)
        {
            fragmentShaderPath = path;
            return this;
        }

        public ShaderBuilder SetVertexShader(string path)
        {
            vertexShaderPath = path;
            return this;
        }

        public ShaderBuilder SetGeometryShader(string path)
        {
            geometryShaderPath = path;
            return this;
        }

        public ShaderBuilder AddDefinition(string definition, string value = DefaultValue)
        {
            if (definitions.ContainsKey(definition) && definitions[definition].Equals(value)) return this;

            definitions[definition] = value;
            return this;
        }

        public ShaderBuilder AddDefinition(string definition, int value)
        {
            return AddDefinition(definition, value.ToString());
        }

        public Shader Build()
        {
            if (string.IsNullOrEmpty(vertexShaderPath) || string.IsNullOrEmpty(fragmentShaderPath))
                throw new Exception("No shader source specified");

            var key = GetShaderKey();
            if (CompiledShaders.TryGetValue(key, out var compiledShader))
                return compiledShader;

            var frag = ShaderAssembler.LoadShaderSource(fragmentShaderPath, definitions);
            var vert = ShaderAssembler.LoadShaderSource(vertexShaderPath, definitions);

            Shader shader;

            if (string.IsNullOrEmpty(geometryShaderPath))
                shader = new Shader(frag, vert);
            else
            {
                var geometry = ShaderAssembler.LoadShaderSource(geometryShaderPath, definitions);
                shader = new Shader(frag, vert, geometry);
            }

            CompiledShaders[key] = shader;
            return CompiledShaders[key];
        }

        private int GetShaderKey()
        {
            var hash = new HashCode();

            hash.Add(vertexShaderPath);
            hash.Add(fragmentShaderPath);
            hash.Add(geometryShaderPath);

            foreach (var (name, value) in definitions)
            {
                hash.Add(name);
                hash.Add(value);
            }

            return hash.ToHashCode();
        }
    }
}
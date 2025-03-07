using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Resources;

namespace VertexEngine.Common.Assets
{
    public class Shader : IAsset
    {
        private readonly int handle;
        private readonly Dictionary<string, int> uniforms;

        public Dictionary<VertexAttribute, int> Attributes { get; }
        public IEnumerable<string> Uniforms => uniforms.Keys;

        public static ShaderBuilder Builder()
        {
            return ShaderBuilder.Builder();
        }

        public static Shader FromFiles(string fragPath, string vertPath)
        {
            return Builder()
                .SetFragmentShader(fragPath)
                .SetVertexShader(vertPath)
                .Build();
        }

        // Needs empty constructor for tests
        #pragma warning disable CS8618, CS9264
        internal Shader()
        {
            
        }
        #pragma warning restore CS8618, CS9264

        internal Shader(string fragSource, string vertSource)
        {
            var fragmentShader = CreateShader(fragSource, ShaderType.FragmentShader);
            var vertexShader = CreateShader(vertSource, ShaderType.VertexShader);

            handle = CreateProgram(fragmentShader, vertexShader, null);

            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);

            Attributes = GetAttributes(handle);
            uniforms = GetUniforms(handle);
        }

        internal Shader(string fragSource, string vertSource, string geometrySource)
        {
            var fragmentShader = CreateShader(fragSource, ShaderType.FragmentShader);
            var vertexShader = CreateShader(vertSource, ShaderType.VertexShader);
            var geometryShader = CreateShader(geometrySource, ShaderType.GeometryShader);

            handle = CreateProgram(fragmentShader, vertexShader, geometryShader);

            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
            GL.DeleteShader(geometryShader);

            Attributes = GetAttributes(handle);
            uniforms = GetUniforms(handle);
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            Use();
        }

        public void Use()
        {
            GL.UseProgram(handle);
        }

        public void SetUniform(string name, object boxedData)
        {
            switch (boxedData)
            {
                case bool data:
                    GL.Uniform1(uniforms[name], data ? 1 : 0);
                    break;
                case int data:
                    GL.Uniform1(uniforms[name], data);
                    break;
                case float data:
                    GL.Uniform1(uniforms[name], data);
                    break;
                case double data:
                    GL.Uniform1(uniforms[name], data);
                    break;
                case Vector2 data:
                    GL.Uniform2(uniforms[name], data);
                    break;
                case Vector3 data:
                    GL.Uniform3(uniforms[name], data);
                    break;
                case Vector4 data:
                    GL.Uniform4(uniforms[name], data);
                    break;
                case Matrix2 data:
                    GL.UniformMatrix2(uniforms[name], true, ref data);
                    break;
                case Matrix4 data:
                    GL.UniformMatrix4(uniforms[name], true, ref data);
                    break;
                case int[] data:
                    GL.Uniform1(uniforms[name], data.Length, data);
                    break;
                case Matrix4[] data:
                    GL.UniformMatrix4(uniforms[name], data.Length, false, ref data[0].Row0.X);
                    break;
                default:
                    throw new ArgumentException($"Invalid uniform type {boxedData.GetType().Name}");
            }
        }

        private static int CreateShader(string source, ShaderType shaderType)
        {
            var shader = GL.CreateShader(shaderType);
            GL.ShaderSource(shader, source);
            CompileShader(shader);

            return shader;
        }

        private static int CreateProgram(int fragmentShader, int vertexShader, int? geometryShader)
        {
            var handle = GL.CreateProgram();

            GL.AttachShader(handle, fragmentShader);
            GL.AttachShader(handle, vertexShader);
            if (geometryShader.HasValue) GL.AttachShader(handle, geometryShader.Value);

            LinkProgram(handle);

            GL.DetachShader(handle, fragmentShader);
            GL.DetachShader(handle, vertexShader);
            if (geometryShader.HasValue) GL.DetachShader(handle, geometryShader.Value);

            return handle;
        }

        private static Dictionary<VertexAttribute, int> GetAttributes(int handle)
        {
            GL.GetProgram(handle, GetProgramParameterName.ActiveAttributes, out var numberOfAttributes);
            var attributes = new Dictionary<VertexAttribute, int>();

            for (var i = 0; i < numberOfAttributes; i++)
            {
                var attribute = GL.GetActiveAttrib(handle, i, out _, out var type);
                var attributeLocation = GL.GetAttribLocation(handle, attribute);

                attributes[new VertexAttribute(attribute, type)] = attributeLocation;
            }

            return attributes;
        }

        private static Dictionary<string, int> GetUniforms(int handle)
        {
            GL.GetProgram(handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
            var uniforms = new Dictionary<string, int>();

            for (var i = 0; i < numberOfUniforms; i++)
            {
                var key = GL.GetActiveUniform(handle, i, out _, out _);
                var location = GL.GetUniformLocation(handle, key);

                uniforms.Add(key, location);
            }

            return uniforms;
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out var code);

            if (code == (int)All.True) return;

            var infoLog = GL.GetShaderInfoLog(shader);
            throw new Exception($"Error occured while compiling Shader({shader}).\n\n{infoLog}");
        }


        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);

            if (code == (int)All.True) return;

            throw new Exception($"Error occurred while linking Program({program}):\n{GL.GetProgramInfoLog(program)}");
        }

        public override string ToString()
        {
            return $"Shader{{{string.Join(", ", Uniforms)}}}";
        }
    }
}
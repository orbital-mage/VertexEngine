using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Resources;

namespace EngineTests
{
    public class ShaderTests
    {
        private static NativeWindow? window;

        [OneTimeSetUp]
        public void Setup()
        {
            window = new NativeWindow(new NativeWindowSettings
            {
                StartVisible = false,
                Size = Vector2i.One
            });
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            window?.Close();
        }

        [Test]
        public void CreateBasicShaderTest()
        {
            var shader = Shader.FromFiles("shader.frag", "shader.vert");

            Assert.That(shader, Is.Not.Null);
        }

        [Test]
        public void Create3DShaderTest(
            [ValueSource(nameof(addDiffuseTypes))] string diffuseType,
            [ValueSource(nameof(addSpecularTypes))] string specularType,
            [Values(0, 1, 5)] int positionalLightsCount,
            [Values(0, 1, 5)] int directionalLightsCount,
            [Values(0, 1, 5)] int pointLightsCount,
            [Values(0, 1, 5)] int spotLightsCount
        )
        {
            var builder = Shader.Builder()
                .SetFragmentShader("~/_3D/shader.frag")
                .SetVertexShader("~/_3D/shader.vert")
                .AddDefinition(diffuseType)
                .AddDefinition(specularType)
                .AddDefinition(ShaderDefinitions.PositionalLights, pointLightsCount)
                .AddDefinition(ShaderDefinitions.DirectionalLights, directionalLightsCount)
                .AddDefinition(ShaderDefinitions.PointLights, pointLightsCount)
                .AddDefinition(ShaderDefinitions.SpotLights, spotLightsCount);

            var shader = builder.Build();

            Assert.That(shader, Is.Not.Null);
        }

        private static string[] addDiffuseTypes =
        [
            ShaderDefinitions.Diffuse3,
            ShaderDefinitions.Diffuse4,
            ShaderDefinitions.DiffuseTexture
        ];

        private static string[] addSpecularTypes =
        [
            ShaderDefinitions.Specular3,
            ShaderDefinitions.Specular4,
            ShaderDefinitions.SpecularTexture
        ];
    }
}
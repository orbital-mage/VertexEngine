using Moq;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Elements.Interfaces;

namespace EngineTests
{
    public static class TreeRendererTestData
    {
        public static Mock<IAsset>[] SetupBaseAssets(Mock<IElement> mock)
        {
            var shaderMock = CreateShader();
            var voMock = CreateVertexShader();

            SetupBaseAssets(mock, shaderMock, voMock);

            return [shaderMock.As<IAsset>(), voMock.As<IAsset>()];
        }

        public static void SetupBaseAssets(
            Mock<IElement> elementMock,
            Mock<Shader> shaderMock,
            Mock<VertexObject> voMock)
        {
            elementMock
                .SetupGet(root => root.Shader)
                .Returns(shaderMock.Object);
            elementMock
                .SetupGet(root => root.VertexObject)
                .Returns(voMock.Object);
        }

        public static Mock<IAsset> SetupMaterial(Mock<IElement> mock)
        {
            var materialAssetMock = CreateMaterialAsset();

            SetupMaterial(mock, materialAssetMock);

            return materialAssetMock.As<IAsset>();
        }

        public static Mock<Material> SetupMaterial(Mock<IElement> elementMock, Mock<UniformAsset> materialAssetMock)
        {
            var materialMock = new Mock<Material>();

            materialMock
                .As<IMaterial>()
                .SetupGet(material => material.Asset)
                .Returns(materialAssetMock.Object);

            elementMock
                .As<IMaterialElement>()
                .SetupGet(root => root.Material)
                .Returns(materialMock.Object);
            elementMock
                .As<IMaterialElement>()
                .SetupGet(root => root.Textures)
                .Returns([]);

            return materialMock;
        }

        public static void SetupChild(Mock<IElement> rootMock, Mock<IElement> childMock)
        {
            SetupChildren(rootMock, [childMock]);
        }

        public static void SetupChildren(Mock<IElement> rootMock, IEnumerable<Mock<IElement>> childrenMocks)
        {
            rootMock.SetupGet(root => root.Children)
                .Returns(childrenMocks.Select(mock => mock.Object));
        }

        public static Mock<Shader> CreateShader()
        {
            var mock = new Mock<Shader>();
            SetupAsset(mock);
            return mock;
        }

        public static Mock<VertexObject> CreateVertexShader()
        {
            var mock = new Mock<VertexObject>();
            SetupAsset(mock);
            return mock;
        }

        public static Mock<UniformAsset> CreateMaterialAsset()
        {
            var mock = new Mock<UniformAsset>(new Dictionary<string, object>());
            SetupAsset(mock);
            SetupEquality(mock);
            return mock;
        }

        public static void VerifyDraw(Mock<IElement> mock, Times times)
        {
            mock.Verify(element => element.Draw(), times);
        }

        public static void VerifyAsset(Mock mock, Times times)
        {
            mock.As<IAsset>()
                .Verify(asset => asset.Use(It.IsAny<Dictionary<Type, IAsset>>()),
                    times);
        }

        private static void SetupAsset(Mock mock)
        {
            mock.As<IAsset>()
                .Setup(shader => shader.Use(It.IsAny<Dictionary<Type, IAsset>>()))
                .Verifiable();
        }

        private static void SetupEquality<T>(Mock<T> mock) where T : class
        {
            mock
                .Setup(asset => asset.GetHashCode())
                .Returns(mock.GetHashCode());

            mock
                .Setup(asset => asset.Equals(It.IsAny<T>()))
                .Returns((T obj) => obj.GetHashCode() == mock.GetHashCode());
        }
    }
}
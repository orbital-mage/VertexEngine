using Moq;
using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VertexEngine;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Rendering;
using VertexEngine.Common.Elements;
using VertexEngine.Common.Rendering;
using static EngineTests.TreeRendererTestData;
using GameWindow = VertexEngine.GameWindow;

namespace EngineTests
{
    public class TreeRendererTests
    {
        private static readonly Mock<GameWindow> GameWindowMock = new(
            GameWindowSettings.Default,
            new NativeWindowSettings
            {
                StartVisible = false,
                Size = Vector2i.One
            });

        private TreeRenderer renderer;
        private Mock<IElement> rootMock;

        [OneTimeSetUp]
        public void Setup()
        {
            typeof(GameWindow)
                .GetProperty("CurrentWindow")
                ?.SetValue(null, GameWindowMock.Object);

            GameWindowMock
                .As<IGameWindow>()
                .SetupGet(w => w.RenderOptions).Returns(new RenderOptions());
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            GameWindowMock.Object.Close();
        }

        [SetUp]
        public void SetUp()
        {
            rootMock = new Mock<IElement>();
            renderer = new TreeRenderer(rootMock.Object);

            var obj = rootMock.As<IElement>().Object;
            renderer.Root = obj;
        }

        [Test]
        public void DrawEmptyElementTest()
        {
            renderer.UpdateTree();
            renderer.Draw();

            VerifyDraw(rootMock, Times.Never());
        }

        [Test]
        public void DrawRealElementTest()
        {
            var assetMocks = SetupBaseAssets(rootMock);

            renderer.UpdateTree();
            renderer.Draw();

            VerifyDraw(rootMock, Times.Once());
            foreach (var assetMock in assetMocks)
                VerifyAsset(assetMock, Times.Once());
        }

        [Test]
        public void DrawMaterialElementsTest()
        {
            // Given
            var childMock = new Mock<IElement>();

            SetupBaseAssets(childMock);
            var materialMock = SetupMaterial(childMock);

            SetupChild(rootMock, childMock);

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(childMock, Times.Once());
            VerifyAsset(materialMock, Times.Once());
        }

        [Test]
        public void DrawManyIdenticalElementsTest()
        {
            // Given
            var shaderMock = CreateShader();
            var voMock = CreateVertexShader();
            var materialMock = CreateMaterialAsset();
            var mocks = new List<Mock<IElement>>();

            for (var i = 0; i < 100; i++)
            {
                var childMock = new Mock<IElement>();

                SetupBaseAssets(childMock, shaderMock, voMock);
                SetupMaterial(childMock, materialMock);

                mocks.Add(childMock);
            }

            SetupChildren(rootMock, mocks);

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(rootMock, Times.Never());

            VerifyAsset(shaderMock, Times.Once());
            VerifyAsset(voMock, Times.Once());
            VerifyAsset(materialMock, Times.Once());

            foreach (var childMock in mocks)
            {
                VerifyDraw(childMock, Times.Once());
            }
        }

        [Test]
        public void DrawManyUniqueElementsTest()
        {
            var mocks = new List<(Mock<IElement> element, IEnumerable<Mock<IAsset>> assets)>();

            for (var i = 0; i < 100; i++)
            {
                var childMock = new Mock<IElement>();
                var baseAssets = SetupBaseAssets(childMock);
                var material = SetupMaterial(childMock);

                mocks.Add((childMock, baseAssets.Append(material)));
            }

            SetupChildren(rootMock, mocks.Select(mock => mock.element));

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(rootMock, Times.Never());

            foreach (var (child, assets) in mocks)
            {
                VerifyDraw(child, Times.Once());
                foreach (var asset in assets)
                    VerifyAsset(asset, Times.Once());
            }
        }

        [Test]
        public void ChangeAssetTest()
        {
            //Given
            var childMock = new Mock<IElement>();

            var shaderMock = CreateShader();
            var voMock = CreateVertexShader();
            var materialAssetMock = CreateMaterialAsset();
            SetupBaseAssets(childMock, shaderMock, voMock);
            var materialMock = SetupMaterial(childMock, materialAssetMock);

            SetupChild(rootMock, childMock);

            renderer.UpdateTree();
            renderer.Draw();

            var newMaterialAssetMock = CreateMaterialAsset();
            materialMock.As<IMaterial>()
                .SetupGet(material => material.Asset)
                .Returns(newMaterialAssetMock.Object);

            childMock.Raise(child => child.AssetsChanged += null, EventArgs.Empty);

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(rootMock, Times.Never());
            VerifyDraw(childMock, Times.Exactly(2));
            VerifyAsset(shaderMock, Times.Exactly(2));
            VerifyAsset(voMock, Times.Exactly(2));
            VerifyAsset(materialAssetMock, Times.AtLeastOnce());
            VerifyAsset(newMaterialAssetMock, Times.Once());
        }

        [Test]
        public void AddElementTest()
        {
            // Given
            renderer.UpdateTree();
            renderer.Draw();

            var child = new Mock<IElement>();
            var assets = SetupBaseAssets(child);
            SetupChild(rootMock, child);

            rootMock.Raise(root => root.TreeChanged += null, TreeChangedArgs.Removed(child.Object));

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(child, Times.Once());
            foreach (var asset in assets)
                VerifyAsset(asset, Times.Once());
        }

        [Test]
        public void RemoveElementTest()
        {
            // Given
            var child = new Mock<IElement>();
            var assets = SetupBaseAssets(child);
            SetupChild(rootMock, child);

            renderer.UpdateTree();
            renderer.Draw();

            SetupChildren(rootMock, Array.Empty<Mock<IElement>>());

            rootMock.Raise(root => root.TreeChanged += null, TreeChangedArgs.Removed(child.Object));

            // When
            renderer.UpdateTree();
            renderer.Draw();

            // Then
            VerifyDraw(child, Times.Once());
            foreach (var asset in assets)
                VerifyAsset(asset, Times.Once());
        }
    }
}
using Moq;
using Moq.Sequences;
using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Graphics3D.Assets.Lights;
using static EngineTests.AssetSetTestData;

namespace EngineTests
{
    public class AssetSetTests
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
        public void EmptyAssetSetTest()
        {
            var set = new AssetSet<Light>();

            Assert.Multiple(() =>
            {
                Assert.That(set.Asset, Is.Not.Null);
                Assert.That(set.Assets, Is.Empty);
                Assert.That(set.Asset.Assets, Is.Empty);
            });
        }

        [Test]
        public void AddAssetSetTest()
        {
            var lights = new Light[] { new PointLight(), new DirectionalLight(), new PointLight() };
            var set = new AssetSet<Light>();
            set.AddRange(lights);

            Assert.Multiple(() =>
            {
                Assert.That(set.Asset, Is.Not.Null);
                CollectionAssert.AreEquivalent(lights, set.Assets);
                CollectionAssert.AreEquivalent(lights, set.Asset.Assets);
            });
        }

        [Test]
        public void RemoveAssetSetTest()
        {
            var lights = new Light[] { new PointLight(), new DirectionalLight(), new PointLight() };
            var set = new AssetSet<Light>();
            set.AddRange(lights);

            set.RemoveRange(lights.Skip(1));

            Assert.Multiple(() =>
            {
                Assert.That(set.Asset, Is.Not.Null);

                CollectionAssert.AreEquivalent(lights.Take(1), set.Assets);
                CollectionAssert.AreEquivalent(lights.Take(1), set.Asset.Assets);
            });
        }

        [Test]
        public void UseSingleAssetTest()
        {
            using var sequence = Sequence.Create();
            
            var mock = new Mock<PointLight>().As<IIndexedAsset>();
            SetupIndexedAsset(mock);
            var set = new AssetSet<IIndexedAsset> { mock.Object };
            
            set.Asset.Use(new Dictionary<Type, IAsset>());
        }
        
        [Test]
        public void UseManyInSameGroupTest()
        {
            using var sequence = Sequence.Create();

            var mocks = new[]
            {
                new Mock<PointLight>().As<IIndexedAsset>(),
                new Mock<DirectionalLight>().As<IIndexedAsset>(),
                new Mock<PointLight>().As<IIndexedAsset>()
            };
            var set = new AssetSet<IIndexedAsset>();
            var i = 0;

            foreach (var mock in mocks)
            {
                SetupIndexedAsset(mock, ++i);

                set.Add(mock.Object);
            }

            set.Asset.Use(new Dictionary<Type, IAsset>());
        }
        
        [Test]
        public void UseManyInDistinctGroupsTest()
        {
            using var sequence = Sequence.Create();

            var mocks = new[]
            {
                new Mock<PointLight>().As<IIndexedAsset>(),
                new Mock<DirectionalLight>().As<IIndexedAsset>(),
                new Mock<PointLight>().As<IIndexedAsset>()
            };
            var set = new AssetSet<IIndexedAsset>();
            var i = 0;

            foreach (var mock in mocks)
            {
                SetupIndexedAsset(mock, ++i, i.ToString());

                set.Add(mock.Object);
            }

            set.Asset.Use(new Dictionary<Type, IAsset>());
        }
    }
}
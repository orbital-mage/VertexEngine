using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Elements;

namespace EngineTests
{
    public class ElementTests
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
        public void CreateEmptyElementTest()
        {
            var element = new Element();

            Assert.That(element, Is.Not.Null);
        }

        [Test]
        public void CreateBasicElementTest()
        {
            var shader = Shader.FromFiles("shader.frag", "shader.vert");
            var vertexObject = VertexObject.Empty;
            var element = new Element(vertexObject, shader);

            Assert.Multiple(() =>
            {
                Assert.That(element.Shader, Is.EqualTo(shader));
                Assert.That(element.VertexObject, Is.EqualTo(vertexObject));
            });
        }

        [Test]
        public void SetParentTest()
        {
            var root = new Element();
            var child = new Element
            {
                Parent = root
            };
            
            Assert.Multiple(() =>
            {
                Assert.That(child.Parent, Is.EqualTo(root));
                Assert.That(root.Children.ToArray(), Does.Contain(child));
            });
        }
        
        [Test]
        public void AddChildTest()
        {
            var root = new Element();
            var child = new Element();
            
            root.AddChild(child);
            
            Assert.Multiple(() =>
            {
                Assert.That(child.Parent, Is.EqualTo(root));
                Assert.That(root.Children.ToArray(), Does.Contain(child));
            });
        }

        [Test]
        public void RemoveChildTest()
        {
            var root = new Element();
            var child = new Element();
            
            root.AddChild(child);
            root.RemoveChild(child);
            
            Assert.Multiple(() =>
            {
                Assert.That(child.Parent, Is.Null);
                Assert.That(root.Children.ToArray(), Is.Empty);
            });
        }

        [Test]
        public void ChangeParentTest()
        {
            var root1 = new Element();
            var root2 = new Element();
            var child = new Element
            {
                Parent = root1
            };

            child.Parent = root2;

            Assert.Multiple(() =>
            {
                Assert.That(child.Parent, Is.EqualTo(root2));
                Assert.That(root2.Children.ToArray(), Does.Contain(child));
            });
        }
    }
}
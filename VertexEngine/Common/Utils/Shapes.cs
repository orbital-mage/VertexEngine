using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Resources.Models;

namespace VertexEngine.Common.Utils
{
    public static class Shapes
    {
        public static readonly VertexObject Square = VertexObject.From(
            [
                1f, 1f, 0f, 1.0f, 1.0f, 0f, 0f, -1f,
                1f, -1f, 0f, 1.0f, 0.0f, 0f, 0f, -1f,
                -1f, -1f, 0f, 0.0f, 0.0f, 0f, 0f, -1f,
                -1f, 1f, 0f, 0.0f, 1.0f, 0f, 0f, -1f
            ],
            [
                0, 1, 3,
                1, 2, 3
            ],
            [
                VertexAttribute.Position3D, VertexAttribute.TextureCoordinates2D, VertexAttribute.Normals3D
            ]);

        public static readonly VertexObject Cube = ModelImporter.ImportVertexObjectFromFile("~/cube.obj");
        public static readonly VertexObject Sphere = ModelImporter.ImportVertexObjectFromFile("~/sphere.obj");
    }
}
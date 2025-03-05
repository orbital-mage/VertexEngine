using Assimp;
using VertexEngine.Common.Assets.Mesh;

namespace VertexEngine.Common.Resources.Models
{
    internal static class MeshParser
    {
        internal static VertexObject ParseVertexObject(Mesh mesh)
        {
            var (vertices, indices, attributes) = ParseVertexData(mesh);
            return VertexObject.From(vertices, indices, attributes);
        }

        internal static VertexObject ParseVertexObject(IEnumerable<Mesh> meshes)
        {
            var (vertices, indices, attributes) = ParseVertexData(meshes);
            return VertexObject.From(vertices, indices, attributes);
        }

        internal static (float[], uint[], VertexAttribute[]) ParseVertexData(Mesh mesh)
        {
            return (GetVerticesFromMesh(mesh), mesh.GetUnsignedIndices(), GetVertexAttributesFromMesh(mesh));
        }

        internal static (float[] vertices, uint[] indices, VertexAttribute[]) ParseVertexData(IEnumerable<Mesh> meshes)
        {
            var vertices = new List<float>();
            var indices = new List<uint>();

            var meshArray = meshes as Mesh[] ?? meshes.ToArray();
            var attributes = GetVertexAttributesFromMesh(meshArray.First());

            foreach (var mesh in meshArray)
            {
                if (!GetVertexAttributesFromMesh(mesh).SequenceEqual(attributes)) continue;

                var startIndex = indices.Count > 0 ? indices.Max() + 1 : 0;
                indices.AddRange(mesh.GetUnsignedIndices().Select(index => index + startIndex));

                vertices.AddRange(GetVerticesFromMesh(mesh));
            }

            return (vertices.ToArray(), indices.ToArray(), attributes);
        }

        private static float[] GetVerticesFromMesh(Mesh mesh)
        {
            var vertices = new List<float>();

            for (var i = 0; i < mesh.VertexCount; i++)
            {
                vertices.AddRange(Deconstruct(mesh.Vertices[i]));

                if (mesh.TextureCoordinateChannelCount > 0)
                {
                    vertices.AddRange(Deconstruct(mesh.TextureCoordinateChannels[0][i]).Take(2));
                }

                if (mesh.HasNormals)
                {
                    vertices.AddRange(Deconstruct(mesh.Normals[i]));
                }
            }

            return vertices.ToArray();
        }

        private static VertexAttribute[] GetVertexAttributesFromMesh(Mesh mesh)
        {
            var attributes = new List<VertexAttribute>
            {
                VertexAttribute.Position3D
            };

            if (mesh.TextureCoordinateChannelCount > 0) attributes.Add(VertexAttribute.TextureCoordinates2D);
            if (mesh.HasNormals) attributes.Add(VertexAttribute.Normals3D);

            return attributes.ToArray();
        }

        private static IEnumerable<float> Deconstruct(Vector3D vector)
        {
            return new[] {vector.X, vector.Y, vector.Z};
        }
    }
}
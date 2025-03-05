using Assimp;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Utils;
using VertexEngine.Graphics3D.Elements;
using Material = VertexEngine.Common.Assets.Materials.Material;
using RawMaterial = Assimp.Material;

namespace VertexEngine.Common.Resources.Models
{
    public static class ModelImporter
    {
        private static readonly AssimpContext Context = new();

        public static Element3D ImportElementsFromFile(string file,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            return ImportElementsFromFile(file, new Element3D(), postProcessSteps);
        }

        public static Element3D ImportElementsFromFile(string file, Element3D root,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            var elementData = ParseElements(ResourceUtils.ReadSceneResource(file, postProcessSteps));

            var materials = MaterialParser.ParseMaterials(elementData.Materials);
            root.AddChildren(elementData.Meshes
                .Select(mesh => new Element3D(VertexObject.From(mesh.Vertices, mesh.Indices, mesh.Attributes))
                    {
                        Material = materials[mesh.MaterialIndex]
                    }
                ));

            return root;
        }

        public static Material[] ImportMaterialsFromFile(string file,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            return MaterialParser.ParseMaterials(ResourceUtils.ReadSceneResource(file, postProcessSteps).Materials);
        }

        public static VertexObject ImportVertexObjectFromFile(string file,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            return MeshParser.ParseVertexObject(ResourceUtils.ReadSceneResource(file, postProcessSteps).Meshes);
        }

        public static VertexObject[] ImportVertexObjectsFromFile(string file,
            PostProcessSteps postProcessSteps = PostProcessSteps.None)
        {
            var meshes = ResourceUtils.ReadSceneResource(file, postProcessSteps).Meshes;
            return meshes.Select(MeshParser.ParseVertexObject).ToArray();
        }

        public static Scene ReadSceneFromFile(string file, PostProcessSteps postProcessSteps)
        {
            return Context.ImportFile(file,
                PostProcessSteps.Triangulate |
                PostProcessSteps.JoinIdenticalVertices |
                PostProcessSteps.RemoveRedundantMaterials |
                PostProcessSteps.GenerateUVCoords |
                postProcessSteps);
        }

        public static Scene ReadSceneFromStream(Stream stream, PostProcessSteps postProcessSteps, string formatHint = null)
        {
            return Context.ImportFileFromStream(stream,
                PostProcessSteps.Triangulate |
                PostProcessSteps.JoinIdenticalVertices |
                PostProcessSteps.RemoveRedundantMaterials |
                PostProcessSteps.GenerateUVCoords |
                postProcessSteps,
                formatHint);
        }

        private static ElementData ParseElements(Scene scene)
        {
            return new ElementData
            {
                Meshes = scene.Meshes.Select(ParseMesh).ToArray(),
                Materials = scene.Materials.ToArray(),
            };
        }

        private static MeshData ParseMesh(Mesh mesh)
        {
            var (vertices, indices, attributes) = MeshParser.ParseVertexData(mesh);

            return new MeshData
            {
                Vertices = vertices,
                Indices = indices,
                Attributes = attributes,
                MaterialIndex = mesh.MaterialIndex
            };
        }
    }

    public struct ElementData
    {
        public MeshData[] Meshes;
        public RawMaterial[] Materials;
    }

    public struct MeshData
    {
        public float[] Vertices;
        public uint[] Indices;
        public VertexAttribute[] Attributes;
        public int MaterialIndex;
    }
}
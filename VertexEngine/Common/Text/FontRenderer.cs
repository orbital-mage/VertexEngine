using FontStashSharp.Interfaces;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Mesh;
using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Common.Text;

public class FontRenderer : IFontStashRenderer2
{
    private const int MaxSprites = 2048;
    private static readonly int MaxVertices = GetVertexCount(MaxSprites);
    private static readonly int MaxIndices = GetIndexCount(MaxSprites);

    private static readonly uint[] Indices = GenerateIndices();

    private static readonly VertexAttribute[] VertexAttributes =
    [
        VertexAttribute.Position3D,
        VertexAttribute.TextureCoordinates2D,
    ];

    public ITexture2DManager TextureManager { get; } = new Texture2DManager();

    private readonly float[] vertices = new float[MaxVertices];
    private readonly Texture2D[] textures = new Texture2D[MaxSprites];

    private int vertexIndex;
    private int textureIndex;
    private int spriteIndex;

    public void DrawQuad(object texture,
        ref VertexPositionColorTexture topLeft,
        ref VertexPositionColorTexture topRight,
        ref VertexPositionColorTexture bottomLeft,
        ref VertexPositionColorTexture bottomRight)
    {
        spriteIndex++;

        AddTexture(texture);

        AddVertex(topLeft);
        AddVertex(topRight);
        AddVertex(bottomLeft);
        AddVertex(bottomRight);
    }

    public (VertexObject vertexObject, Texture2D atlas, Vector2i size) CreateAssets()
    {
        var vertexArray = vertices.Take(GetVertexCount(spriteIndex)).ToArray();
        var indexArray = Indices.Take(GetIndexCount(spriteIndex)).ToArray();
        var texture = textures[textureIndex - 1];

        vertexIndex = 0;
        textureIndex = 0;
        spriteIndex = 0;

        var (modelSpaceVertices, size) = ConvertToModelSpace(vertexArray);

        return (VertexObject.From(modelSpaceVertices, indexArray, VertexAttributes), texture, size);
    }

    private static (float[] vertices, Vector2i size) ConvertToModelSpace(float[] vertices)
    {
        const int stride = 5;

        if (vertices.Length == 0)
            return (vertices, Vector2i.Zero);

        var minX = float.MaxValue;
        var minY = float.MaxValue;
        var maxX = float.MinValue;
        var maxY = float.MinValue;

        for (var i = 0; i < vertices.Length; i += stride)
        {
            minX = Math.Min(minX, vertices[i]);
            maxX = Math.Max(maxX, vertices[i]);
            minY = Math.Min(minY, vertices[i + 1]);
            maxY = Math.Max(maxY, vertices[i + 1]);
        }

        var width = Math.Max(maxX - minX, 1f);
        var height = Math.Max(maxY - minY, 1f);
        var normalized = new float[vertices.Length];
        Array.Copy(vertices, normalized, vertices.Length);

        for (var i = 0; i < normalized.Length; i += stride)
        {
            normalized[i] = 2f * (normalized[i] - minX) / width - 1f;
            normalized[i + 1] = 1f - 2f * (normalized[i + 1] - minY) / height;
        }

        return (normalized, new Vector2i((int)Math.Ceiling(width), (int)Math.Ceiling(height)));
    }

    private void AddTexture(object texture)
    {
        if (texture is not Texture2D texture2D) throw new ArgumentException("Texture is not a Texture2D");
        if (textureIndex > 0 && texture == textures[textureIndex - 1]) return;

        textures[textureIndex++] = texture2D;
    }

    private void AddVertex(VertexPositionColorTexture vertex)
    {
        vertices[vertexIndex++] = vertex.Position.X;
        vertices[vertexIndex++] = vertex.Position.Y;
        vertices[vertexIndex++] = vertex.Position.Z;

        vertices[vertexIndex++] = vertex.TextureCoordinate.X;
        vertices[vertexIndex++] = vertex.TextureCoordinate.Y;
    }

    private static uint[] GenerateIndices()
    {
        var indices = new uint[MaxIndices];

        for (uint i = 0, j = 0; i < MaxIndices; i += 6, j += 4)
        {
            indices[i] = j;
            indices[i + 1] = j + 1;
            indices[i + 2] = j + 2;
            indices[i + 3] = j + 3;
            indices[i + 4] = j + 2;
            indices[i + 5] = j + 1;
        }

        return indices;
    }

    private static int GetVertexCount(int spriteCount)
    {
        return spriteCount * 4 * 5;
    }

    private static int GetIndexCount(int spriteCount)
    {
        return spriteCount * 6;
    }
}
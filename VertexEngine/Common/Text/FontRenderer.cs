using FontStashSharp.Interfaces;
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

    public (VertexObject vertexObject, Texture2D atlas) CreateAssets()
    {
        var vertexArray = vertices.Take(GetVertexCount(spriteIndex)).ToArray();
        var indexArray = Indices.Take(GetIndexCount(spriteIndex)).ToArray();
        var texture = textures[textureIndex - 1];

        vertexIndex = 0;
        textureIndex = 0;
        spriteIndex = 0;

        return (VertexObject.From(vertexArray, indexArray, VertexAttributes), texture);
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
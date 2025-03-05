using OpenTK.Graphics.OpenGL;
using VertexEngine.Common.Utils;

namespace VertexEngine.Common.Assets.Mesh
{
    public record struct VertexAttribute(string Name, VertexAttribPointerType Type, int Length, bool Normalized)
    {
        private const string PositionName = "Position";
        private const string TextureCoordinatesName = "TextureCoords";
        private const string NormalsName = "Normal";
        private const string ColorName = "Color";

        public VertexAttribute(string name, ActiveAttribType type) :
            this(name, type.PointerType(), type.Length(), false)
        {
        }

        public static readonly VertexAttribute Position3D = new(PositionName, ActiveAttribType.FloatVec3);

        public static readonly VertexAttribute TextureCoordinates2D =
            new(TextureCoordinatesName, ActiveAttribType.FloatVec2);

        public static readonly VertexAttribute Normals3D = new(NormalsName, ActiveAttribType.FloatVec3);

        public static readonly VertexAttribute Color = new(ColorName, ActiveAttribType.FloatVec4);

        public static VertexAttribute Position(ActiveAttribType type) => new(PositionName, type);
    }
}
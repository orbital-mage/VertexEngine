using System.Drawing;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace VertexEngine.Common.Utils
{
    public static class GLExtentions
    {
        public static int Size(this ActiveAttribType type)
        {
            return type.Length() * type.PointerType().Size();
        }

        public static int Length(this ActiveAttribType type)
        {
            switch (type)
            {
                case ActiveAttribType.None:
                    return 0;
                case ActiveAttribType.Int:
                case ActiveAttribType.UnsignedInt:
                case ActiveAttribType.Float:
                case ActiveAttribType.Double:
                    return 1;
                case ActiveAttribType.IntVec2:
                case ActiveAttribType.UnsignedIntVec2:
                case ActiveAttribType.FloatVec2:
                case ActiveAttribType.DoubleVec2:
                    return 2;
                case ActiveAttribType.IntVec3:
                case ActiveAttribType.UnsignedIntVec3:
                case ActiveAttribType.FloatVec3:
                case ActiveAttribType.DoubleVec3:
                    return 3;
                case ActiveAttribType.IntVec4:
                case ActiveAttribType.UnsignedIntVec4:
                case ActiveAttribType.FloatVec4:
                case ActiveAttribType.FloatMat2:
                case ActiveAttribType.DoubleVec4:
                case ActiveAttribType.DoubleMat2:
                    return 4;
                case ActiveAttribType.FloatMat2x3:
                case ActiveAttribType.FloatMat3x2:
                case ActiveAttribType.DoubleMat2x3:
                case ActiveAttribType.DoubleMat3x2:
                    return 6;
                case ActiveAttribType.FloatMat2x4:
                case ActiveAttribType.FloatMat4x2:
                case ActiveAttribType.DoubleMat2x4:
                case ActiveAttribType.DoubleMat4x2:
                    return 8;
                case ActiveAttribType.FloatMat3:
                case ActiveAttribType.DoubleMat3:
                    return 9;
                case ActiveAttribType.FloatMat3x4:
                case ActiveAttribType.FloatMat4x3:
                case ActiveAttribType.DoubleMat3x4:
                case ActiveAttribType.DoubleMat4x3:
                    return 12;
                case ActiveAttribType.FloatMat4:
                case ActiveAttribType.DoubleMat4:
                    return 16;
                default: throw new ArgumentOutOfRangeException(nameof(type), "Not supported attribute type");
            }
        }

        public static VertexAttribPointerType PointerType(this ActiveAttribType type)
        {
            switch (type)
            {
                case ActiveAttribType.Int:
                case ActiveAttribType.IntVec2:
                case ActiveAttribType.IntVec3:
                case ActiveAttribType.IntVec4:
                    return VertexAttribPointerType.Int;
                case ActiveAttribType.UnsignedInt:
                case ActiveAttribType.UnsignedIntVec2:
                case ActiveAttribType.UnsignedIntVec3:
                case ActiveAttribType.UnsignedIntVec4:
                    return VertexAttribPointerType.UnsignedInt;
                case ActiveAttribType.Float:
                case ActiveAttribType.FloatVec2:
                case ActiveAttribType.FloatVec3:
                case ActiveAttribType.FloatVec4:
                case ActiveAttribType.FloatMat2:
                case ActiveAttribType.FloatMat2x3:
                case ActiveAttribType.FloatMat2x4:
                case ActiveAttribType.FloatMat3:
                case ActiveAttribType.FloatMat3x2:
                case ActiveAttribType.FloatMat3x4:
                case ActiveAttribType.FloatMat4:
                case ActiveAttribType.FloatMat4x2:
                case ActiveAttribType.FloatMat4x3:
                    return VertexAttribPointerType.Float;
                case ActiveAttribType.Double:
                case ActiveAttribType.DoubleVec2:
                case ActiveAttribType.DoubleVec3:
                case ActiveAttribType.DoubleVec4:
                case ActiveAttribType.DoubleMat2:
                case ActiveAttribType.DoubleMat2x3:
                case ActiveAttribType.DoubleMat2x4:
                case ActiveAttribType.DoubleMat3:
                case ActiveAttribType.DoubleMat3x2:
                case ActiveAttribType.DoubleMat3x4:
                case ActiveAttribType.DoubleMat4:
                case ActiveAttribType.DoubleMat4x2:
                case ActiveAttribType.DoubleMat4x3:
                    return VertexAttribPointerType.Double;
                case ActiveAttribType.None:
                default: throw new ArgumentOutOfRangeException(nameof(type), "Not supported attribute type");
            }
        }

        public static int Size(this VertexAttribPointerType type)
        {
            return type switch
            {
                VertexAttribPointerType.Int => sizeof(int),
                VertexAttribPointerType.UnsignedInt => sizeof(uint),
                VertexAttribPointerType.Float => sizeof(float),
                VertexAttribPointerType.Double => sizeof(double),
                VertexAttribPointerType.Byte => sizeof(byte),
                VertexAttribPointerType.UnsignedByte => sizeof(byte),
                _ => 0
            };
        }

        public static Point ToPoint(this Vector2i v)
        {
            return new Point(v.X, v.Y);
        }
        
        public static Vector2i ToVector2I(this Vector2 v)
        {
            return new Vector2i((int) v.X, (int) v.Y);
        }
        
        public static Vector2i ToVector2I(this Point v)
        {
            return new Vector2i(v.X, v.Y);
        }
        
        public static Vector2i ToVector2I(this Size v)
        {
            return new Vector2i(v.Width, v.Height);
        }

        public static Vector2 ToOpenTkVector2(this System.Numerics.Vector2 v)
        {
            return new Vector2(v.X, v.Y);
        }
        
        public static System.Numerics.Vector2 ToSystemVector2(this Vector2 v)
        {
            return new System.Numerics.Vector2(v.X, v.Y);
        }
        
        public static System.Numerics.Vector2 ToSystemVector2(this Vector2i v)
        {
            return new System.Numerics.Vector2(v.X, v.Y);
        }
    }
}
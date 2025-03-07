using OpenTK.Graphics.OpenGL;
using VertexEngine.Common.Utils;

namespace VertexEngine.Common.Assets.Mesh
{
    public class VertexObject : IAsset
    {
        private static readonly Dictionary<VertexObjectKey, VertexObject> VertexObjects = new();

        public static readonly VertexObject Empty = From([], []);

        private readonly float[] vertices;
        private readonly uint[]? indices;
        private readonly VertexAttribute[] attributes;
        private readonly PrimitiveType primitiveType;

        private readonly int vertexBufferObject;
        private readonly int vertexArrayObject;
        private readonly int elementBufferObject;
        private int vertexCount;
        private Dictionary<VertexAttribute, int>? boundAttributes;

        public static VertexObject From(float[] vertices, VertexAttribute[] attributes)
        {
            return From(vertices, null, attributes);
        }

        public static VertexObject From(float[] vertices, VertexAttribute[] attributes, PrimitiveType primitiveType)
        {
            return From(vertices, null, attributes, primitiveType);
        }

        public static VertexObject From(float[] vertices, uint[]? indices, VertexAttribute[] attributes)
        {
            return From(vertices, indices, attributes, PrimitiveType.Triangles);
        }

        public static VertexObject From(
            float[] vertices,
            uint[]? indices,
            VertexAttribute[] attributes,
            PrimitiveType primitiveType)
        {
            var key = new VertexObjectKey(vertices, indices, attributes, primitiveType);

            if (VertexObjects.TryGetValue(key, out var value)) return value;

            value = new VertexObject(vertices, indices, attributes, primitiveType);
            VertexObjects[key] = value;

            return value;
        }
        
        // Needs empty constructor for tests
        #pragma warning disable CS8618, CS9264
        internal VertexObject()
        {
            
        }
        #pragma warning restore CS8618, CS9264

        private VertexObject(
            float[] vertices,
            uint[]? indices,
            VertexAttribute[] attributes,
            PrimitiveType primitiveType)
        {
            this.vertices = vertices;
            this.indices = indices;
            this.attributes = attributes;
            this.primitiveType = primitiveType;

            vertexBufferObject = GL.GenBuffer();
            BufferVertexBufferObject();
            vertexArrayObject = GL.GenVertexArray();
            BindVertexArrayObject();

            if (indices == null) return;

            elementBufferObject = GL.GenBuffer();
            BufferElementBufferObject();
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            BindVertexBufferObject();
            BindVertexArrayObject();
            BindElementBufferObject();
            SetShaderVertexAttributes(IAsset.GetAsset<Shader>(currentAssets)?.Attributes);
        }

        public void Draw()
        {
            if (indices == null)
                GL.DrawArrays(primitiveType, 0, vertexCount);
            else
                GL.DrawElements(primitiveType, indices.Length * sizeof(uint), DrawElementsType.UnsignedInt, 0);
        }

        private void SetShaderVertexAttributes(Dictionary<VertexAttribute, int>? shaderAttributes)
        {
            if (shaderAttributes == null || BoundAttributesUnchanged(shaderAttributes)) return;

            boundAttributes = shaderAttributes;
            EnableVertexAttributes(shaderAttributes);

            if (indices == null)
                vertexCount = vertices.Length /
                              shaderAttributes.Keys.Select(attribute => attribute.Length).Sum();
        }

        private void BufferVertexBufferObject()
        {
            BindVertexBufferObject();
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices,
                BufferUsageHint.StaticDraw);
        }

        private void BufferElementBufferObject()
        {
            if (indices == null) return;

            BindElementBufferObject();
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices,
                BufferUsageHint.StaticDraw);
        }

        private void BindVertexBufferObject()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        }

        private void BindVertexArrayObject()
        {
            GL.BindVertexArray(vertexArrayObject);
        }

        private void BindElementBufferObject()
        {
            if (indices == null) return;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferObject);
        }

        private void EnableVertexAttributes(Dictionary<VertexAttribute, int> shaderAttributes)
        {
            var offset = 0;
            var stride = CalculateStride();

            foreach (var attribute in attributes)
            {
                if (shaderAttributes.TryGetValue(attribute, out var index))
                {
                    GL.VertexAttribPointer(
                        index, attributes.Length, attribute.Type, attribute.Normalized, stride, offset);
                    GL.EnableVertexAttribArray(index);
                }

                offset += attribute.Length * attribute.Type.Size();
            }
        }

        private int CalculateStride()
        {
            if (indices == null)
                return attributes.Select(a => a.Length * a.Type.Size()).Sum();

            return (int)(sizeof(float) * (vertices.Length / (indices.Max() + 1)));
        }

        private bool BoundAttributesUnchanged(Dictionary<VertexAttribute, int> shaderAttributes)
        {
            return boundAttributes != null &&
                   (shaderAttributes == boundAttributes ||
                    shaderAttributes.All(attribute =>
                        boundAttributes.ContainsKey(attribute.Key) &&
                        boundAttributes[attribute.Key].Equals(attribute.Value)));
        }

        private readonly record struct VertexObjectKey(
            float[] Vertices,
            uint[]? Indices,
            VertexAttribute[] Attributes,
            PrimitiveType PrimitiveType)
        {
            public bool Equals(VertexObjectKey? other)
            {
                return other is { } value &&
                       Vertices.SequenceEqual(value.Vertices) &&
                       (Indices?.SequenceEqual(value.Indices ?? []) ?? Indices == value.Indices) &&
                       Attributes.SequenceEqual(value.Attributes) &&
                       value.PrimitiveType == PrimitiveType;
            }

            public override int GetHashCode()
            {
                var hash = new HashCode();

                foreach (var vertex in Vertices) hash.Add(vertex);
                foreach (var index in Indices ?? []) hash.Add(index);
                foreach (var attribute in Attributes) hash.Add(attribute);
                hash.Add(PrimitiveType);

                return hash.ToHashCode();
            }
        }
    }
}
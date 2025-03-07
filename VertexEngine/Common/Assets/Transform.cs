using OpenTK.Mathematics;

namespace VertexEngine.Common.Assets
{
    public abstract class Transform : IAsset
    {
        private const string TransformUniform = "transform";

        private readonly Dictionary<string, object> uniforms = new()
        {
            {TransformUniform, Matrix4.Identity}
        };

        public event EventHandler? TransformChanged;

        public Matrix4 Matrix
        {
            get => (Matrix4) uniforms[TransformUniform];
            set
            {
                uniforms[TransformUniform] = value;

                OnTransformChanged();
            }
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            UniformUtils.SetUniforms(currentAssets, uniforms);
        }

        private void OnTransformChanged()
        {
            TransformChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
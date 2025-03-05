using OpenTK.Mathematics;

namespace VertexEngine.Common.Assets
{
    public abstract class Camera : IAsset
    {
        private const string ViewUniform = "view";
        private const string ProjectionUniform = "projection";
        private const string ViewPosUniform = "viewPos";

        private readonly Dictionary<string, object> uniforms = new()
        {
            {ViewUniform, Matrix4.Identity},
            {ProjectionUniform, Matrix4.Identity},
            {ViewPosUniform, Vector3.Zero},
        };

        public Matrix4 ViewMatrix
        {
            get => (Matrix4) uniforms[ViewUniform];
            protected set => uniforms[ViewUniform] = value;
        }

        public Matrix4 ProjectionMatrix
        {
            get => (Matrix4) uniforms[ProjectionUniform];
            protected set => uniforms[ProjectionUniform] = value;
        }

        public Vector3 ViewPosition
        {
            get => (Vector3) uniforms[ViewPosUniform];
            protected set => uniforms[ViewPosUniform] = value;
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            UniformUtils.SetUniforms(currentAssets, uniforms);
        }
    }
}
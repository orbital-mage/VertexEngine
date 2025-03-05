namespace VertexEngine.Graphics3D.Assets.Lights
{
    public class PointLight : PositionalLight
    {
        public new const string SetName = "pointLights";
        private const string ConstantUniform = "constant";
        private const string LinearUniform = "linear";
        private const string QuadraticUniform = "quadratic";

        protected override string PrefixName => SetName;

        protected override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            { ConstantUniform, Constant },
            { LinearUniform, Linear },
            { QuadraticUniform, Quadratic }
        };

        public float Constant { get; set; }
        public float Linear { get; set; }
        public float Quadratic { get; set; }
    }
}
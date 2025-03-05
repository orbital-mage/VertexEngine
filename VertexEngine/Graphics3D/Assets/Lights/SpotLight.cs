namespace VertexEngine.Graphics3D.Assets.Lights
{
    public class SpotLight : PointLight
    {
        public new const string SetName = "spotLights";
        private const string CutOffUniform = "cutOff";
        private const string OuterCutOffUniform = "outerCutOff";

        protected override string PrefixName => SetName;

        protected override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            {CutOffUniform, CutOff},
            {OuterCutOffUniform, OuterCutOff}
        };

        public float CutOff { get; set; }
        public float OuterCutOff { get; set; }
    }
}
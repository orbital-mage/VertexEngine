using OpenTK.Mathematics;
using VertexEngine.Graphics3D.Assets.Lights.Shadows;

namespace VertexEngine.Graphics3D.Assets.Lights
{
    public class PositionalLight : Light
    {
        public const string SetName = "lights";
        private const string PositionUniform = "position";

        private Vector3 position;

        protected override string PrefixName => SetName;

        protected override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            { PositionUniform, Position }
        };

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                if (ShadowMap != null) ShadowMap.Position = position;
            }
        }

        public new OmnidirectionalShadowMap? ShadowMap
        {
            get => base.ShadowMap as OmnidirectionalShadowMap;
            init
            {
                base.ShadowMap = value;
                if (ShadowMap != null) ShadowMap.Position = position;
            }
        }
    }
}
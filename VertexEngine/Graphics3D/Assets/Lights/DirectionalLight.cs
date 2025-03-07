using OpenTK.Mathematics;
using VertexEngine.Graphics3D.Assets.Lights.Shadows;

namespace VertexEngine.Graphics3D.Assets.Lights
{
    public class DirectionalLight : Light
    {
        public const string SetName = "dirLights";
        private const string DirectionUniform = "direction";

        private Vector3 direction;

        protected override string PrefixName => SetName;

        protected override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            { DirectionUniform, Direction }
        };

        public Vector3 Direction
        {
            get => direction;
            set
            {
                direction = value;
                if (ShadowMap != null) ShadowMap.Direction = Direction;
            }
        }

        public new DirectionalShadowMap? ShadowMap
        {
            get => base.ShadowMap as DirectionalShadowMap;
            init
            {
                base.ShadowMap = value;
                if (ShadowMap != null) ShadowMap.Direction = direction;
            }
        }
    }
}
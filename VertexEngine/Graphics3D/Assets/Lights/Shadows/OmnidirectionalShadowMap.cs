using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets;

namespace VertexEngine.Graphics3D.Assets.Lights.Shadows
{
    public class OmnidirectionalShadowMap : ShadowMap
    {
        private const string LightPosUniform = "lightPos";
        private const string FarPlaneUniform = "farPlane";
        private const string LightSpaceMatricesUniform = "lightSpaces[0]";
        private const string ShadowMapFarPlaneUniform = "shadowMap.farPlane";
        private const string ShadowPcfOffsetUniform = "shadowMap.pcfOffset";
        private const string ShadowPcfSamplesUniform = "shadowMap.pcfSamples";

        private static readonly Shader OmnidirectionalShdShadowMapShader =
            Shader.Builder()
                .SetFragmentShader("~/_3D/Shadows/omnidirectional.frag")
                .SetVertexShader("~/_3D/Shadows/omnidirectional.vert")
                .SetGeometryShader("~/_3D/Shadows/omnidirectional.geom")
                .Build();

        private readonly Matrix4[] lightSpaceTransforms = new Matrix4[6];
        private Vector3 position;
        private float nearPlane = 1f;
        private float farPlane = 15;

        public OmnidirectionalShadowMap()
        {
            Texture = Cubemap.Empty(Vector2i.One * 2048,
                PixelInternalFormat.DepthComponent,
                PixelFormat.DepthComponent,
                PixelType.Float);
            Texture.SetMinMagFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            Texture.SetWrapMode(TextureWrapMode.ClampToEdge);
        }

        public override Shader Shader => OmnidirectionalShdShadowMapShader;

        public new Cubemap Texture
        {
            get => base.Texture as Cubemap;
            private init => base.Texture = value;
        }

        public override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            { ShadowMapFarPlaneUniform, FarPlane },
            { ShadowPcfOffsetUniform, FeatherOffset },
            { ShadowPcfSamplesUniform, FeatherSamples }
        };

        // TODO: get actual size from cubemap
        protected override Vector2i Size => Vector2i.One * 2048;

        public float NearPlane
        {
            get => nearPlane;
            set
            {
                nearPlane = value;
                UpdateLightSpaceMatrix();
            }
        }

        public float FarPlane
        {
            get => farPlane;
            set
            {
                farPlane = value;
                UpdateLightSpaceMatrix();
            }
        }

        public float FeatherOffset { get; set; } = 0.1f;

        public int FeatherSamples { get; set; } = 0;

        internal Vector3 Position
        {
            set
            {
                position = value;
                UpdateLightSpaceMatrix();
            }
        }

        public override void Bind()
        {
            base.Bind();

            Shader.SetUniform(LightSpaceMatricesUniform, lightSpaceTransforms);

            Shader.SetUniform(LightPosUniform, position);
            Shader.SetUniform(FarPlaneUniform, FarPlane);
        }

        private void UpdateLightSpaceMatrix()
        {
            for (var i = 0; i < 6; i++)
            {
                lightSpaceTransforms[i] = GetViewMatrix(i) * GetProjectionMatrix();
            }
        }

        private Matrix4 GetViewMatrix(int face)
        {
            var (target, up) = face switch
            {
                0 => (Vector3.UnitX, -Vector3.UnitY),
                1 => (-Vector3.UnitX, -Vector3.UnitY),
                2 => (Vector3.UnitY, Vector3.UnitZ),
                3 => (-Vector3.UnitY, -Vector3.UnitZ),
                4 => (Vector3.UnitZ, -Vector3.UnitY),
                5 => (-Vector3.UnitZ, -Vector3.UnitY),
                _ => (Vector3.Zero, Vector3.Zero)
            };

            return Matrix4.LookAt(position, position + target, up);
        }

        private Matrix4 GetProjectionMatrix()
        {
            var aspect = (float)Size.X / Size.Y;
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver2, aspect, NearPlane, FarPlane);
        }
    }
}
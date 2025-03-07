using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Graphics3D.Assets.Lights.Shadows
{
    public class DirectionalShadowMap : ShadowMap
    {
        private const string LightSpaceMatrixUniform = "lightSpace";
        private const string ShadowMapSpaceMatrixUniform = "shadowMap.space";
        private const string ShadowPcfRadiusUniform = "shadowMap.pcfRadius";

        private static readonly Shader DirectionalShdShadowMapShader =
            Shader.FromFiles("~/_3D/Shadows/directional.frag", "~/_3D/Shadows/directional.vert");

        private Vector3 direction;
        private float distance = 1;
        private Vector2 planeSize = Vector2.One;
        private float nearPlane = 1;
        private float farPlane = 10;
        private Vector3 offset;

        public DirectionalShadowMap() : this(Vector2i.One * 2048)
        {
        }

        public DirectionalShadowMap(Vector2i resolution) : base(Texture2D.Empty(
            resolution,
            PixelInternalFormat.DepthComponent,
            PixelFormat.DepthComponent,
            PixelType.Float))
        {
            Texture.SetMinMagFilter(TextureMinFilter.Nearest, TextureMagFilter.Nearest);
            Texture.SetWrapMode(TextureWrapMode.ClampToBorder);
            Texture.SetBorderColor(Vector4.One);
        }

        public override Shader Shader => DirectionalShdShadowMapShader;

        public new Texture2D Texture => (base.Texture as Texture2D)!;

        public override Dictionary<string, object> Uniforms => new(base.Uniforms)
        {
            { ShadowMapSpaceMatrixUniform, LightSpaceMatrix },
            { ShadowPcfRadiusUniform, FeatherRadius },
        };

        protected override Vector2i Size => Texture.Size;

        internal Vector3 Direction
        {
            set
            {
                direction = value;
                UpdateLightSpaceMatrix();
            }
        }

        public float Distance
        {
            get => distance;
            set
            {
                distance = value;
                UpdateLightSpaceMatrix();
            }
        }

        public Vector2 PlaneSize
        {
            get => planeSize;
            set
            {
                planeSize = value;
                UpdateLightSpaceMatrix();
            }
        }

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

        public Vector3 Offset
        {
            get => offset;
            set
            {
                offset = value;
                UpdateLightSpaceMatrix();
            }
        }

        public int FeatherRadius { get; set; }

        public override void Bind()
        {
            base.Bind();
            Shader.SetUniform(LightSpaceMatrixUniform, LightSpaceMatrix);
        }

        private Matrix4 LightSpaceMatrix { get; set; }

        private void UpdateLightSpaceMatrix()
        {
            LightSpaceMatrix = GetViewMatrix() * GetProjectionMatrix();
        }

        private Matrix4 GetViewMatrix()
        {
            var front = Vector3.Normalize(direction);
            var right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            var up = Vector3.Normalize(Vector3.Cross(right, front));

            return Matrix4.LookAt(-front * Distance + Offset, Offset, up);
        }

        private Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreateOrthographic(PlaneSize.X, PlaneSize.Y, NearPlane, FarPlane);
        }
    }
}
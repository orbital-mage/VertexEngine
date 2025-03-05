using OpenTK.Mathematics;

namespace VertexEngine.Graphics3D.Assets.Cameras
{
    public class OrthographicCamera : Camera3D
    {
        private Vector3 position;
        private Vector3 front = -Vector3.UnitZ;
        private Vector3 up = Vector3.UnitY;
        private Vector3 right = Vector3.UnitX;
        private Vector2 size;
        private float pitch;
        private float yaw = -MathHelper.PiOver2;
        private float depthNear = 0.01f;
        private float depthFar = 100f;

        public OrthographicCamera(Vector3 position, Vector2 size)
        {
            this.position = position;
            this.size = size;

            UpdateUniforms();
        }

        public Vector3 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateUniforms();
            }
        }

        public Vector3 Front
        {
            get => front;
            set
            {
                front = value;
                UpdateUniforms();
            }
        }

        public Vector3 Up
        {
            get => up;
            set
            {
                up = value;
                UpdateUniforms();
            }
        }

        public Vector2 Size
        {
            private get => size;
            set
            {
                size = value;
                UpdateUniforms();
            }
        }

        public float Pitch
        {
            get => MathHelper.RadiansToDegrees(pitch);
            set
            {
                pitch = MathHelper.DegreesToRadians(
                    MathHelper.Clamp(value, -89f, 89f));
                UpdateVectors();
            }
        }

        public float Yaw
        {
            get => MathHelper.RadiansToDegrees(yaw);
            set
            {
                yaw = MathHelper.DegreesToRadians(value);
                UpdateVectors();
            }
        }

        public float DepthNear
        {
            get => depthNear;
            set
            {
                depthNear = value;
                UpdateUniforms();
            }
        }

        public float DepthFar
        {
            get => depthFar;
            set
            {
                depthFar = value;
                UpdateUniforms();
            }
        }

        private void UpdateVectors()
        {
            front.X = MathF.Cos(pitch) * MathF.Cos(yaw);
            front.Y = MathF.Sin(pitch);
            front.Z = MathF.Cos(pitch) * MathF.Sin(yaw);

            front = Vector3.Normalize(front);

            right = Vector3.Normalize(Vector3.Cross(front, Vector3.UnitY));
            up = Vector3.Normalize(Vector3.Cross(right, front));

            UpdateUniforms();
        }


        private void UpdateUniforms()
        {
            ViewMatrix = GetViewMatrix();
            ProjectionMatrix = GetProjectionMatrix();
            ViewPosition = Position;
        }

        private Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        private Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreateOrthographic(size.X, size.Y, depthNear, depthFar);
        }
    }
}
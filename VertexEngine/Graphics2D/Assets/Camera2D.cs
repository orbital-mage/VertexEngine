using OpenTK.Mathematics;
using VertexEngine.Common.Assets;

namespace VertexEngine.Graphics2D.Assets
{
    public class Camera2D : Camera
    {
        public Vector2 Position
        {
            get => -ViewMatrix.ExtractTranslation().Xy;
            set => UpdateView(-value, Distance, Rotation);
        }

        public float Distance
        {
            get => ViewMatrix.ExtractTranslation().Z;
            set => UpdateView(Position, MathHelper.Max(value, 0), Rotation);
        }

        public float Rotation
        {
            get => MathHelper.RadiansToDegrees(ViewMatrix.ExtractRotation().Normalized().ToEulerAngles().Z);
            set => UpdateView(Position, Distance, MathHelper.DegreesToRadians(value));
        }

        private void UpdateView(Vector2 position, float distance, float rotation)
        {
            ViewPosition = new Vector3(position.X, position.Y, distance);
            ViewMatrix = Matrix4.CreateFromAxisAngle(Vector3.UnitZ, rotation) *
                         Matrix4.CreateTranslation(ViewPosition);
        }
    }
}
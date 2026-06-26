using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Exceptions;
using VertexEngine.Common.Rendering;

namespace VertexEngine.Graphics2D.Assets
{
    public class Transform2D : Transform
    {
        private Vector2i? containerSize;
        private Vector2i? contentSize;

        private Vector2 AspectRatio => ContainerSize.ToVector2().Normalized().Yx;
        private Matrix4 ScreenMatrix => Matrix4.CreateScale(new Vector3(AspectRatio.X, AspectRatio.Y, 1));

        private Matrix4 NormalizedMatrix => Matrix * ScreenMatrix.Inverted();

        public Vector2 Translation
        {
            get => NormalizedMatrix.ExtractTranslation().Xy * AspectRatio;
            set => UpdateMatrix(value, Scale, Rotation);
        }

        public Vector2 Scale
        {
            get => NormalizedMatrix.ExtractScale().Xy * AspectRatio;
            set => UpdateMatrix(Translation, value, Rotation);
        }

        public float Rotation
        {
            get => MathHelper.RadiansToDegrees(NormalizedMatrix.ExtractRotation().Normalized().ToEulerAngles().Z);
            set => UpdateMatrix(Translation, Scale, MathHelper.DegreesToRadians(value));
        }

        public Vector2i Position
        {
            get
            {
                var position = (Vector2i)((Translation + Vector2.One) / 2 * ContainerSize);
                position.Y = ContainerSize.Y - position.Y;
                return position;
            }
            set
            {
                var translation = new Vector2(value.X, ContainerSize.Y - value.Y);
                Translation = translation / ContainerSize * 2 - Vector2.One;
            }
        }

        public Vector2i Size
        {
            get => (Vector2i)(Scale * ContentSize);
            set => Scale = value.ToVector2() / ContentSize;
        }

        public Vector2i ContainerSize
        {
            get => containerSize ?? Viewport.GetSize(FrameBuffer.Default);
            internal set
            {
                containerSize = value;
                UpdateMatrix(Translation, Scale, Rotation);
            }
        }

        public Vector2i ContentSize
        {
            get => contentSize ?? Viewport.GetSize(FrameBuffer.Default);
            internal set
            {
                if (value.X == 0 || value.Y == 0) throw new ZeroScaleException();

                contentSize = value;
                UpdateMatrix(Translation, Scale, Rotation);
            }
        }

        private void UpdateMatrix(Vector2 translation, Vector2 scale, float rotation)
        {
            if (scale.X == 0 || scale.Y == 0) throw new ZeroScaleException();

            scale /= AspectRatio;
            translation /= AspectRatio;

            var scaleMat = Matrix4.CreateScale(new Vector3(scale.X, scale.Y, 1));
            var rotationMat = Matrix4.CreateFromQuaternion(Quaternion.FromAxisAngle(Vector3.UnitZ, rotation));
            var translationMat = Matrix4.CreateTranslation(new Vector3(translation));

            Matrix = scaleMat *
                     rotationMat *
                     translationMat *
                     ScreenMatrix;
        }
    }
}
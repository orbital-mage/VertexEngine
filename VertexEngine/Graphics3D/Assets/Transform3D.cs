﻿using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Exceptions;

namespace VertexEngine.Graphics3D.Assets
{
    public class Transform3D : Transform
    {
        public Vector3 Position
        {
            get => Matrix.ExtractTranslation();
            set => Matrix = Matrix4.CreateScale(Scale) *
                            Matrix4.CreateFromQuaternion(Rotation) *
                            Matrix4.CreateTranslation(value);
        }

        public Vector3 Scale
        {
            get => Matrix.ExtractScale();
            set
            {
                if (value.X == 0 || value.Y == 0 || value.Z == 0) throw new ZeroScaleException();

                Matrix = Matrix4.CreateScale(value) *
                         Matrix4.CreateFromQuaternion(Rotation) *
                         Matrix4.CreateTranslation(Position);
            }
        }

        public Quaternion Rotation
        {
            get => Matrix.ExtractRotation().Normalized();
            set => Matrix = Matrix4.CreateScale(Scale) *
                            Matrix4.CreateFromQuaternion(value) *
                            Matrix4.CreateTranslation(Position);
        }

        public override string ToString()
        {
            return $"Transform3D{{{Matrix.ToString().Replace("\n", ", ")}}}";
        }
    }
}
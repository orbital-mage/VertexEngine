using OpenTK.Graphics.OpenGL4;

namespace VertexEngine.Common.Assets.Rendering
{
    public record struct FaceCullingOptions
    {
        public static readonly FaceCullingOptions Off = new() { Enabled = false };
        public static readonly FaceCullingOptions On = new() { Enabled = true, CullMode = CullFaceMode.Back };
        public static readonly FaceCullingOptions CullFront = new() { Enabled = true, CullMode = CullFaceMode.Front };

        public bool Enabled { get; set; }
        public CullFaceMode? CullMode { get; set; }
        public FrontFaceDirection? FrontDirection { get; set; }

        public void Use()
        {
            if (Enabled)
            {
                GL.Enable(EnableCap.CullFace);
                if (CullMode.HasValue) GL.CullFace(CullMode.Value);
                if (FrontDirection.HasValue) GL.FrontFace(FrontDirection.Value);
            }
            else
            {
                GL.Disable(EnableCap.CullFace);
            }
        }
    }
}
using OpenTK.Graphics.OpenGL;

namespace VertexEngine.Common.Assets.Rendering
{
    public record struct DepthTestingOptions
    {
        public static readonly DepthTestingOptions On = new() { Enabled = true, MaskEnabled = true };
        public static readonly DepthTestingOptions Off = new() { Enabled = false, MaskEnabled = true };
        public static readonly DepthTestingOptions MaskOff = new() { Enabled = false, MaskEnabled = false };

        public bool Enabled { get; set; }
        public bool MaskEnabled { get; set; }
        public DepthFunction? DepthFunction { get; set; }

        public void Use()
        {
            if (Enabled)
            {
                GL.Enable(EnableCap.DepthTest);
                if (DepthFunction.HasValue) GL.DepthFunc(DepthFunction.Value);
            }
            else
            {
                GL.Disable(EnableCap.DepthTest);
            }

            GL.DepthMask(MaskEnabled);
        }
    }
}
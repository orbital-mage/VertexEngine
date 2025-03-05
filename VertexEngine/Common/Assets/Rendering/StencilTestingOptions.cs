using OpenTK.Graphics.OpenGL;

namespace VertexEngine.Common.Assets.Rendering
{
    public record struct StencilTestingOptions
    {
        public static readonly StencilTestingOptions Off = new() { Enabled = false };

        public bool Enabled { get; set; }
        public StencilFunction? StencilFunction { get; set; }
        public int Ref { get; set; }
        public int RefMask { get; set; }
        public int? Mask { get; set; }

        public void Use()
        {
            if (Enabled)
            {
                GL.Enable(EnableCap.StencilTest);
                if (StencilFunction.HasValue) GL.StencilFunc(StencilFunction.Value, Ref, RefMask);
                if (Mask.HasValue) GL.StencilMask(Mask.Value);
            }
            else
            {
                GL.Disable(EnableCap.StencilTest);
            }
        }
    }
}
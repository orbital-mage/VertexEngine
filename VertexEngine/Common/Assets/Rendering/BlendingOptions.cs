using OpenTK.Graphics.OpenGL;

namespace VertexEngine.Common.Assets.Rendering
{
    public record struct BlendingOptions
    {
        public static readonly BlendingOptions Off = new() { Enabled = false };

        public static readonly BlendingOptions Basic = new()
        {
            Enabled = true,
            SourceFactor = BlendingFactorSrc.SrcAlpha,
            DestinationFactor = BlendingFactorDest.OneMinusSrcAlpha,
            AlphaSourceFactor = BlendingFactorSrc.One,
            AlphaDestinationFactor = BlendingFactorDest.One
        };

        public bool Enabled { get; set; }
        public BlendingFactorSrc? SourceFactor { get; set; }
        public BlendingFactorDest? DestinationFactor { get; set; }
        public BlendingFactorSrc? AlphaSourceFactor { get; set; }
        public BlendingFactorDest? AlphaDestinationFactor { get; set; }

        public void Use()
        {
            if (Enabled)
            {
                GL.Enable(EnableCap.Blend);
                if (SourceFactor.HasValue && DestinationFactor.HasValue)
                    GL.BlendFuncSeparate(
                        SourceFactor.Value,
                        DestinationFactor.Value,
                        AlphaSourceFactor ?? SourceFactor.Value,
                        AlphaDestinationFactor ?? DestinationFactor.Value);
            }
            else
            {
                GL.Disable(EnableCap.Blend);
            }
        }
    }
}
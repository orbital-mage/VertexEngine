namespace VertexEngine.Common.Assets.Rendering
{
    public sealed class RenderOptions
    {
        private DepthTestingOptions depthTestingOptions = DepthTestingOptions.On;
        private StencilTestingOptions stencilTestingOptions = StencilTestingOptions.Off;
        private BlendingOptions blendingOptions = BlendingOptions.Off;
        private FaceCullingOptions faceCullingOptions = FaceCullingOptions.Off;
        public event EventHandler OptionsChanged;

        public RenderOptions()
        {
            UpdateAsset();
        }

        public RenderOptionsAsset Asset { get; private set; }

        public DepthTestingOptions DepthTestingOptions
        {
            get => depthTestingOptions;
            set
            {
                depthTestingOptions = value;
                OnOptionsChanged();
            }
        }

        public StencilTestingOptions StencilTestingOptions
        {
            get => stencilTestingOptions;
            set
            {
                stencilTestingOptions = value;
                OnOptionsChanged();
            }
        }

        public BlendingOptions BlendingOptions
        {
            get => blendingOptions;
            set
            {
                blendingOptions = value;
                OnOptionsChanged();
            }
        }

        public FaceCullingOptions FaceCullingOptions
        {
            get => faceCullingOptions;
            set
            {
                faceCullingOptions = value;
                OnOptionsChanged();
            }
        }

        private void OnOptionsChanged()
        {
            UpdateAsset();
            OptionsChanged?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateAsset()
        {
            Asset = new RenderOptionsAsset(
                DepthTestingOptions,
                StencilTestingOptions,
                BlendingOptions,
                FaceCullingOptions);
        }
    }
}
namespace VertexEngine.Common.Assets.Rendering
{
    public readonly record struct RenderOptionsAsset(
        DepthTestingOptions DepthTestingOptions,
        StencilTestingOptions StencilTestingOptions,
        BlendingOptions BlendingOptions,
        FaceCullingOptions FaceCullingOptions)
        : IAsset
    {
        public void Use()
        {
            DepthTestingOptions.Use();
            StencilTestingOptions.Use();
            BlendingOptions.Use();
            FaceCullingOptions.Use();
        }

        public void Use(Dictionary<Type, IAsset> currentAssets)
        {
            Use();
        }
    }
}
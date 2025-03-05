namespace VertexEngine.Common.Assets.Materials
{
    public class MaterialChangeArgs : EventArgs
    {
        public static readonly MaterialChangeArgs Default = new();
        public static readonly MaterialChangeArgs FootPrintChanged = new() {IsFootPrintChanged = true};
        public static readonly MaterialChangeArgs TexturesUpdated = new() {AreTexturesUpdated = true};

        public static readonly MaterialChangeArgs FullChange = new()
            {IsFootPrintChanged = true, AreTexturesUpdated = true};

        public static MaterialChangeArgs Get(bool isFootPrintChanged, bool areTexturesUpdated)
        {
            return isFootPrintChanged switch
            {
                true when areTexturesUpdated => FullChange,
                true => FootPrintChanged,
                false when areTexturesUpdated => TexturesUpdated,
                _ => Default
            };
        }

        private MaterialChangeArgs()
        {
        }

        public bool IsFootPrintChanged { get; private init; }
        public bool AreTexturesUpdated { get; private init; }
    }
}
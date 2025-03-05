using VertexEngine.Common.Assets.Textures;

namespace VertexEngine.Common.Assets.Materials
{
    public interface IMaterial : IEnumerable<KeyValuePair<string, object>>
    {
        public Dictionary<string, object> Values { get; }
        public IEnumerable<Texture> TextureValues { get; }

        public UniformAsset Asset { get; }
        
        public event EventHandler<MaterialChangeArgs> ValuesChanged;

        public void Add(string name, object value);
        public object this[string name] { get; set; }
    }
}
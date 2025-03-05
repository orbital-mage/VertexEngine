using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Common.Assets.Textures;
using VertexEngine.Common.Elements.Interfaces;

namespace VertexEngine.Common.Elements.Tree
{
    public class MaterialManager : TreeManager<IMaterialElement>
    {
        private IMaterial material = new Material();

        public event EventHandler<MaterialChangeArgs> MaterialChanged;

        public MaterialManager(IMaterialElement element) : base(element)
        {
            Material.ValuesChanged += OnMaterialChanged;
        }

        public IMaterial Material
        {
            get => material;
            set
            {
                material.ValuesChanged -= OnMaterialChanged;
                material = value;
                material.ValuesChanged += OnMaterialChanged;
                OnMaterialChanged(MaterialChangeArgs.FullChange);
            }
        }

        public AssetSet<Texture> Textures { get; } = [];

        public override void Propagate(IElement child)
        {
        }

        public override void DePropagate(IElement child)
        {
        }

        private void OnMaterialChanged(object sender, MaterialChangeArgs args)
        {
            OnMaterialChanged(args);
        }

        private void OnMaterialChanged(MaterialChangeArgs args)
        {
            if (args.AreTexturesUpdated)
            {
                Textures.Clear();
                Textures.AddRange(Material.TextureValues);
            }

            MaterialChanged?.Invoke(this, args);
        }
    }
}
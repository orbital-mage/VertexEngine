using Assimp;
using OpenTK.Mathematics;
using VertexEngine.Common.Assets.Materials;
using VertexEngine.Common.Assets.Textures;
using Material = VertexEngine.Common.Assets.Materials.Material;
using RawMaterial = Assimp.Material;

namespace VertexEngine.Common.Resources.Models
{
    internal static class MaterialParser
    {
        private static readonly HashSet<(string, Func<RawMaterial, bool>, Func<RawMaterial, object>)> ValueGetters =
            new()
            {
                (MaterialUniforms.Ambient3, HasColorAmbient, ColorAmbient),
                (MaterialUniforms.Diffuse3, HasColorDiffuse, ColorDiffuse),
                (MaterialUniforms.Specular3, HasColorSpecular, ColorSpecular),
                (MaterialUniforms.AmbientTexture, HasTextureAmbient, TextureAmbient),
                (MaterialUniforms.DiffuseTexture, HasTextureDiffuse, TextureDiffuse),
                (MaterialUniforms.SpecularTexture, HasTextureSpecular, TextureSpecular),
                (MaterialUniforms.Shininess, HasShininess, Shininess),
            };

        internal static Material[] ParseMaterials(IEnumerable<RawMaterial> rawMaterials)
        {
            var materials = new List<Material>();

            foreach (var material in rawMaterials)
            {
                var materialTk = new Dictionary<string, object>();

                foreach (var (name, exists, getter) in ValueGetters)
                    if (exists.Invoke(material))
                        materialTk[name] = getter.Invoke(material);

                materials.Add(new Material(materialTk));
            }

            return materials.ToArray();
        }

        private static bool HasColorAmbient(RawMaterial material) => material.HasColorAmbient;
        private static bool HasColorDiffuse(RawMaterial material) => material.HasColorDiffuse;
        private static bool HasColorSpecular(RawMaterial material) => material.HasColorSpecular;
        private static bool HasTextureAmbient(RawMaterial material) => material.HasTextureAmbient;
        private static bool HasTextureDiffuse(RawMaterial material) => material.HasTextureDiffuse;
        private static bool HasTextureSpecular(RawMaterial material) => material.HasTextureSpecular;
        private static bool HasShininess(RawMaterial material) => material.HasShininess;

        private static object ColorAmbient(RawMaterial material) => GetColor(material.ColorAmbient);
        private static object ColorDiffuse(RawMaterial material) => GetColor(material.ColorDiffuse);
        private static object ColorSpecular(RawMaterial material) => GetColor(material.ColorSpecular);
        private static object TextureAmbient(RawMaterial material) => GetTexture(material.TextureAmbient);
        private static object TextureDiffuse(RawMaterial material) => GetTexture(material.TextureDiffuse);
        private static object TextureSpecular(RawMaterial material) => GetTexture(material.TextureSpecular);
        private static object Shininess(RawMaterial material) => material.Shininess;

        private static object GetColor(Color4D value)
        {
            return new Vector3(value.R, value.G, value.B);
        }

        private static object GetTexture(TextureSlot value)
        {
            return Texture2D.LoadFromFile(value.FilePath);
        }
    }
}
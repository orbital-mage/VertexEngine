using OpenTK.Mathematics;
using VertexEngine.Common.Assets;
using VertexEngine.Common.Assets.Sets;
using VertexEngine.Graphics3D.Assets.Lights.Shadows;

namespace VertexEngine.Graphics3D.Assets.Lights
{
    public abstract class Light : IIndexedAsset
    {
        private const string AmbientUniform = "ambient";
        private const string DiffuseUniform = "diffuse";
        private const string SpecularUniform = "specular";
        private const string ShadowMapEnableUniform = "shadowMap.enable";
        private const string ShadowMapUniform = "shadowMap.map";

        public object Grouping => PrefixName;

        protected abstract string PrefixName { get; }

        protected virtual Dictionary<string, object> Uniforms =>
            new(ShadowMap?.Uniforms ?? Enumerable.Empty<KeyValuePair<string, object>>())
            {
                { AmbientUniform, Ambient },
                { DiffuseUniform, Diffuse },
                { SpecularUniform, Specular },
                { ShadowMapEnableUniform, ShadowMap != null },
                { ShadowMapUniform, ShadowMap != null ? ShadowMap.Texture : 30 }
            };

        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }

        public ShadowMap? ShadowMap { get; protected init; }

        public virtual void Use(Dictionary<Type, IAsset> assets, int unit)
        {
            UniformUtils.SetIndexedUniforms(assets, PrefixName, unit, Uniforms);
        }

        public int CompareTo(object? obj)
        {
            return GetHashCode().CompareTo(obj?.GetHashCode());
        }
    }
}
namespace VertexEngine.Graphics3D.Elements.Tree
{
    public class LightsChangedArgs(bool texturesChanged)
    {
        public bool TexturesChanged { get; } = texturesChanged;
    }
}
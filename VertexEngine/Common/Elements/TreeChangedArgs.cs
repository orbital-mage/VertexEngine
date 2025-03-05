namespace VertexEngine.Common.Elements
{
    public class TreeChangedArgs : EventArgs
    {
        public HashSet<IElement>? AddedElements { get; init; }
        public HashSet<IElement>? RemovedElements { get; init; }

        public static TreeChangedArgs Added(params IElement[] addedElements)
        {
            return Added(addedElements as IEnumerable<IElement>);
        }

        public static TreeChangedArgs Added(IEnumerable<IElement> addedElements)
        {
            return new TreeChangedArgs
            {
                AddedElements = addedElements
                    .SelectMany(IElement.FlattenTree)
                    .ToHashSet()
            };
        }

        public static TreeChangedArgs Removed(params IElement[] removedElements)
        {
            return Removed(removedElements as IEnumerable<IElement>);
        }

        public static TreeChangedArgs Removed(IEnumerable<IElement> removedElements)
        {
            return new TreeChangedArgs
            {
                RemovedElements = removedElements
                    .SelectMany(IElement.FlattenTree)
                    .ToHashSet()
            };
        }
    }
}
using VertexEngine.Common.Utils;

namespace VertexEngine.Common.Resources
{
    internal static class ShaderAssembler
    {
        private const string IncludeStatement = "#include";
        private const string PartialDefineStatement = "#define {0} ";
        private const string DefineStatement = "#define {0} {1}";

        internal static string LoadShaderSource(string path, IReadOnlyDictionary<string, string> definitions)
        {
            return PreProcess(LoadFromFile(path), definitions);
        }

        private static string PreProcess(string source, IReadOnlyDictionary<string, string> definitions)
        {
            source = ProcessIncludes(source);
            source = ProcessDefinitions(source, definitions);

            return source;
        }

        private static string ProcessIncludes(string source)
        {
            var index = source.IndexOf(IncludeStatement, StringComparison.Ordinal);

            while (index != -1)
            {
                var includeStart = index + IncludeStatement.Length;
                var eol = source.IndexOf(Environment.NewLine, index, StringComparison.Ordinal);
                var fileName = source.Substring(includeStart, eol - includeStart).Trim();

                var includeSrc = LoadFromFile(fileName);

                source = source.Remove(index, eol - index);
                source = source.Insert(index, includeSrc + Environment.NewLine);

                index = source.IndexOf(IncludeStatement, StringComparison.Ordinal);
            }

            return source;
        }

        private static string ProcessDefinitions(string source, IReadOnlyDictionary<string, string> definitions)
        {
            var index = source.IndexOf(Environment.NewLine, StringComparison.Ordinal);

            foreach (var (definition, value) in definitions)
            {
                var existingDefIndex = source.IndexOf(string.Format(PartialDefineStatement, definition),
                    StringComparison.Ordinal);
                if (existingDefIndex != -1)
                {
                    var eolIndex = source.IndexOf(Environment.NewLine, existingDefIndex, StringComparison.Ordinal);
                    source = source.Remove(existingDefIndex, eolIndex - existingDefIndex);
                }

                source = source.Insert(index, string.Format(DefineStatement, definition, value));
                source = source.Insert(index, Environment.NewLine);
            }

            return source;
        }

        private static string LoadFromFile(string file)
        {
            return ResourceUtils.ReadShader(file);
        }
    }
}
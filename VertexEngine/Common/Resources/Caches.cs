using System.Xml.Serialization;

namespace VertexEngine.Common.Resources
{
    public static class Caches
    {
        public static string? ProjectName { get; set; }

        public static string CachePath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @$"VertexEngine\{ProjectName}\Cache");

        static Caches()
        {
            if (!Directory.Exists(CachePath))
                Directory.CreateDirectory(CachePath);
        }

        public static T? TryReadStructFromCache<T>(params string[] properties) where T : struct
        {
            return Exists(properties) ? ReadFromCache<T>(properties) : null;
        }

        public static T? TryReadClassFromCache<T>(params string[] properties) where T : class
        {
            return Exists(properties) ? ReadFromCache<T>(properties) : null;
        }

        public static bool Exists(params string[] properties)
        {
            return File.Exists(GetCacheName(properties));
        }

        public static void WriteToCache<T>(T data, params string[] properties)
        {
            var serializer = new XmlSerializer(typeof(T));
            var file = File.Create(GetCacheName(properties));
            serializer.Serialize(file, data);
            file.Close();
        }

        public static T? ReadFromCache<T>(params string[] properties)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new StreamReader(GetCacheName(properties));
            var result = serializer.Deserialize(reader);
            reader.Close();
            return (T?)result;
        }

        private static string GetCacheName(params string[] properties)
        {
            return Path.Combine(CachePath, $"{string.Join("-", properties).Replace("/", ".")}.xml");
        }
    }
}
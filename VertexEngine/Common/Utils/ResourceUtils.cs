using System.Reflection;
using Assimp;
using Microsoft.Bot.Builder;
using StbImageSharp;
using VertexEngine.Common.Resources.Models;

namespace VertexEngine.Common.Utils;

public static class ResourceUtils
{
    private const string ResourcesPrefix = "VertexEngine.Resources.";
    private const string ShadersPrefix = "VertexEngine.Shaders.";
    private const string ResourceSeparator = ".";

    private static readonly string InternalFilePrefix = PathUtils.NormalizePath("~/");
    private static readonly string FileSeparator = PathUtils.NormalizePath("/");

    private static readonly string WorkingPath = Directory.GetCurrentDirectory();
    private static readonly Assembly EngineAssembly = Assembly.GetExecutingAssembly();

    // TODO: Read Scene from stream throws exception

    public static string ResourcesPath { get; set; } = Path.Combine(WorkingPath, "Resources");
    public static string ShadersPath { get; set; } = Path.Combine(WorkingPath, "Shaders");

    public static string[] GetResources(string directory)
    {
        return ReadResource(directory, GetFiles, GetInternalResources);
    }

    public static Stream GetResourceStream(string file)
    {
        return ReadResource(file, File.OpenRead, GetInternalResourceStream);
    }

    public static string ReadTextResource(string file)
    {
        return ReadResource(file, File.ReadAllText, ReadInternalTextResource);
    }

    public static ImageResult ReadImageResource(string file,
        ColorComponents colorComponents = ColorComponents.RedGreenBlueAlpha)
    {
        return ReadResource(file,
            f => ReadImageFromFile(f, colorComponents),
            f => ReadInternalImageResource(f, colorComponents));
    }

    public static Scene ReadSceneResource(string file, PostProcessSteps postProcessSteps = PostProcessSteps.None)
    {
        return ReadResource(file,
            f => ModelImporter.ReadSceneFromFile(f, postProcessSteps),
            f => ReadInternalSceneResource(f, postProcessSteps));
    }

    public static string ReadShader(string file)
    {
        var normalFile = PathUtils.NormalizePath(file);

        ArgumentNullException.ThrowIfNull(normalFile);

        if (File.Exists(normalFile)) return File.ReadAllText(normalFile);

        return normalFile.StartsWith(InternalFilePrefix)
            ? ReadInternalShader(normalFile)
            : File.ReadAllText(Path.Combine(ShadersPath, normalFile));
    }

    private static T ReadResource<T>(
        string file,
        Func<string, T> getResource,
        Func<string, T> getResourceFromStream)
    {
        var normalFile = PathUtils.NormalizePath(file);

        ArgumentNullException.ThrowIfNull(normalFile);

        if (File.Exists(normalFile)) return getResource.Invoke(normalFile);

        return normalFile.StartsWith(InternalFilePrefix)
            ? getResourceFromStream.Invoke(normalFile)
            : getResource.Invoke(Path.Combine(ResourcesPath, normalFile));
    }

    private static string[] GetFiles(string directory)
    {
        if (!Directory.Exists(directory)) throw new FileNotFoundException();

        return Directory.GetFiles(directory);
    }

    private static string[] GetInternalResources(string directory)
    {
        var resourcePath = InternalFilePathToResourcePath(directory);
        return EngineAssembly.GetManifestResourceNames()
            .Where(resource => resource.StartsWith(resourcePath))
            .Select(ResourcePathToInternalFilePath)
            .ToArray();
    }

    private static ImageResult ReadImageFromFile(string file, ColorComponents colorComponents)
    {
        return ImageResult.FromStream(File.OpenRead(file), colorComponents);
    }

    private static string ReadInternalShader(string file)
    {
        var resource = InternalFilePathToShaderPath(file);

        using var stream = EngineAssembly.GetManifestResourceStream(resource);
        if (stream == null) throw new FileNotFoundException($"Resource {resource} not found");
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    private static string ReadInternalTextResource(string file)
    {
        using var stream = GetInternalResourceStream(file);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }

    private static ImageResult ReadInternalImageResource(string file, ColorComponents colorComponents)
    {
        using var stream = GetInternalResourceStream(file);

        return ImageResult.FromStream(stream, colorComponents);
    }

    private static Scene ReadInternalSceneResource(string file, PostProcessSteps postProcessSteps)
    {
        var fileExtension = Path.GetExtension(file);
        using var stream = GetInternalResourceStream(file);

        return ModelImporter.ReadSceneFromStream(stream, postProcessSteps, fileExtension);
    }

    private static Stream GetInternalResourceStream(string file)
    {
        var resource = InternalFilePathToResourcePath(file);

        var stream = EngineAssembly.GetManifestResourceStream(resource);
        if (stream == null) throw new FileNotFoundException($"Resource {resource} not found");

        return stream;
    }

    private static string InternalFilePathToResourcePath(string file)
    {
        return file.Replace(InternalFilePrefix, ResourcesPrefix).Replace(FileSeparator, ResourceSeparator);
    }

    private static string InternalFilePathToShaderPath(string file)
    {
        return file.Replace(InternalFilePrefix, ShadersPrefix).Replace(FileSeparator, ResourceSeparator);
    }

    private static string ResourcePathToInternalDirectoryPath(string resource)
    {
        return resource
            .Replace(ResourcesPrefix, InternalFilePrefix)
            .Replace(ResourceSeparator, FileSeparator);
    }

    private static string ResourcePathToInternalFilePath(string resource)
    {
        var extensionIndex = resource.LastIndexOf(ResourceSeparator, StringComparison.Ordinal);

        return resource[..extensionIndex]
                   .Replace(ResourcesPrefix, InternalFilePrefix)
                   .Replace(ResourceSeparator, FileSeparator)
               + resource[extensionIndex..];
    }
}
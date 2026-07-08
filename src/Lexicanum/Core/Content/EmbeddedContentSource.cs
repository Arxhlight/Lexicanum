using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Lexicanum.Core.Content;

/// <summary>
/// Reads content packs from JSON files embedded in the application assembly.
/// Deserialization is strict: unknown JSON members and missing required members fail loudly
/// so typos in content files surface at startup instead of as silent defaults.
/// </summary>
public sealed class EmbeddedContentSource : IContentSource
{
    private const string ResourcePrefix = "Lexicanum.Content.";

    private static readonly JsonSerializerOptions StrictOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow,
        ReadCommentHandling = JsonCommentHandling.Skip
    };

    private readonly Assembly _assembly = typeof(EmbeddedContentSource).Assembly;

    public IReadOnlyList<ContentPack<T>> LoadPacks<T>(string area)
    {
        var areaPrefix = $"{ResourcePrefix}{area}.";

        var resourceNames = _assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith(areaPrefix, StringComparison.Ordinal)
                        && name.EndsWith(".json", StringComparison.Ordinal))
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToList();

        var packs = new List<ContentPack<T>>();

        foreach (var resourceName in resourceNames)
        {
            using var stream = _assembly.GetManifestResourceStream(resourceName)
                ?? throw new ContentValidationException(resourceName, "resource stream could not be opened");

            try
            {
                var pack = JsonSerializer.Deserialize<ContentPack<T>>(stream, StrictOptions)
                    ?? throw new ContentValidationException(resourceName, "file deserialized to null");
                packs.Add(pack);
            }
            catch (JsonException ex)
            {
                throw new ContentValidationException(resourceName, ex.Message);
            }
        }

        return packs;
    }
}

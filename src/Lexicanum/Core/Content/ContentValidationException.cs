namespace Lexicanum.Core.Content;

/// <summary>
/// A content file is malformed or violates the content contract.
/// Carries the resource or pack name so the offending file is identifiable.
/// </summary>
public sealed class ContentValidationException : Exception
{
    public string ResourceName { get; }

    public ContentValidationException(string resourceName, string reason)
        : base($"Invalid content in '{resourceName}': {reason}")
    {
        ResourceName = resourceName;
    }
}

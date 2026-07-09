namespace Lexicanum.Tests;

/// <summary>
/// A unique directory under the system temp path, deleted on dispose,
/// for tests that exercise real file persistence.
/// </summary>
public sealed class TempDirectory : IDisposable
{
    public string Path { get; } =
        System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"lexicanum-tests-{Guid.NewGuid():N}");

    public string FilePath(params string[] segments)
    {
        return System.IO.Path.Combine(new[] { Path }.Concat(segments).ToArray());
    }

    public void Dispose()
    {
        if (Directory.Exists(Path))
        {
            Directory.Delete(Path, recursive: true);
        }
    }
}

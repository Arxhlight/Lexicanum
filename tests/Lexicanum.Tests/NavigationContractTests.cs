using Lexicanum.Core.Content;
using Lexicanum.Core.Scoring;
using Spectre.Console.Testing;

namespace Lexicanum.Tests;

/// <summary>
/// The whole-app lifecycle contract: the real composition (real content, real score
/// store in a temp directory) driven end-to-end through a TestConsole — welcome,
/// open a category, back out, exit, and the session is persisted.
/// </summary>
public sealed class NavigationContractTests : IDisposable
{
    private readonly string _tempDirectory;
    private readonly string _scoreFilePath;

    public NavigationContractTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), $"lexicanum-tests-{Guid.NewGuid():N}");
        _scoreFilePath = Path.Combine(_tempDirectory, "highscores.json");
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            Directory.Delete(_tempDirectory, recursive: true);
        }
    }

    [Fact]
    public void FullSession_WelcomeNavigateBackExit_SavesScoreAndShowsSummary()
    {
        var console = new TestConsole().Interactive();
        var content = ApplicationContent.LoadAndValidate(new EmbeddedContentSource());
        var scoreService = new ScoreService(new JsonScoreStore(_scoreFilePath));
        var app = new LexicanumApp(console, content, scoreService);

        console.Input.PushTextWithEnter("TestUser");
        console.Input.PushKey(ConsoleKey.Enter);

        console.Input.PushKey(ConsoleKey.Enter);

        var lexiconChildCount = content.LexiconPacks.Count;
        for (int i = 0; i < lexiconChildCount; i++)
        {
            console.Input.PushKey(ConsoleKey.DownArrow);
        }
        console.Input.PushKey(ConsoleKey.Enter);

        for (int i = 0; i < 4; i++)
        {
            console.Input.PushKey(ConsoleKey.DownArrow);
        }
        console.Input.PushKey(ConsoleKey.Enter);

        console.Input.PushKey(ConsoleKey.Enter);

        app.Run();

        Assert.Contains("TestUser", console.Output);
        Assert.Contains("Lexicon", console.Output);
        Assert.Contains("Session Summary", console.Output);

        var saved = new JsonScoreStore(_scoreFilePath).Load();
        Assert.False(saved.LoadFailed);
        var savedScore = Assert.Single(saved.Scores);
        Assert.Equal("TestUser", savedScore.PlayerName);
    }
}

using Spectre.Console;

namespace Lexicanum.Navigation;

/// <summary>
/// One screen of the application. <see cref="Run"/> owns the screen's state, renders it,
/// and blocks for input; state changes only happen inside this call (MVU-style).
/// </summary>
public interface IScreen
{
    string Title { get; }
    ScreenResult Run(IAnsiConsole console);
}

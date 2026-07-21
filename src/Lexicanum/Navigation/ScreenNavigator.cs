using Spectre.Console;

namespace Lexicanum.Navigation;

/// <summary>
/// The single loop that owns the terminal: runs the top screen of an explicit
/// navigation stack and applies its result until the stack empties or a screen exits.
/// </summary>
public sealed class ScreenNavigator
{
    private readonly IAnsiConsole _console;
    private readonly Stack<IScreen> _screens = new();

    public ScreenNavigator(IAnsiConsole console)
    {
        _console = console;
    }

    public void Run(IScreen rootScreen)
    {
        _screens.Push(rootScreen);

        while (_screens.Count > 0)
        {
            var result = _screens.Peek().Run(_console);

            switch (result.Action)
            {
                case ScreenAction.Push:
                    _screens.Push(result.NextScreen!);
                    break;
                case ScreenAction.Pop:
                    _screens.Pop();
                    break;
                case ScreenAction.Exit:
                    return;
            }
        }
    }
}

namespace Lexicanum.Navigation;

/// <summary>
/// What the navigator should do after a screen finishes: push a new screen,
/// pop back to the previous one, or exit the application.
/// </summary>
public sealed class ScreenResult
{
    public static readonly ScreenResult Pop = new(ScreenAction.Pop, null);
    public static readonly ScreenResult Exit = new(ScreenAction.Exit, null);

    public ScreenAction Action { get; }
    public IScreen? NextScreen { get; }

    private ScreenResult(ScreenAction action, IScreen? nextScreen)
    {
        Action = action;
        NextScreen = nextScreen;
    }

    public static ScreenResult Push(IScreen screen)
    {
        return new ScreenResult(ScreenAction.Push, screen);
    }
}

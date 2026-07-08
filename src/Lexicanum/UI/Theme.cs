using Spectre.Console;

namespace Lexicanum.UI;

/// <summary>
/// The single source of truth for every color and style in the application.
/// Widgets take the <see cref="Style"/> fields; inline markup uses the string constants.
/// </summary>
public static class Theme
{
    public const string Accent = "cyan1";
    public const string Narrator = "yellow";
    public const string Success = "green";
    public const string Error = "red";
    public const string Muted = "grey";
    public const string Score = "yellow";
    public const string Body = "white";
    public const string CodeBlock = "yellow";
    public const string WrongAnswer = "red";
    public const string CorrectAnswer = "green";
    public const string RankGold = "gold1";
    public const string RankSilver = "grey70";
    public const string RankBronze = "darkorange3";

    public static readonly Style AccentStyle = Style.Parse(Accent);
    public static readonly Style NarratorStyle = Style.Parse(Narrator);
    public static readonly Style MutedStyle = Style.Parse(Muted);
    public static readonly Style BodyStyle = Style.Parse(Body);
    public static readonly Style CodeBlockStyle = Style.Parse(CodeBlock);
    public static readonly Style WrongAnswerStyle = Style.Parse(WrongAnswer);
    public static readonly Style CorrectAnswerStyle = Style.Parse(CorrectAnswer);
}

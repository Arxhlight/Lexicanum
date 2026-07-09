using Spectre.Console;

namespace Lexicanum.UI;

/// <summary>
/// The single source of truth for every color and style in the application.
/// Widgets take the <see cref="Style"/> fields; inline markup uses the string constants.
/// Code always picks the token that matches the text's purpose (see the Theme Token
/// Rule in CLAUDE.md); the colors behind the tokens are tweakable here alone.
/// </summary>
public static class Theme
{
    /// <summary>App identity: screen headers, menu highlight, and the welcome greeting.</summary>
    public const string Accent = "cyan1";

    /// <summary>Informative text: explanations, hints, totals, instructions.</summary>
    public const string Info = "cadetblue";

    /// <summary>Moxy's voice: narrator lines and box borders.</summary>
    public const string Narrator = "yellow";

    /// <summary>Everything the player types.</summary>
    public const string Input = "deepskyblue1";

    /// <summary>The running score and score totals.</summary>
    public const string Score = "gold1";

    /// <summary>Code lines inside lexicon pages.</summary>
    public const string CodeBlock = "khaki1";

    /// <summary>Positive outcomes: correct-answer confirmations.</summary>
    public const string Success = "green";

    /// <summary>Negative outcomes: wrong-answer and failure messages.</summary>
    public const string Error = "red";

    /// <summary>Primary reading text: questions, descriptions, page bodies.</summary>
    public const string Body = "white";

    /// <summary>De-emphasized chrome: key hints, prompt titles, borders, plain lexicon text.</summary>
    public const string Muted = "grey";

    /// <summary>The player's incorrect code in comparisons.</summary>
    public const string WrongAnswer = "red";

    /// <summary>The model answer in comparisons.</summary>
    public const string CorrectAnswer = "green";

    public const string RankGold = "gold1";
    public const string RankSilver = "grey70";
    public const string RankBronze = "darkorange3";

    public static readonly Style AccentStyle = Style.Parse(Accent);
    public static readonly Style InfoStyle = Style.Parse(Info);
    public static readonly Style NarratorStyle = Style.Parse(Narrator);
    public static readonly Style InputStyle = Style.Parse(Input);
    public static readonly Style MutedStyle = Style.Parse(Muted);
    public static readonly Style BodyStyle = Style.Parse(Body);
    public static readonly Style CodeBlockStyle = Style.Parse(CodeBlock);
    public static readonly Style WrongAnswerStyle = Style.Parse(WrongAnswer);
    public static readonly Style CorrectAnswerStyle = Style.Parse(CorrectAnswer);
}

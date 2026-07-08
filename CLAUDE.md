# Lexicanum Rules

Enforced conventions for this repository. Each rule names its **enforcement mechanism** and its **source**.
Architecture principles and the layer map live in [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) — read both before building anything new.

## Build & Analysis

- **Warnings are errors.** `TreatWarningsAsErrors`, `AnalysisLevel=latest-recommended`, and `EnforceCodeStyleInBuild` are set in `Directory.Build.props` and apply to every project. A build with any warning fails locally and in CI.
  *Enforced by:* the build itself. *Source:* [MS Learn — Code analysis in .NET](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview), [compiler options](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-options/errors-warnings).
- **Formatting is `.editorconfig`-defined and gated.** Run `dotnet format Lexicanum.sln` before committing; CI runs `dotnet format --verify-no-changes` and fails unformatted PRs. This is this repo's prettier.
  *Enforced by:* CI (`ci.yml`). *Source:* [MS Learn — enforce dotnet format in GitHub Actions](https://learn.microsoft.com/en-us/community/content/how-to-enforce-dotnet-format-using-editorconfig-github-actions).
- **Analyzer suppressions must be scoped and justified.** A rule may only be disabled in `.editorconfig` for a specific path, with a comment explaining why (see the `tests/**.cs` section).

## Style & Naming

- File-scoped namespaces; `using` directives outside the namespace; namespace matches folder (IDE0130); one type per file, file named after the type (team convention — Microsoft mandates only one declaration per line).
  *Enforced by:* `.editorconfig` + build. *Source:* [MS C# Coding Conventions](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions), [IDE0130](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/style-rules/ide0130).
- PascalCase types/members, `I`-prefixed interfaces, `_camelCase` private instance fields, PascalCase `const` and `private static readonly` fields, no public instance fields.
  *Enforced by:* `.editorconfig` naming rules. *Source:* [MS identifier names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names), [Framework Design Guidelines — field design](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/field).
- **Naming Rule:** a name must say exactly what the thing is or does. No abbreviations, no vague `Helper`/`Manager` unless the type truly manages something. Rename on sight when a better name exists.
  *Enforced by:* review. *Source:* [FDG — General Naming Conventions](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/general-naming-conventions) ("favor readability over brevity").
- Static classes only for stateless constants and pure helpers (`Theme`, `ScoringRules`, `ConsoleViews`).
  *Source:* [FDG — Static Class Design](https://learn.microsoft.com/en-us/dotnet/standard/design-guidelines/static-class).

## Comment Rule (the hardest rule)

- **No `//` comments.** Code must be clean and self-explanatory; if a line needs a comment, rename or extract until it doesn't (e.g. `ShuffleUntilOrderChanges` instead of a comment about re-shuffling).
- The only permitted comments are XML `///` docs, and only where they carry information the signature can't: the purpose of a public type, a non-obvious invariant, `<exception>` behavior, a genuine gotcha in `<remarks>`. Never restate the member name.
  *Enforced by:* review. *Source:* [MS XML documentation — recommended tags](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/xmldoc/recommended-tags).

## Types & Data

- Immutable data uses `record` types with `required` members (`MenuNode`, content records).
  *Source:* [MS Learn — records](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record).
- **No magic numbers or strings.** Scoring constants live in `Core/Scoring/ScoringRules.cs`, feature keys in `FeatureIds.cs`, every color/style in `UI/Theme.cs`. A literal used twice is a constant.
  *Source:* FDG; mirrors the JS project's theme-token rule.
- **Data-oriented:** behavior that varies by content is data, not code branches. Adding a lexicon entry, quiz, or exercise means adding JSON — never a class or an `if`.

## Errors

- Catch **specific** exceptions only, and only where you can act on them (`IOException`/`UnauthorizedAccessException` around the score save, `JsonException` in stores). Never a bare `catch`, never swallow silently — degraded behavior must be reported (`ScoreboardLoadResult.LoadFailed`).
  *Source:* [MS — Best practices for exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/best-practices-for-exceptions).
- **Fail fast on invalid content:** startup validates all embedded JSON and exits 2 with the offending resource name before rendering anything.
  *Source:* same, plus [clig.dev](https://clig.dev/) (non-zero exit codes).

## Console / TUI

- **All terminal I/O goes through an injected `IAnsiConsole`.** The static `AnsiConsole` is banned everywhere except `Program.cs`. Shared rendering lives in `UI/ConsoleViews.cs`/`UI/ScoreViews.cs` as extensions over the interface.
  *Enforced by:* review (grep for `AnsiConsole.`). *Source:* [Spectre.Console — testing console output](https://spectreconsole.net/console/how-to/testing-console-output) ("accept `IAnsiConsole` as a parameter").
- All styling via `Theme` — no inline color names in feature code.
- One loop owns the terminal: `ScreenNavigator` over an explicit screen stack. No nested `while` menu loops, no recursion for navigation. State changes only inside a screen's `Run` (MVU-style).
  *Source:* Bubble Tea's Elm architecture ([tutorial](https://github.com/charmbracelet/bubbletea/blob/main/tutorials/basics/README.md)), Terminal.Gui's Application loop.
- CLI etiquette: `Main` returns an exit code (0 success, 1 not-a-terminal, 2 invalid content); no prompts when stdin is redirected; Ctrl+C saves the session and exits 0. Spectre handles `NO_COLOR` and capability detection — never override it.
  *Source:* [clig.dev](https://clig.dev/), [MS `Console.CancelKeyPress`](https://learn.microsoft.com/en-us/dotnet/api/system.console.cancelkeypress).

## Testing Rule

- **Contract suites only — one per major system**, in `tests/Lexicanum.Tests`: `ContentContractTests` (all embedded JSON parses strictly and validates), `PersistenceContractTests` (score store round-trip and failure modes), `NavigationContractTests` (whole-app session via `TestConsole`).
- Tests run against **real data** (the actual embedded resources) — no mocks or stubs of our own code, no testing third-party code, no per-method unit tests for game mechanics.
- New content areas are covered by `ContentContractTests` automatically. Do not add tests that pin old behavior: this is not a live product, the new design is law.
  *Source:* [MS — unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices) (avoid infrastructure dependencies; test names `Method_Scenario_ExpectedBehavior`); mirrors the JS project's Testing Rule.

## Data Rule

- App content = embedded JSON under `src/Lexicanum/Content/`, validated at startup. Loose files are not bundled into a single-file exe, which is why content is embedded.
  *Source:* [MS — single-file deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/single-file/overview).
- User data (highscores) lives in `%APPDATA%\Lexicanum\` — never in the repo, never next to the exe, never CWD-relative.
  *Source:* [MS `Environment.GetFolderPath`](https://learn.microsoft.com/en-us/dotnet/api/system.environment.getfolderpath).

## Quality Loop Rule

After each plan execution, before commit/PR:

1. Run `/simplify` and `/code-review`; apply the fixes.
2. Run `dotnet format Lexicanum.sln` and `dotnet test Lexicanum.sln`.
3. At session wrap-up, run the pr-loop.

(Mirrors the JS project's Rule 10; `dotnet format` is this repo's prettier, the analyzer-enforced build is its ESLint.)

## Git & CI

- Branches: `feature/<signature>/<topic>` or `bugfix/...`; PRs target `develop`; `main` receives only completed features via PR.
- `ci.yml` gates every PR on format + build (analyzers) + tests. `release-build.yml` publishes the single-file `Lexicanum.exe` artifact on every push to `main`.
- Never add `continue-on-error` to a test step.

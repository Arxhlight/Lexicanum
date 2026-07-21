# Lexicanum Architecture

Principles and the layer map. The enforced conventions live in [CLAUDE.md](../CLAUDE.md) — read both.

## Principles

- **One loop owns the terminal** — navigation is an explicit screen stack, never nested menu loops or recursion (Elm/MVU as popularized by Bubble Tea; Terminal.Gui's Application loop).
- **Rendering is injected** — every class that draws takes `IAnsiConsole`; nothing touches the static console except `Program.cs`. That one seam is what makes the whole UI testable with `TestConsole`.
- **Content is data** — reference pages, quizzes, and exercises are JSON validated at startup. Code loops over content; it never branches on it.
- **Dependencies point inward** — `Core/` references nothing above it; UI objects never appear in Core signatures (MS Learn, "Common web application architectures").
- **Explicit dependencies** — constructor injection everywhere; the composition root is the only place objects are wired together (MS DI guidelines). No DI container: ~10 singletons with one lifetime make a container pure ceremony.

## Repository Layout

```
CLAUDE.md                  Enforced rules (each with source)
Directory.Build.props      Shared build/analyzer configuration
.editorconfig              Style and naming rules
docs/ARCHITECTURE.md       This file
src/Lexicanum/
├── Program.cs             Entry point: content load, exit codes, Ctrl+C hookup
├── LexicanumApp.cs        Composition root: wiring + lifecycle
├── Content/               Embedded JSON: lexicon/, quizzes/, exercises/
├── Core/                  Zero UI references
│   ├── Content/           Content records, ApplicationContent,
│   │                      EmbeddedContentSource, ContentValidator
│   └── Scoring/           PlayerScore, ScoreService, IScoreStore/JsonScoreStore,
│                          ScoringRules, FeatureIds
├── Navigation/            IScreen, ScreenResult, ScreenNavigator, MenuNode, MenuScreen
├── Features/              One folder per feature: its screen + its logic
│   ├── Welcome/           WelcomeScreen
│   ├── Quizlet/           QuizScreen, ShuffledQuestion
│   ├── CodeTrainer/       CodeTrainerScreen, CodeAnswerValidator
│   ├── Lexicon/           LexiconMenu, LexiconEntryScreen
│   └── Scoreboard/        ScoreboardScreen
└── UI/                    Theme (ALL colors/styles), ConsoleViews, ScoreViews
tests/Lexicanum.Tests/     Three contract suites (content, persistence, navigation)
.github/workflows/         ci.yml (PR gate), release-build.yml (main → exe artifact)
```

**Placement rule:** where code lives is decided by *what it is* —
pure logic/data → `Core/`; rendering → `UI/` over `IAnsiConsole`; a feature's screen + logic → `Features/<Name>/`; machinery shared by all features → `Navigation/`; wiring → `Program.cs`; app content → `Content/*.json`; user data → `%APPDATA%\Lexicanum\`. Namespace always matches folder.

## The Event Loop & Screen Stack

`ScreenNavigator` holds a `Stack<IScreen>` and runs one loop: render the top screen, block for its input, apply its `ScreenResult` (`Push` a new screen, `Pop` back, or `Exit`). A screen's `Run(IAnsiConsole)` owns that screen's state — state changes happen nowhere else.

`MenuScreen` is the only menu implementation: it renders any `MenuNode` (an immutable record tree) as an arrow-key `SelectionPrompt`. Branch children push another `MenuScreen`; leaf children push the screen their factory creates. There is no per-menu code.

Why not the old recursive walker: recursion entangles navigation depth with the call stack, makes "exit from anywhere" impossible without unwinding, and hides where state lives. A stack in data is inspectable and testable.

## Data-Driven Content

- `Content/{lexicon,quizzes,exercises}/*.json` are embedded resources (a single-file exe does not carry loose files).
- Each file is a `ContentPack<T>`: `category`, `description`, `items`. Packs become menu branches; a single-item lexicon pack becomes a direct leaf.
- `EmbeddedContentSource` deserializes strictly (unknown members rejected, `required` members enforced); `ContentValidator` checks ids, indexes, and regexes at startup and exits 2 naming the broken file.
- **Adding content = dropping a JSON file.** The menu picks it up, and `ContentContractTests` covers it automatically. No recompile of logic, no new classes.

## Persistence

`IScoreStore` → `JsonScoreStore(filePath)`: path injected (tests use temp dirs; the app uses `%APPDATA%\Lexicanum\highscores.json`). Writes go through a temp file + atomic move so a crash never corrupts the scoreboard. Reads report failure honestly (`ScoreboardLoadResult.LoadFailed`) instead of swallowing.

## Application Lifecycle

`Program.Main`: load + validate content (exit 2 on failure) → refuse redirected stdin (exit 1) → wire `LexicanumApp` → register Ctrl+C handler (save once if there is a score, exit 0) → run. `LexicanumApp.Run`: welcome → build the root `MenuNode` from content → navigator loop → save score (once per session) → farewell summary.

## Testing Strategy

Three contract suites, real data, no mocks (see CLAUDE.md Testing Rule):

| Suite | Contract |
|---|---|
| `ContentContractTests` | Every embedded content file parses strictly, validates, and each exercise's own example passes its own pattern |
| `PersistenceContractTests` | Score round-trip; missing/corrupt/locked files degrade honestly |
| `NavigationContractTests` | A whole session through `TestConsole`: welcome → navigate → back → exit → score persisted |

## How to Add a Feature

1. Create `Features/<Name>/` with a `<Name>Screen : IScreen` (and a `CreateMenuNode(...)` factory if it has menu entries).
2. If it has content, add `Content/<area>/*.json` + a record in `Core/Content/` + a `ContentValidator` case + a `LoadPacks` call in `ApplicationContent`.
3. Wire its `MenuNode` into the root menu in `Program.cs`.
4. If it scores, add its key to `FeatureIds` and constants to `ScoringRules`.
5. Colors only via `Theme`; rendering only via the injected `IAnsiConsole`.
6. Run the Quality Loop (CLAUDE.md).

# Lexicanum

## Overview
Lexicanum is an interactive, gamified console application designed to help developers learn and practice programming
concepts. With a sassy narrator guiding you through various learning modules, you'll earn points while mastering Git
commands, terminal operations, programming fundamentals, and more.

## Features

### 🎮 Gamified Learning
- **Moxy Narrator** - Sarcastic commentary to keep you motivated
- **Hardmode** - One mistake and you're out (for the brave)

- **Global Score System** - Earn points across all activities
- 
- **Leaderboard** - Compete with yourself and your alter egos
- **Session Tracking** - See your progress breakdown by feature
- **Persistent High Scores** - Your achievements are saved

### 📚 Learning Modules

| Module | Description                                                                |
|--------|----------------------------------------------------------------------------|
| **Lexicon** | Reference guides for Git commands, terminal operations, and more           |
| **Quizlet** | Multiple-choice quizzes with randomized questions and answers              |
| **Live Code Training** | Practice writing code with instant validation and without IDE autocomplete |
| **Scoreboard** | View leaderboards, session scores, and personal history                    |


## Getting Started

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download) or later

### Installation

```bash
# Clone the repository
git clone https://github.com/Arxhlight/Lexicanum.git

# Navigate to the project
cd Lexicanum

# Build the solution
dotnet build

# Run the tests
dotnet test

# Run the application
dotnet run --project src/Lexicanum
```

## Usage

1. **Start the application** - You'll be greeted by the Lexicanum narrator
2. **Enter your name** - Your scores will be tracked
3. **Choose a module** from the main menu:
   - `Lexicon` - Browse reference material
   - `Quizlet` - Take quizzes to earn points
   - `Live Code Training` - Practice writing code
   - `Scoreboard` - Check your scores and rankings
4. **Earn points** - Correct answers add to your global score
5. **Exit** - Your session score is saved to the leaderboard

## Development

Read [CLAUDE.md](CLAUDE.md) (enforced rules, each with its source) and
[docs/ARCHITECTURE.md](docs/ARCHITECTURE.md) (principles, layer map, how to add a feature) before contributing.

### Contributing
>- Contributors must create new branch on `develop`.
>- PRs should be made against the `develop` branch.
>- Branches should be named using the following format:  `feature/` or `bugfix/`.
>- Example: `feature/yoursignature/explanation` as the branch name.

Every PR is gated by CI: `dotnet format --verify-no-changes`, an analyzer-enforced build with
warnings as errors, and the contract test suites. Merges to `main` automatically publish the
single-file `Lexicanum.exe` artifact.

Contributions are welcome! Feel free to:
- Add new quiz questions
- Create new lexicon entries
- Add support for new programming languages in Code Trainer
- Improve the UI/UX

### Adding New Content

Content is data: add a JSON file under `src/Lexicanum/Content/{lexicon,quizzes,exercises}/`.
It is embedded into the executable, validated at startup, appears in the menu automatically,
and is covered by `ContentContractTests` without writing any code.


## License

This project is licensed under the MIT License - see the [LICENSE] file for details.

---

*"Ah, another brave soul enters the Lexicanum..."*

© Arxh 2026

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
git clone https://github.com/yourusername/Lexicanum.git

# Navigate to the project
cd Lexicanum

# Build the project
dotnet build

# Run the application
dotnet run --project Lexicanum
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

### Contributing
>- Contributors must create new branch on `develop`.
>- PRs should be made against the `develop` branch.
>- Branches should be named using the following format:  `feature/` or `bugfix/`.
>- Example: `feature/yoursignature/explanation` as the branch name.
>- any branch not using the `feature/` or `bugfix/` prefix will be rejected by the CI pipeline.

Contributions are welcome! Feel free to:
- Add new quiz questions
- Create new lexicon entries
- Add support for new programming languages in Code Trainer
- Improve the UI/UX

### Adding New Content

To add a new quiz or lexicon entry, simply modify the relevant feature file in `Features/`. The architecture supports:
- Nested subcategories (unlimited depth)
- Custom execution handlers
- Multiple content types


## License

This project is licensed under the MIT License - see the [LICENSE] file for details.

---

*"Ah, another brave soul enters the Lexicanum..."*

© Arxh 2026

---

# Lexicanum Design Principles

## Architecture Goals

- **Decoupled**: Categories, SubCategories, and Content are separate concerns
- **Easy to extend**: Just add new categories in ContentRepository
- **Easy to remove**: Use registry.UnregisterCategory("name")
- **Clear namespaces**: Each feature has its own namespace
- **Single Responsibility**: Each class does one thing
- **Open/Closed Principle**: Open for extension, closed for modification
- **Interface-based**: Easy to mock and test

## Folder Structure

```
Lexicanum/
├── Program.cs (Entry point)
├── Namespaces/
│   ├── Startup/
│   │   ├── WelcomeScreen.cs
│   │   ├── MenuSystem.cs
│   │   └── NavigationManager.cs
│   ├── CodeTrainer/
│   │   ├── LiveCodeSession.cs
│   │   └── CodeExercise.cs
│   ├── Lexicon/
│   │   ├── LexiconViewer.cs
│   │   └── LexiconEntry.cs
│   └── Quizlet/
│       ├── QuizSession.cs
│       └── QuizQuestion.cs
├── Core/
│   ├── Models/
│   │   ├── Category.cs
│   │   ├── SubCategory.cs
│   │   ├── ContentItem.cs
│   │   └── MenuItem.cs
│   ├── Interfaces/
│   │   ├── ICategory.cs
│   │   ├── ISubCategory.cs
│   │   ├── IContentProvider.cs
│   │   └── INavigable.cs
│   ├── Services/
│   │   ├── CategoryRegistry.cs
│   │   ├── ContentLoader.cs
│   │   └── DisplayService.cs
│   └── Data/
│       ├── CategoryData.cs
│       └── ContentRepository.cs
└── UI/
    ├── MenuRenderer.cs
    ├── InputHandler.cs
    └── ConsoleHelper.cs
```

## Key Principles

1. **Interface-based design** - All major components should have interfaces for abstraction
2. **Namespace separation** - Features belong in their own namespace (Features/CodeTrainer, Features/Lexicon, etc.)
3. **Decoupling** - Components should not be tightly coupled; use dependency injection
4. **Easy extensibility** - Adding/removing features should require minimal changes
5. **Single Responsibility** - Each class handles one concern
6. **Open/Closed** - Extend behavior without modifying existing code

# Future Content Requests

## Breadcrumb Navigation
Display the current navigation path at the top of each screen using `NavigationManager.GetCurrentPath()`.

**Implementation idea:**
- Call `_navigation.GetCurrentPath()` before rendering each menu
- Display like: `Lexicon > Git Commands > Repository Inspection`
- Helps users understand where they are in the hierarchy

**Method already exists:** `NavigationManager.GetCurrentPath()` returns the path as a string.

---

## INavigable Interface for Enhanced Navigation
Create a common interface for any navigable item to enable polymorphic navigation.

**Interface concept:**
```csharp
public interface INavigable
{
    string DisplayName { get; }
    void Navigate();
    bool CanNavigateBack { get; }
}
```

**Use cases:**
- Make Categories, SubCategories, and ContentItems all navigable through a single interface
- Enable a unified navigation stack that handles any navigable item
- Simplify NavigationManager by treating all items uniformly

---

## Unlockable Categories (Achievement System)
Use `ContentLoader.UnloadCategory()` to create locked content that unlocks based on achievements.

**Concept:**
- Categories start locked/hidden until player achieves certain milestones
- Reaching score thresholds unlocks new content
- Completing specific quizzes or code challenges grants access to advanced topics

**Implementation ideas:**
- Create a global save file (JSON) storing player achievements and unlocked content
- `AchievementService` tracks progress and triggers unlocks
- Use `ContentLoader.LoadCategory()` dynamically when achievements are earned
- Display locked categories as grayed out with unlock requirements shown

**Example unlocks:**
- Score 500+ points → Unlock "Advanced Git Workflows"
- Complete all Git quizzes → Unlock "Git Internals Deep Dive"
- 100% on Code Training → Unlock "Expert Challenges"

**Required components:**
- `achievements.json` - Store player achievements and unlock states
- `AchievementService` - Track and evaluate achievement conditions
- UI indicator for locked vs unlocked content

**Method already exists:** `ContentLoader.UnloadCategory(name)` can hide categories at runtime.

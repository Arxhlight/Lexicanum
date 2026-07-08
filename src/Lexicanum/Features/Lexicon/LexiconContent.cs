using Lexicanum.Navigation;

namespace Lexicanum.Features.Lexicon;

/// <summary>
/// The lexicon's reference material and the menu tree that exposes it.
/// </summary>
public static class LexiconContent
{
    public static MenuNode CreateMenuNode()
    {
        return MenuNode.Branch("Lexicon", "Reference guides and command documentation",
            CreateTerminalCommandsNode(),
            CreateGitCommandsNode(),
            CreateGitLfsNode(),
            CreateFlagReferenceNode(),
            CreateWorkflowsNode(),
            CreateSafetyWarningsNode());
    }

    private static MenuNode EntryLeaf(string title, string description, string entryTitle, Func<string> contentFactory)
    {
        return MenuNode.Leaf(title, description, () => new LexiconEntryScreen(entryTitle, contentFactory()));
    }

    #region Terminal Commands

    private static MenuNode CreateTerminalCommandsNode()
    {
        return MenuNode.Branch("Terminal Commands", "Command line essentials",
            EntryLeaf("Filesystem Navigation", "Navigate and manage files", "Filesystem Navigation", GetFilesystemNavigationContent),
            EntryLeaf("File Operations", "Create, copy, move, and delete files", "File Operations", GetFileOperationsContent),
            EntryLeaf("Process Management", "Manage running processes", "Process Management", GetProcessManagementContent));
    }

    private static string GetFilesystemNavigationContent()
    {
        return @"
## Filesystem Navigation

Essential commands for navigating the filesystem.

## List Files
```
ls <path>                    # List files in directory
ls -l <path>                 # List files with detailed information
ls -la <path>                # List all files including hidden ones
dir                          # List files (Windows)
```

## Change Directory
```
cd <path>                    # Change to specified directory
cd ..                        # Move up one level
cd ~                         # Move to home directory
cd -                         # Move to previous directory
pwd                          # Print current working directory
```

## Create & Delete Directories
```
mkdir <folder>               # Create new directory
mkdir -p path/to/dir         # Create nested directories
rm -rf <folder>              # Delete folder and contents recursively
```

## View File Contents
```
cat <file>                   # Display file contents
echo ""text"" > <file>         # Write text to file (overwrites)
```
";
    }

    private static string GetFileOperationsContent()
    {
        return @"
## File Operations

Commands for creating, copying, moving, and deleting files.

## Create Files
```
touch <filename>             # Create empty file (Unix/Mac)
echo """" > <filename>         # Create empty file (cross-platform)
```

## Copy
```
cp <source> <dest>           # Copy file
cp -r <source> <dest>        # Copy directory recursively
```

## Move/Rename
```
mv <source> <dest>           # Move or rename file/directory
```

## Delete
```
rm <file>                    # Delete file
rm -r <directory>            # Delete directory recursively
rm -rf <directory>           # Force delete (no confirmation - careful!)
```
";
    }

    private static string GetProcessManagementContent()
    {
        return @"
## Process Management

Commands for managing running processes.

## View Processes
```
ps                           # List processes
ps aux                       # Detailed process list (Unix/Mac)
top                          # Interactive process viewer
htop                         # Better interactive viewer (if installed)
```

## Kill Processes
```
kill <PID>                   # Terminate process by ID
kill -9 <PID>                # Force kill process
killall <name>               # Kill all processes by name
```

## Background Jobs
```
<command> &                  # Run command in background
jobs                         # List background jobs
fg                           # Bring most recent job to foreground
fg %<job-number>             # Bring specific job to foreground
bg                           # Continue paused job in background
Ctrl+Z                       # Pause current foreground process
```
";
    }

    #endregion

    #region Git Commands

    private static MenuNode CreateGitCommandsNode()
    {
        return MenuNode.Branch("Git Commands", "Version control commands and workflows",
            EntryLeaf("Repository Inspection", "Inspect repository state", "Repository Inspection", GetGitInspectionContent),
            EntryLeaf("Staging & Committing", "Stage and commit changes", "Staging & Committing", GetGitStagingContent),
            EntryLeaf("Working Directory Changes", "Manage uncommitted changes", "Working Directory Changes", GetWorkingDirectoryContent),
            EntryLeaf("Branch Management", "Create, switch, and delete branches", "Branch Management", GetBranchManagementContent),
            EntryLeaf("Merging", "Merge branches together", "Merging", GetMergingContent),
            EntryLeaf("Rebasing", "Rebase commits onto another branch", "Rebasing", GetRebasingContent),
            EntryLeaf("Remote Operations", "Push, pull, and fetch", "Remote Operations", GetRemoteOperationsContent),
            MenuNode.Branch("Submodules", "Manage Git submodules",
                EntryLeaf("Submodule Commands", "Everyday submodule operations", "Submodule Commands", GetSubmoduleCommandsContent),
                EntryLeaf("Submodule Status Symbols", "Reading submodule status output", "Submodule Status Symbols", GetSubmoduleStatusContent)));
    }

    private static string GetGitInspectionContent()
    {
        return @"
## Repository Inspection

Commands to inspect repository state and history.

## Status & Diff
```
git status                   # Show branch and working tree status
git diff                     # Show unstaged changes
git diff --staged            # Show staged changes ready to commit
```

## History
```
git log --oneline            # Display compact commit history
git log --graph --oneline --all  # Visual branch graph
git show <commit-hash>       # Show details of specific commit
git show HEAD                # Show details of latest commit
```

## Branches & Remotes
```
git branch                   # List local branches
git branch -r                # List remote branches only
git branch -a                # List all branches (local and remote)
git remote -v                # Show remote repository URLs
```
";
    }

    private static string GetGitStagingContent()
    {
        return @"
## Staging & Committing

Stage changes and create commits.

## Staging Files
```
git add <file>               # Stage specific file for commit
git add .                    # Stage all changes in current directory
git add -p                   # Interactive staging (choose hunks)
```

## Unstaging Files
```
git reset <file>             # Unstage file but keep changes (classic)
git reset HEAD <file>        # Same as above, explicit HEAD
git restore --staged <file>  # Modern way to unstage file
git restore --staged .       # Unstage all staged files
```

## Committing
```
git commit -m ""message""      # Create commit with message
git commit -am ""message""     # Add tracked files and commit in one step
git commit --amend           # Modify last commit (message or content)
```

## Best Practices
- Write clear, descriptive commit messages
- Keep commits atomic (one logical change per commit)
- Use present tense in commit messages
";
    }

    private static string GetWorkingDirectoryContent()
    {
        return @"
## Working Directory Changes

Manage uncommitted changes in your working directory.

## Discard Changes
```
git restore <file>           # Discard changes to specific file (DANGEROUS)
git restore .                # Discard all unstaged changes (DANGEROUS)
```

## Compare Changes
```
git diff HEAD~1              # Compare current state with previous commit
```

⚠️ WARNING: git restore permanently discards changes!
";
    }

    private static string GetBranchManagementContent()
    {
        return @"
## Branch Management

Create, switch, and delete branches.

## Switch Branches
```
git checkout <branch>        # Switch to existing branch (classic)
git switch <branch>          # Switch to existing branch (modern)
```

## Create Branches
```
git checkout -b <new-branch> # Create and switch to new branch
git checkout -b <new> <source> # Create new branch from source branch
git switch -c <new-branch>   # Create and switch (modern)
```

## Delete Branches
```
git branch -d <branch>       # Delete local branch (safe)
git branch -D <branch>       # Force delete local branch
```

## Rename Branch
```
git branch -m <old> <new>    # Rename branch
git branch -m <new-name>     # Rename current branch
```
";
    }

    private static string GetMergingContent()
    {
        return @"
## Merging

Merge branches together.

## Basic Merge
```
git merge <branch>           # Merge specified branch into current branch
git merge --no-ff <branch>   # Merge with explicit merge commit
```

## Squash Merge
```
git merge --squash <branch>  # Squash all commits into single commit
```

## After Squash Merge
```
git commit -m ""message""      # Complete squash merge with commit
```
";
    }

    private static string GetRebasingContent()
    {
        return @"
## Rebasing

Rebase commits onto another branch.

## Basic Rebase
```
git rebase <base-branch>     # Replay current branch commits onto base
git rebase <base> <branch>   # Rebase branch onto base (not on branch)
```

## Rebase Control
```
git rebase --continue        # Continue after resolving conflicts
git rebase --abort           # Cancel rebase and restore original state
git rebase --skip            # Skip current conflicting commit
```

⚠️ CAUTION: Rebasing rewrites commit history!
";
    }

    private static string GetRemoteOperationsContent()
    {
        return @"
## Remote Operations

Push, pull, and fetch from remotes.

## Push
```
git push                     # Push to configured upstream branch
git push origin <branch>     # Push specific branch to origin
git push -u origin <branch>  # Push and set upstream tracking
git push --force-with-lease  # Safe force push after rebase
```

## Fetch
```
git fetch                    # Download remote refs without merging
git fetch origin             # Update references from origin remote
```
";
    }

    private static string GetSubmoduleCommandsContent()
    {
        return @"
## Submodule Commands

Manage Git submodules.

## Cloning with Submodules
```
git clone --recurse-submodules <repo-url>   # Clone repo and all submodules
```

## Initialize After Clone
If you already cloned without --recurse-submodules:
```
git submodule update --init --recursive     # Initialize all submodules
```

## Add Submodule
```
git submodule add <repo-url> <path>        # Add submodule to repository
git submodule add -b <branch> <repo> <path> # Add submodule tracking specific branch
```

## Initialize & Update
```
git submodule init                         # Initialize submodules from .gitmodules
git submodule status                       # Show submodule commit status
git submodule update                       # Checkout recorded commit
git submodule update --init                # Clone and initialize submodules
git submodule update --init --recursive    # Initialize nested submodules
git submodule update --remote              # Update to latest commit from remote
```

## Remove Submodule
```
git submodule deinit <path>                # Remove submodule working tree
git submodule deinit -f <path>             # Force remove (discard local changes)
git rm <path>                              # Remove submodule from repository
```

## Batch Operations
```
git submodule foreach '<command>'          # Run command in each submodule
git submodule foreach --recursive '<cmd>'  # Run in all nested submodules
git submodule sync                         # Sync URLs from .gitmodules
git submodule sync --recursive             # Sync all nested submodules
```

## Configuration
```
git submodule set-branch --branch <branch> <path>  # Change tracked branch
git submodule set-branch --default <path>  # Use default branch
git submodule set-url <path> <newurl>      # Change submodule URL
```

## Commands in Submodule
```
git -C <path> status                       # Run git command in submodule directory
git -C <path> commit -m \""msg\""              # Commit inside submodule
git -C <path> lfs fetch                    # Fetch LFS files in submodule
```
";
    }

    private static string GetSubmoduleStatusContent()
    {
        return @"
## Submodule Status Symbols

Understanding submodule status output.

## Status Symbols
```
-<commit>                    # Submodule registered but not cloned
 <commit>                    # Submodule correct and initialized
+<commit>                    # Submodule at wrong commit
U<commit>                    # Submodule has merge conflict
```

## Example
```
$ git submodule status
-abc1234 path/to/submodule   # Not yet cloned
 def5678 another/submodule   # Correct state
+ghi9012 third/submodule     # Wrong commit checked out
```
";
    }

    #endregion

    #region Git LFS

    private static MenuNode CreateGitLfsNode()
    {
        return MenuNode.Branch("Git LFS Commands", "Large File Storage commands",
            EntryLeaf("LFS Inspection", "Inspect LFS tracked files", "LFS Inspection", GetLfsInspectionContent),
            EntryLeaf("LFS in Submodules", "LFS commands for submodules", "LFS in Submodules", GetLfsSubmodulesContent));
    }

    private static string GetLfsInspectionContent()
    {
        return @"
## LFS Inspection

Commands to inspect and manage LFS tracked files.

## Check LFS
```
git lfs version              # Check if Git LFS is installed
git lfs ls-files             # List all LFS-tracked files
git lfs track                # Show current LFS tracking rules
```

## Untrack Files
```
git lfs untrack ""*.ext""      # Stop tracking file pattern
```
";
    }

    private static string GetLfsSubmodulesContent()
    {
        return @"
## LFS in Submodules

Git LFS commands for submodules.

## Fetch & Checkout
```
git -C <submodule> lfs fetch --all    # Download all LFS files
git -C <submodule> lfs checkout       # Replace pointers with actual files
```

## Disable LFS
```
git -C <submodule> lfs uninstall --local # Disable LFS in submodule
```
";
    }

    #endregion

    #region Flag Reference

    private static MenuNode CreateFlagReferenceNode()
    {
        return EntryLeaf("Flag Reference", "Common command flags explained", "Common Flags", GetFlagReferenceContent);
    }

    private static string GetFlagReferenceContent()
    {
        return @"
## Flag Reference

Common command line flags and their meanings.

## Branch Flags
```
-b                           # Create new branch
-d                           # Delete branch (safe)
-D                           # Force delete branch
```

## Remote & Tracking
```
-u                           # Set upstream tracking
-a                           # All (context-dependent: branches, files)
-r                           # Recursive operation
-C <path>                    # Run command in specified directory
```

## Force & Safety
```
--force                      # Override safety checks (DANGEROUS)
--force-with-lease           # Safer force push
```

## Index & Staging
```
--cached                     # Operate on index only
--staged                     # Operate on staged files
```

## Repository Scope
```
--local                      # Local repository scope only
--recursive                  # Include nested repositories
--init                       # Initialize repository or submodule
```

## Merge & Rebase
```
--squash                     # Combine commits into single commit
--rebase                     # Reapply commits on new base
```

## Operation Control
```
--continue                   # Resume paused operation
--abort                      # Cancel operation and restore state
--skip                       # Skip current step in operation
```
";
    }

    #endregion

    #region Common Workflows

    private static MenuNode CreateWorkflowsNode()
    {
        return MenuNode.Branch("Common Workflows", "Step-by-step workflow guides",
            EntryLeaf("Feature Branch Workflow", "Create branch, commit, push", "Feature Branch Workflow", GetFeatureBranchWorkflowContent),
            EntryLeaf("Quick Commit Workflow", "Stage all and push", "Quick Commit Workflow", GetQuickCommitWorkflowContent),
            EntryLeaf("Submodule Workflow", "Add and manage submodules", "Add Submodule Workflow", GetSubmoduleWorkflowContent),
            EntryLeaf("Rebase Workflow", "Rebase feature onto main", "Rebase Feature Workflow", GetRebaseWorkflowContent),
            EntryLeaf("Unstage Workflow", "Safely unstage files", "Unstage Files Safely", GetUnstageWorkflowContent));
    }

    private static string GetFeatureBranchWorkflowContent()
    {
        return @"
## Feature Branch Workflow

Create Branch → Commit → Push

## Steps
```
git checkout -b <branch>     # 1. Create and switch to new branch
git add <file>               # 2. Stage changes
git commit -m ""message""      # 3. Create commit
git push -u origin <branch>  # 4. Push and set upstream
```

## Example
```
git checkout -b feature/login-page
git add src/login.cs
git commit -m ""Add login page component""
git push -u origin feature/login-page
```
";
    }

    private static string GetQuickCommitWorkflowContent()
    {
        return @"
## Quick Commit Workflow

Commit All Changes → Push

## Steps
```
git add .                    # 1. Stage all changes
git commit -m ""message""      # 2. Create commit
git push origin <branch>     # 3. Push to remote
```

## Example
```
git add .
git commit -m ""Fix bug in user validation""
git push origin main
```
";
    }

    private static string GetSubmoduleWorkflowContent()
    {
        return @"
## Add Submodule Workflow

Add a submodule to your repository.

## Steps
```
git submodule add <url> <path>  # 1. Register submodule
git commit -m ""Add submodule""   # 2. Save submodule reference
git push origin <branch>        # 3. Push changes
```

## Example
```
git submodule add https://github.com/lib/utils.git external/utils
git commit -m ""Add utils library as submodule""
git push origin main
```
";
    }

    private static string GetRebaseWorkflowContent()
    {
        return @"
## Rebase Feature onto Main

Update your feature branch with latest main.

## Steps
```
git checkout <feature>       # 1. Switch to feature branch
git fetch origin             # 2. Update remote references
git rebase origin/main       # 3. Rebase onto latest main
git push --force-with-lease  # 4. Update remote (if needed)
```

## Example
```
git checkout feature/login-page
git fetch origin
git rebase origin/main
git push --force-with-lease
```

⚠️ Only force push if you're the only one working on the branch!
";
    }

    private static string GetUnstageWorkflowContent()
    {
        return @"
## Unstage Files Safely

Remove files from staging without losing changes.

## Unstage All Files
```
git restore --staged .       # Unstage all files
```

## Unstage Specific File
```
git restore --staged <file>  # Unstage specific file
```

## Example
```
git add .
git restore --staged secrets.json  # Oops, don't commit this!
git commit -m ""Add new features""
```
";
    }

    #endregion

    #region Safety Warnings

    private static MenuNode CreateSafetyWarningsNode()
    {
        return EntryLeaf("Safety Warnings", "Dangerous commands to use with caution", "Dangerous Commands", GetSafetyWarningsContent);
    }

    private static string GetSafetyWarningsContent()
    {
        return @"
## Safety Warnings

⚠️ These commands can cause data loss. Use with caution!

## DANGER: Permanent Data Loss
```
git restore .                # Permanently discards local changes
git rm -rf                   # Permanently deletes files
git push --force             # Can overwrite others' work
```

## CAUTION: History Rewriting
```
git rebase                   # Rewrites commit history
```

## SAFER Alternatives
```
git push --force-with-lease  # Checks remote state first
git stash                    # Temporarily save changes instead of discard
```

## Before Using Dangerous Commands
1. Make sure you have a backup or the changes are committed elsewhere
2. Double-check you're on the right branch
3. Verify what will be affected with --dry-run when available
4. Consider if there's a safer alternative
";
    }

    #endregion
}

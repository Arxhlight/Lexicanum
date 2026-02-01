using Lexicanum.Core.Interfaces;
using Lexicanum.Core.Models;
using Lexicanum.UI;

namespace Lexicanum.Features.Lexicon
{
    public class LexiconViewer
    {
        private readonly ConsoleHelper _console;

        public LexiconViewer(ConsoleHelper console)
        {
            _console = console;
        }

        public static Category CreateLexiconCategory()
        {
            var category = new Category("Lexicon", "Reference guides and command documentation");

            // Terminal Commands Category
            category.AddSubCategory(CreateTerminalCommandsSubCategory());
            
            // Git Commands Category
            category.AddSubCategory(CreateGitCommandsSubCategory());
            
            // Git LFS Commands Category
            category.AddSubCategory(CreateGitLfsSubCategory());
            
            // Flag Reference Category
            category.AddSubCategory(CreateFlagReferenceSubCategory());
            
            // Common Workflows Category
            category.AddSubCategory(CreateWorkflowsSubCategory());
            
            // Safety Warnings Category
            category.AddSubCategory(CreateSafetyWarningsSubCategory());

            return category;
        }

        #region Terminal Commands

        private static SubCategory CreateTerminalCommandsSubCategory()
        {
            var terminalCommands = new SubCategory("Terminal Commands", "Command line essentials");
            
            // Filesystem Navigation
            var filesystemNav = new SubCategory("Filesystem Navigation", "Navigate and manage files");
            filesystemNav.AddContentItem(new LexiconEntry("Filesystem Navigation", GetFilesystemNavigationContent()));
            terminalCommands.AddSubcategory(filesystemNav);

            // File Operations
            var fileOps = new SubCategory("File Operations", "Create, copy, move, and delete files");
            fileOps.AddContentItem(new LexiconEntry("File Operations", GetFileOperationsContent()));
            terminalCommands.AddSubcategory(fileOps);

            // Process Management
            var processMgmt = new SubCategory("Process Management", "Manage running processes");
            processMgmt.AddContentItem(new LexiconEntry("Process Management", GetProcessManagementContent()));
            terminalCommands.AddSubcategory(processMgmt);

            return terminalCommands;
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

        private static SubCategory CreateGitCommandsSubCategory()
        {
            var gitCommands = new SubCategory("Git Commands", "Version control commands and workflows");

            // Repository Inspection
            var repoInspection = new SubCategory("Repository Inspection", "Inspect repository state");
            repoInspection.AddContentItem(new LexiconEntry("Repository Inspection", GetGitInspectionContent()));
            gitCommands.AddSubcategory(repoInspection);

            // Staging & Committing
            var stagingCommitting = new SubCategory("Staging & Committing", "Stage and commit changes");
            stagingCommitting.AddContentItem(new LexiconEntry("Staging & Committing", GetGitStagingContent()));
            gitCommands.AddSubcategory(stagingCommitting);

            // Working Directory Changes
            var workingDir = new SubCategory("Working Directory Changes", "Manage uncommitted changes");
            workingDir.AddContentItem(new LexiconEntry("Working Directory Changes", GetWorkingDirectoryContent()));
            gitCommands.AddSubcategory(workingDir);

            // Branch Management
            var branchMgmt = new SubCategory("Branch Management", "Create, switch, and delete branches");
            branchMgmt.AddContentItem(new LexiconEntry("Branch Management", GetBranchManagementContent()));
            gitCommands.AddSubcategory(branchMgmt);

            // Merging
            var merging = new SubCategory("Merging", "Merge branches together");
            merging.AddContentItem(new LexiconEntry("Merging", GetMergingContent()));
            gitCommands.AddSubcategory(merging);

            // Rebasing
            var rebasing = new SubCategory("Rebasing", "Rebase commits onto another branch");
            rebasing.AddContentItem(new LexiconEntry("Rebasing", GetRebasingContent()));
            gitCommands.AddSubcategory(rebasing);

            // Remote Operations
            var remoteOps = new SubCategory("Remote Operations", "Push, pull, and fetch");
            remoteOps.AddContentItem(new LexiconEntry("Remote Operations", GetRemoteOperationsContent()));
            gitCommands.AddSubcategory(remoteOps);

            // Submodules
            var submodules = new SubCategory("Submodules", "Manage Git submodules");
            submodules.AddContentItem(new LexiconEntry("Submodule Commands", GetSubmoduleCommandsContent()));
            submodules.AddContentItem(new LexiconEntry("Submodule Status Symbols", GetSubmoduleStatusContent()));
            gitCommands.AddSubcategory(submodules);

            return gitCommands;
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

## Add Submodule
```
git submodule add <repo-url> <path>        # Add submodule to repository
```

## Status & Update
```
git submodule status                       # Show submodule commit status
git submodule update                       # Checkout recorded commit
git submodule update --init                # Clone and initialize submodules
git submodule update --init --recursive    # Initialize nested submodules
```

## Commands in Submodule
```
git -C <path> status                       # Run git command in submodule directory
git -C <path> commit -m ""msg""              # Commit inside submodule
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

        private static SubCategory CreateGitLfsSubCategory()
        {
            var gitLfs = new SubCategory("Git LFS Commands", "Large File Storage commands");

            // LFS Inspection
            var lfsInspection = new SubCategory("LFS Inspection", "Inspect LFS tracked files");
            lfsInspection.AddContentItem(new LexiconEntry("LFS Inspection", GetLfsInspectionContent()));
            gitLfs.AddSubcategory(lfsInspection);

            // LFS in Submodules
            var lfsSubmodules = new SubCategory("LFS in Submodules", "LFS commands for submodules");
            lfsSubmodules.AddContentItem(new LexiconEntry("LFS in Submodules", GetLfsSubmodulesContent()));
            gitLfs.AddSubcategory(lfsSubmodules);

            return gitLfs;
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

        private static SubCategory CreateFlagReferenceSubCategory()
        {
            var flagRef = new SubCategory("Flag Reference", "Common command flags explained");
            flagRef.AddContentItem(new LexiconEntry("Common Flags", GetFlagReferenceContent()));
            return flagRef;
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

        private static SubCategory CreateWorkflowsSubCategory()
        {
            var workflows = new SubCategory("Common Workflows", "Step-by-step workflow guides");

            // Feature Branch Workflow
            var featureBranch = new SubCategory("Feature Branch Workflow", "Create branch, commit, push");
            featureBranch.AddContentItem(new LexiconEntry("Feature Branch Workflow", GetFeatureBranchWorkflowContent()));
            workflows.AddSubcategory(featureBranch);

            // Quick Commit Workflow
            var quickCommit = new SubCategory("Quick Commit Workflow", "Stage all and push");
            quickCommit.AddContentItem(new LexiconEntry("Quick Commit Workflow", GetQuickCommitWorkflowContent()));
            workflows.AddSubcategory(quickCommit);

            // Submodule Workflow
            var submoduleWorkflow = new SubCategory("Submodule Workflow", "Add and manage submodules");
            submoduleWorkflow.AddContentItem(new LexiconEntry("Add Submodule Workflow", GetSubmoduleWorkflowContent()));
            workflows.AddSubcategory(submoduleWorkflow);

            // Rebase Workflow
            var rebaseWorkflow = new SubCategory("Rebase Workflow", "Rebase feature onto main");
            rebaseWorkflow.AddContentItem(new LexiconEntry("Rebase Feature Workflow", GetRebaseWorkflowContent()));
            workflows.AddSubcategory(rebaseWorkflow);

            // Unstage Workflow
            var unstageWorkflow = new SubCategory("Unstage Workflow", "Safely unstage files");
            unstageWorkflow.AddContentItem(new LexiconEntry("Unstage Files Safely", GetUnstageWorkflowContent()));
            workflows.AddSubcategory(unstageWorkflow);

            return workflows;
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

        private static SubCategory CreateSafetyWarningsSubCategory()
        {
            var safety = new SubCategory("Safety Warnings", "Dangerous commands to use with caution");
            safety.AddContentItem(new LexiconEntry("Dangerous Commands", GetSafetyWarningsContent()));
            return safety;
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
}

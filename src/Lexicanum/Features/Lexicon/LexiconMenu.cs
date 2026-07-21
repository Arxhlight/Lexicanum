using Lexicanum.Core.Content;
using Lexicanum.Navigation;

namespace Lexicanum.Features.Lexicon;

/// <summary>
/// Builds the lexicon menu tree from content packs: each pack becomes a branch of
/// entry screens, and a single-entry pack becomes a direct leaf.
/// </summary>
public static class LexiconMenu
{
    public static MenuNode CreateMenuNode(IReadOnlyList<ContentPack<LexiconEntry>> lexiconPacks)
    {
        var packNodes = lexiconPacks.Select(CreatePackNode).ToArray();

        return MenuNode.Branch("Lexicon", "Reference guides and command documentation", packNodes);
    }

    private static MenuNode CreatePackNode(ContentPack<LexiconEntry> pack)
    {
        if (pack.Items.Count == 1)
        {
            return EntryLeaf(pack.Category, pack.Description, pack.Items[0]);
        }

        var entryLeaves = pack.Items
            .Select(entry => EntryLeaf(entry.Title, entry.Description, entry))
            .ToArray();

        return MenuNode.Branch(pack.Category, pack.Description, entryLeaves);
    }

    private static MenuNode EntryLeaf(string title, string description, LexiconEntry entry)
    {
        return MenuNode.Leaf(title, description, () => new LexiconEntryScreen(entry.Title, entry.Body));
    }
}

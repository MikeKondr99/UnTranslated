using System.Collections.ObjectModel;

namespace UnTranslated.ViewModels;

internal partial class Translation : IAssets
{
    public Translation(string name, string path, bool isOriginal = false)
    {
        Name = name;
        Path = path;
        IsOriginal = isOriginal;
    }


    public string Path { get; init; }
    public string Name { get; init; }
    public bool IsOriginal { get; init; }


    public ObservableCollection<TreeNode> Assets { get; init; } = new ObservableCollection<TreeNode>()
    {
        new TreeNode(null,"Language",
            "english-achievements.xml",
            "english-creatures_eng-US.xml",
            "english-events.xml",
            "english-exit-console.xml",
            "english-exit_eng-US.xml",
            "english-gameplayhints.xml",
            "english-gods_eng-US.xml",
            "english-intro-hints.xml",
            "english-intro.xml",
            "english-items.xml",
            "english-major-mysteries.xml",
            "english-new.xml",
            "english-new_dlc_DR_eng-US.xml",
            "english-other.xml",
            "english-puzzle_hints_eng-US.xml",
            "english-quests_eng-US.xml",
            "english-runes.xml",
            "english-special-stories.xml",
            "english-tutorial_eng-US.xml",
            "english-ui.xml",
            "english-vendors.xml",
            "english-weapons.xml"
        ),
        new TreeNode(null,"Sprites",new TreeNode[]
        {
            new TreeNode(null,"Fonts",
                "myndraine20.xnb",
                "myndraine20bold.xnb",
                "myndraine40.xnb",
                "myndraine40bold.xnb"
            )
        }),
    };
}

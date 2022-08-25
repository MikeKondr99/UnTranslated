using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

namespace UnTranslated.ViewModels;

[INotifyPropertyChanged]
internal partial class TreeNode
{
    public TreeNode(TreeNode parent, string name)
    {
        this.Name = name;
        this.Parent = parent;
    }

    public TreeNode(TreeNode parent, string name, params string[] children)
    {
        this.Name = name;
        this.Parent = parent;
        Children = new ObservableCollection<TreeNode>(children.Select(x => new TreeNode(this, x)));
    }

    public TreeNode(TreeNode parent, string name, TreeNode[] dirs, params string[] children)
    {
        this.Name = name;
        this.Parent = parent;
        foreach (var dir in dirs)
            dir.Parent = this;
        Children = new ObservableCollection<TreeNode>(dirs.Concat<TreeNode>(children.Select(x => new TreeNode(this, x))));
    }

    [ObservableProperty]
    ObservableCollection<TreeNode> children = new ObservableCollection<TreeNode>();

    [ObservableProperty]
    bool isSelected = false;

    [ObservableProperty]
    bool isExpanded = false;

    [ObservableProperty]
    string name;

    [ObservableProperty]
    TreeNode parent;

    public string Path
    {
        get
        {
            if (Parent is null)
                return Name;
            else
                return $@"{Parent.Path}\{Name}";
        }
    }
}

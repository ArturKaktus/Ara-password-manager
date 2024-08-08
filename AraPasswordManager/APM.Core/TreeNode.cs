using System.Collections.ObjectModel;
using System.ComponentModel;
using APM.Core.Models.Interfaces;
using Avalonia.Controls;

namespace APM.Core;

public class TreeNode
{
    public ObservableCollection<TreeNode> Child { get; set; } = [];
    public IObject Item { get; set; }
    public bool IsSelected { get; set; }
    public TreeNode()
    {

    }

    public TreeNode(ObservableCollection<TreeNode> child)
    {
        Child = child;
    }
}

public class TreeNodeTransfer
{
    private TreeNode _SelectedTreeNode;

    public TreeNode SelectedTreeNode
    {
        get => _SelectedTreeNode;
        set
        {
            _SelectedTreeNode = value;
            OnPropertyChanged(nameof(SelectedTreeNode));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
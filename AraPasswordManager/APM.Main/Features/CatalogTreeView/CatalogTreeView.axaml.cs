using APM.Core;
using Avalonia.Controls;
using Avalonia.Input;
using System.Collections.Generic;
using System.Linq;

namespace APM.Main.Features.CatalogTreeView;

public partial class CatalogTreeView : UserControl
{
    private CatalogTreeViewViewModel viewModel = new();

    public CatalogTreeView()
    {
        this.DataContext = viewModel;
        InitializeComponent();
        CatalogTree.PointerReleased += CatalogTree_PointerReleased;
    }

    private void TreeView_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (sender is TreeView tw && tw.SelectedItem != null && tw.SelectedItem is TreeNode tn)
        {
            AppDocument.NodeTransfer.SelectedTreeNode = tn;
        }
    }

    private void CatalogTree_PointerReleased(object sender, PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Right)
        {
            var contextMenu = new ContextMenu();
            if (CatalogTree.SelectedItem is TreeNode tn)
            {
                var menuItems = GenerateMenuItems(tn);

                foreach (var menuItem in menuItems)
                {
                    contextMenu.Items.Add(menuItem);
                }
                contextMenu.Open(CatalogTree);
            }
        }
    }

    private List<MenuItem> GenerateMenuItems(TreeNode treeNote)
    {
        var menuItems = AppDocument.ContextMenuList.Where(item => item.CanExecute(treeNote)).OrderBy(i => i.Order);
        List<MenuItem> items = [];
        foreach (var menuItem in menuItems)
        {
            var m = new MenuItem()
            {
                Header = menuItem.Title,
                Command = menuItem.Execute,
                CommandParameter = viewModel,
                IsEnabled = menuItem.IsEnabledMenu(treeNote)
            };
            items.Add(m);
        }
        return items;
    }
}


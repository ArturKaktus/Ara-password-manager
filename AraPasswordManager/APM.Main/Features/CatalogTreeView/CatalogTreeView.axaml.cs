using APM.Core;
using APM.Main;
using APM.Main.Features.CatalogTreeView;
using Avalonia;
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
        TreeNode selectedItem;
        if (sender is TreeView tw)
        {
            AppDocument.NodeTransfer.SelectedTreeNode = tw.SelectedItem as TreeNode;
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


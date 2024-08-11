using APM.Core;
using APM.Main;
using APM.Main.Features.CatalogTreeView;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ara_password_manager.Features.CatalogTreeView;

public partial class CatalogTreeView : UserControl
{
    private CatalogTreeViewViewModel viewModel = new();
    public CatalogTreeView()
    {
        this.DataContext = viewModel;
        InitializeComponent();
    }
    private void TreeView_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        TreeNode selectedItem;
        if (sender is TreeView tw)
        {
            AppDocument.NodeTransfer.SelectedTreeNode = tw.SelectedItem as TreeNode;
        }
    }

    private void AddItem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedItem = CatalogTree.SelectedItem;
        viewModel.AddItem(selectedItem as TreeNode);
    }

    private void DeleteItem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedItem = CatalogTree.SelectedItem;
        viewModel.DeleteItem(selectedItem as TreeNode);
    }
}


using APM.Core;
using APM.Main;
using APM.Main.Features.CatalogTreeView;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ara_password_manager.Features.CatalogTreeView;

public partial class CatalogTreeView : UserControl
{
    public CatalogTreeView()
    {
        this.DataContext = new CatalogTreeViewViewModel();
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
}


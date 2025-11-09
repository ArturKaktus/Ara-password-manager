using APM.Core;
using APM.Desktop.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using System.Linq;

namespace APM.Desktop.Features.CatalogTreeView;

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
                var selectedVisualItem = CatalogTree.GetVisualDescendants()
                    .OfType<TreeViewItem>()
                    .FirstOrDefault(item => item.DataContext == tn);

                if (selectedVisualItem != null)
                {
                    var mousePosition = e.GetPosition(CatalogTree);
                    var position = selectedVisualItem.TranslatePoint(new Point(0, 0), CatalogTree);
                    var bounds = selectedVisualItem.Bounds;
                    if (mousePosition.X >= position.Value.X && mousePosition.X <= position.Value.X + bounds.Width &&
                        mousePosition.Y >= position.Value.Y && mousePosition.Y <= position.Value.Y + bounds.Height)
                    {
                        var menuItems = GetClassesUtils.GenerateMenuItems(viewModel, tn);

                        foreach (var menuItem in menuItems)
                        {
                            contextMenu.Items.Add(menuItem);
                        }
                        contextMenu.Open(CatalogTree);
                    }
                }
            }
        }
    }
}


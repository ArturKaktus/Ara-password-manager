using APM.Core;
using APM.Main.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
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
                // Находим визуальный элемент, соответствующий выбранному элементу
                var selectedVisualItem = CatalogTree.GetVisualDescendants()
                    .OfType<TreeViewItem>()
                    .FirstOrDefault(item => item.DataContext == tn);

                if (selectedVisualItem != null)
                {
                    // Получаем координаты указателя мыши относительно CatalogTree
                    var mousePosition = e.GetPosition(CatalogTree);
                    // Координаты относительно родительского элемента
                    var position = selectedVisualItem.TranslatePoint(new Point(0, 0), CatalogTree);
                    // Получаем прямоугольник выбранного элемента
                    var bounds = selectedVisualItem.Bounds;
                    // Проверяем, находится ли указатель мыши внутри выбранной ячейки
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


using APM.Core.Models.Interfaces;
using APM.Main.Utils;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace APM.Main.Features.CatalogTable;

public partial class CatalogTable : UserControl
{
    private CatalogTableViewModel viewModel = new();
    public CatalogTable()
    {
        this.DataContext = viewModel;
        InitializeComponent();
        TableGrid.PointerReleased += TableGrid_PointerReleased;
    }
    
    private void TableGrid_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Right)
        {
            var contextMenu = new ContextMenu();
            var mousePosition = e.GetPosition(TableGrid);
            var row = FindRowAtPosition(mousePosition);
            if (row != null && row.DataContext is IRecord specificRecord)
            {
                viewModel.SelectedRecord = specificRecord;
                var menuItems = GetClassesUtils.GenerateMenuItems(viewModel, specificRecord);
                foreach (var menuItem in menuItems)
                {
                    contextMenu.Items.Add(menuItem);
                }
                contextMenu.Open(TableGrid);
            }
        }
    }

    private DataGridRow FindRowAtPosition(Point position)
    {
        var hitTestResult = TableGrid.GetVisualAt(position);
        if (hitTestResult != null)
        {
            var visual = hitTestResult;
            while (visual != null)
            {
                if (visual is DataGridRow row)
                {
                    return row;
                }
                visual = visual.GetVisualParent();
            }
        }
        return null;
    }
}

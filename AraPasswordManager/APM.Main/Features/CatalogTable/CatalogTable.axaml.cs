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
            // ѕолучаем координаты указател€ мыши относительно TableGrid
            var mousePosition = e.GetPosition(TableGrid);

            // Ќаходим строку, на которую наведен указатель мыши с помощью HitTest
            var row = FindRowAtPosition(mousePosition);

            // ѕровер€ем, €вл€етс€ ли найденна€ строка определенной строкой
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
    // ћетод дл€ поиска строки по координатам
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

using APM.Core.Models.Interfaces;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace APM.Main.Features.CatalogTable;

public partial class CatalogTable : UserControl
{
    public CatalogTable()
    {
        this.DataContext = new CatalogTableViewModel();
        InitializeComponent();
        TableGrid.PointerReleased += TableGrid_PointerReleased;
    }

    private void TableGrid_PointerReleased(object? sender, Avalonia.Input.PointerReleasedEventArgs e)
    {
        if (e.InitialPressMouseButton == MouseButton.Right)
        {
            // ѕолучаем координаты указател€ мыши относительно TableGrid
            var mousePosition = e.GetPosition(TableGrid);

            // Ќаходим строку, на которую наведен указатель мыши с помощью HitTest
            var row = FindRowAtPosition(mousePosition);

            // ѕровер€ем, €вл€етс€ ли найденна€ строка определенной строкой
            if (row != null && row.DataContext is IRecord specificRecord)
            {

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

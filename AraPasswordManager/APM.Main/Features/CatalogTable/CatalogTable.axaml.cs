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
            // �������� ���������� ��������� ���� ������������ TableGrid
            var mousePosition = e.GetPosition(TableGrid);

            // ������� ������, �� ������� ������� ��������� ���� � ������� HitTest
            var row = FindRowAtPosition(mousePosition);

            // ���������, �������� �� ��������� ������ ������������ �������
            if (row != null && row.DataContext is IRecord specificRecord)
            {

            }
        }
    }
    // ����� ��� ������ ������ �� �����������
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

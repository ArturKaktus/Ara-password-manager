using Avalonia.Controls;
using Avalonia.Input;

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

        }
    }
}

using Avalonia.Controls;

namespace APM.Desktop.Features.MenuStrip;

public partial class MenuStrip : UserControl
{
    public MenuStrip()
    {
        this.DataContext = new MenuStripViewModel();
        InitializeComponent();
    }
}
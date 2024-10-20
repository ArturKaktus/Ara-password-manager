using APM.Main.Features.MenuStrip;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace APM.Main.Features.MenuStrip;

public partial class MenuStrip : UserControl
{
    public MenuStrip()
    {
        this.DataContext = new MenuStripViewModel();
        InitializeComponent();
    }
}
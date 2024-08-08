using APM.Main.Features.MenuStrip;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Ara_password_manager.Features.MenuStrip;

public partial class MenuStrip : UserControl
{
    public MenuStrip()
    {
        this.DataContext = new MenuStripViewModel();
        InitializeComponent();
    }
}
using APM.Main.Features.ContextMenuControls;
using Avalonia.Controls;

namespace APM.Main.Features.MenuStrip;

public partial class MenuStrip : UserControl
{
    public MenuStrip()
    {
        var model = new MenuStripViewModel();
        this.DataContext = model;
        InitializeComponent();
        FileMenu.ItemsSource = ContextMenuUtils.GenerateMenuItems(null, model);
    }
}
using APM.Main.Features.ContextMenuControls;
using Avalonia.Controls;

namespace APM.Main.Features.MenuStrip;

public partial class MenuStrip : UserControl
{
    public MenuStrip()
    {
        InitializeComponent();
        FileMenu.ItemsSource = ContextMenuUtils.GenerateMenuItems(null, this);
    }
}
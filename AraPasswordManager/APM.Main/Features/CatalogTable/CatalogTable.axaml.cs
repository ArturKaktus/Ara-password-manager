using APM.Main;
using APM.Main.Features.CatalogTable;
using Avalonia.Controls;
using Avalonia.Input;
using System.Linq;
using System.Reflection;
using APM.Core;
using System;
using APM.Core.Enums;
using Avalonia.Interactivity;

namespace Ara_password_manager.Features.CatalogTable;

public partial class CatalogTable : UserControl
{
    public CatalogTable()
    {
        this.DataContext = new CatalogTableViewModel();
        InitializeComponent();
    }

    private void TableGrid_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var point = e.GetCurrentPoint(TableGrid);
        if (point.Properties.IsRightButtonPressed)
        {
            var contextMenu = new Avalonia.Controls.ContextMenu();

            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes();
            var implementingTypes = types.Where(t => t.GetInterfaces().Contains(typeof(IContextMenu)) && t.IsClass);
            foreach (var t in implementingTypes)
            {
                var classInstance = Activator.CreateInstance(t);
                if (classInstance is IContextMenu icm && icm.CanExecute(ObjectType.Record))
                {
                    var mi = new MenuItem() { Header = icm.Title, IsEnabled = icm.IsEnabledMenu(null), Command = icm.ExecuteCommand};
                    contextMenu.Items.Add(mi);
                }
            }

            // Открываем контекстное меню
            contextMenu.Open(TableGrid);
        }
    }
    public static MenuItem BuildMenuItem(string header, bool isEnable, EventHandler<RoutedEventArgs>? onClickHandler = null)
    {
        var item = new MenuItem { Header = header };
        if (onClickHandler != null)
        {
            item.Click += onClickHandler;
        }

        return item;
    }
}

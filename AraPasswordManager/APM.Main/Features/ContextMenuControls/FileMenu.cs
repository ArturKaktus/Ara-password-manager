using APM.Core;
using APM.Main.Features.MenuStrip;
using Avalonia.Controls;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class FileMenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 10;

        public bool CanExecute(object parameter)
        {
            return parameter is MenuStripViewModel;
        }

        public void Exec(object parameter)
        {
            
        }

        public Task ExecAsync(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabledMenu(object parameter)
        {
            return true;
        }

        public MenuItem ReturnMenuItem(object? mainObj, object? obj)
        {
            var mi = new MenuItem()
            {
                Name = "File",
                Header = "Файл"
            };
            var menuList = ContextMenuUtils.GenerateMenuItems(null, mi);
            mi.ItemsSource = menuList;
            return mi;
        }
    }
}

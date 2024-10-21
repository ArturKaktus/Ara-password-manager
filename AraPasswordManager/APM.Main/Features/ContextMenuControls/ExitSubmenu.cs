using APM.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class ExitSubmenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 40;

        public bool CanExecute(object parameter)
        {
            return parameter is MenuItem mi && mi.Name == "File";
        }

        public void Exec(object parameter)
        {
            var app = Application.Current;
            if (app?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }

        public Task ExecAsync(object parameter)
        {
            throw new NotImplementedException();
        }

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem ReturnMenuItem(object? mainObj, object? obj)
        {
            return new MenuItem()
            {
                Header = "Выход",
                Command = Execute
            };
        }
    }
}

using APM.Core;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class CreateNewFileSubmenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 10;

        public bool CanExecute(object parameter)
        {
            return parameter is MenuItem mi && mi.Name == "File";
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
            return null;
        }
    }
}

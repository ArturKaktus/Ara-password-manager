using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Desktop.Features.CatalogTable;
using APM.Desktop.Features.CatalogTreeView;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Desktop.Features.ContextMenuControls
{
    internal class DeleteRecordContextMenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 20;

        public bool CanExecute(object parameter) => parameter is IRecord;

        public void Exec(object parameter)
        {
            if (parameter is CatalogTableViewModel table)
            {
                AppDocument.CurrentDatabaseModel.DeleteRecordById(table.SelectedRecord.Id);
                table.RefreshTable();
            }
        }

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem? ReturnMenuItem(object? mainObj, object? obj)
        {
            if (mainObj is CatalogTableViewModel uc && obj is IRecord)
            {
                return new MenuItem()
                {
                    Header = "Удалить запись",
                    Command = Execute,
                    CommandParameter = uc,
                    IsEnabled = IsEnabledMenu(obj)
                };
            }
            return null;
        }
    }
}

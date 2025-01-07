using System.Windows.Input;
using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using APM.Main.Features.CatalogTable;
using APM.Main.Features.CatalogTable.Controls.RecordProps;
using Avalonia;
using Avalonia.Controls;

namespace APM.Main.Features.ContextMenuControls;

public class EditRecordContextMenu : IContextMenu
{
    public bool CanExecute(object parameter) => parameter is IRecord;

    public bool IsEnabledMenu(object parameter) => true;

    public ICommand? Execute => new DelegateCommand(Exec);
    
    public async void Exec(object parameter)
    {
        if (parameter is CatalogTableViewModel table)
        {
            var rec = ((RecordModel)table.SelectedRecord).Clone() as RecordModel;
            var context = new RecordPropsView(rec);
            var result = await WindowManager.ShowConfirmDialog(Application.Current, context, "Введите запись", 200, 320);
            if (result)
            {
                AppDocument.CurrentDatabaseModel.EditRecord(rec);
            }
        }
    }

    public int Order => 10;
    public MenuItem? ReturnMenuItem(object? mainObj, object? obj)
    {
        if (mainObj is CatalogTableViewModel uc && obj is IRecord)
        {
            return new MenuItem()
            {
                Header = "Редактировать запись",
                Command = Execute,
                CommandParameter = uc,
                IsEnabled = IsEnabledMenu(obj)
            };
        }
        return null;
    }
}
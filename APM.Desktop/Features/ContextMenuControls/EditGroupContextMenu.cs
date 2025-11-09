using System.Threading.Tasks;
using System.Windows.Input;
using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using APM.Desktop.Features.CatalogTreeView;
using Avalonia.Controls;

namespace APM.Desktop.Features.ContextMenuControls;

public class EditGroupContextMenu : IContextMenu
{
    public bool CanExecute(object parameter) => parameter is TreeNode { Item: IGroup };

    public bool IsEnabledMenu(object parameter) => parameter is TreeNode { Item: IGroup } tn && tn.Item.Id != 1;

    public ICommand? Execute => new DelegateCommand(Exec);

    public async void Exec(object parameter)
    {
        if (parameter is not CatalogTreeViewViewModel treeView) return;
        var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
            
        // Получаем полную модель группы из базы данных
        var groupModel = AppDocument.CurrentDatabaseModel.GetGroupById(selectedItem.Item.Id);
                
        // Создаем копию для редактирования (чтобы не изменять оригинал до подтверждения)
        var editGroupModel = new GroupModel(groupModel.Id, groupModel.Pid, groupModel.Title);
                
        // Создаем контент на основе модели
        var content = ModelContentBuilder.CreateContent(editGroupModel);
                
        // Показываем диалоговое окно
        var result = await DialogWindow.ShowAsync(
            content: content,
            title: "Введите название папки",
            width: 320,
            height: 150
        );
                
        if (result)
        {
            // Сохраняем изменения
            AppDocument.CurrentDatabaseModel.EditGroup(editGroupModel.Id, editGroupModel.Title);
        }
    }

    public int Order => 20;
    public MenuItem? ReturnMenuItem(object? mainObj, object? obj)
    {
        if (mainObj is CatalogTreeViewViewModel uc && obj is TreeNode { Item: IGroup } tn)
        {
            return new MenuItem()
            {
                Header = "Редактировать папку",
                Command = Execute,
                CommandParameter = uc,
                IsEnabled = IsEnabledMenu(tn)
            };
        }
        return null;
    }
}
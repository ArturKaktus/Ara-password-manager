using System;
using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using APM.Desktop.Features.CatalogTreeView;
using Avalonia.Controls;
using System.Windows.Input;

namespace APM.Desktop.Features.ContextMenuControls
{
    internal class NewGroupContextMenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 10;

        public bool CanExecute(object parameter) => parameter is TreeNode { Item: IGroup };

        public async void Exec(object parameter)
        {
            try
            {
                if (parameter is not CatalogTreeViewViewModel treeView) return;
                var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
                
                // Создаем модель для новой группы
                var newGroupModel = new GroupModel(0, selectedItem.Item.Id, "Новая папка");
                
                // Создаем контент на основе модели
                var content = ModelContentBuilder.CreateContent(newGroupModel);
                
                // Показываем диалоговое окно
                var result = await DialogWindow.ShowAsync(
                    content: content,
                    title: "Введите название папки",
                    width: 320,
                    height: 150
                );
                
                if (!result) return;
                
                // Создаем группу с введенным названием
                var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(selectedItem.Item.Id, newGroupModel.Title);
                treeView.AddGroupToTreeNode(selectedItem, newGroup);
            }
            catch (Exception e)
            {
                e.Show("Контекстное меню Новая папка.");
            }
        }

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem? ReturnMenuItem(object? mainObj, object? obj)
        {
            if (mainObj is CatalogTreeViewViewModel uc && obj is TreeNode { Item: IGroup } tn)
            {
                return new MenuItem()
                {
                    Header = "Новая папка",
                    Command = Execute,
                    CommandParameter = uc,
                    IsEnabled = IsEnabledMenu(tn)
                };
            }
            return null;
        }
    }
}

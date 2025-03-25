using System;
using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Desktop.Features.CatalogTreeView;
using APM.Desktop.Features.CatalogTreeView.Controls.NewGroup;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
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
                var context = new NewGroupView();
                var result = Application.Current != null && await WindowManager.ShowConfirmDialog(Application.Current, context, "Введите название папки", 150, 320);
                if (!result) return;
                var folderName = ((NewGroupViewModel)context.DataContext!)?.FolderName;
                if (folderName == null) return;
                var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(selectedItem.Item.Id, folderName);
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

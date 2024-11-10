using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Main.Devices.CryptoKakadu.Controls.OpenPinCode;
using APM.Main.Features.CatalogTreeView;
using APM.Main.Features.CatalogTreeView.Controls.NewGroup;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class NewGroupContextMenu : IContextMenu
    {
        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 10;

        public bool CanExecute(object parameter) => parameter is TreeNode tn && tn.Item is IGroup;

        public async void Exec(object parameter)
        {
            if (parameter is CatalogTreeViewViewModel treeView)
            {
                var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
                if (selectedItem is TreeNode tn)
                {
                    var context = new NewGroupView();
                    var result = await WindowManager.ShowConfirmDialog(Application.Current, context, "Введите название папки", 150, 320);
                    if (result)
                    {
                        var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(tn.Item.Id, ((NewGroupViewModel)context.DataContext).FolderName);
                        treeView.AddGroupToTreeNode(tn, newGroup);
                    }
                }
            }
        }

        public bool IsEnabledMenu(object parameter) => true;

        public MenuItem ReturnMenuItem(object? mainObj, object? obj)
        {
            if (mainObj is CatalogTreeViewViewModel uc && obj is TreeNode tn && tn.Item is IGroup)
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

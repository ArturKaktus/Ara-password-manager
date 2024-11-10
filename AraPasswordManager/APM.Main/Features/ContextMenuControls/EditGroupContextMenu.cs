using System.Threading.Tasks;
using System.Windows.Input;
using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Main.Features.CatalogTreeView;
using APM.Main.Features.CatalogTreeView.Controls.NewGroup;
using Avalonia;
using Avalonia.Controls;

namespace APM.Main.Features.ContextMenuControls;

public class EditGroupContextMenu : IContextMenu
{
    public bool CanExecute(object parameter) => parameter is TreeNode { Item: IGroup };

    public bool IsEnabledMenu(object parameter) => parameter is TreeNode { Item: IGroup } tn && tn.Item.Id != 1;

    public ICommand? Execute => new DelegateCommand(Exec);

    public async void Exec(object parameter)
    {
        if (parameter is CatalogTreeViewViewModel treeView)
        {
            var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
            if (selectedItem is TreeNode tn)
            {
                var context = new NewGroupView(tn.Item.Title);
                var result = await WindowManager.ShowConfirmDialog(Application.Current, context, "Введите название папки", 150, 320);
                if (result)
                {
                    AppDocument.CurrentDatabaseModel.EditGroup(tn.Item.Id, ((NewGroupViewModel)context.DataContext).FolderName);
                }
            }
        }
    }

    public int Order => 20;
    public MenuItem ReturnMenuItem(object? mainObj, object? obj)
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
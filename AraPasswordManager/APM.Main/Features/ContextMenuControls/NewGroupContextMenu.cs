using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Main.Features.CatalogTreeView;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenuControls
{
    internal class NewGroupContextMenu : IContextMenu
    {
        public string Title => "Новая папка";

        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 10;

        public bool CanExecute(object parameter) => parameter is TreeNode tn && tn.Item is IGroup;

        public void Exec(object parameter)
        {
            if (parameter is CatalogTreeViewViewModel treeView)
            {
                var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
                if (selectedItem is TreeNode tn)
                {
                    var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(tn.Item.Id, "New Folder");
                    treeView.AddGroupToTreeNode(tn, newGroup);
                }
            }
        }

        public bool IsEnabledMenu(object parameter) => true;
    }
}

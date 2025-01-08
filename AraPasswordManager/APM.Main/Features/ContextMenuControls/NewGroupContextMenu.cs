﻿using System;
using APM.Core;
using APM.Core.Models.Interfaces;
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

        public bool CanExecute(object parameter) => parameter is CatalogTreeViewViewModel or TreeNode { Item: IGroup };

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
                var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(selectedItem == null ? 0 : selectedItem.Item.Id, folderName);
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
            if (mainObj is CatalogTreeViewViewModel uc)
            {
                return new MenuItem()
                {
                    Header = "Новая папка",
                    Command = Execute,
                    CommandParameter = uc
                };
            }
            return null;
        }
    }
}

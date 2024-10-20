﻿using APM.Core;
using APM.Core.Models.Interfaces;
using APM.Main.Features.CatalogTreeView;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace APM.Main.Features.ContextMenu
{
    internal class DeleteGroupContextMenu : IContextMenu
    {
        public string Title => "Удалить папку";

        public ICommand? Execute => new DelegateCommand(Exec);

        public int Order => 20;

        public bool CanExecute(object parameter) => parameter is TreeNode tn && tn.Item is IGroup;

        public void Exec(object parameter)
        {
            if (parameter is CatalogTreeViewViewModel treeView)
            {
                var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
                List<int> groudpIds = new();
                List<int> recordIds = new();
                if (selectedItem is TreeNode tn)
                {
                    groudpIds.Add(tn.Item.Id);
                    AddToLists(tn.Item.Id, groudpIds, recordIds);
                    AppDocument.CurrentDatabaseModel.DeleteRecordsById(recordIds);
                    AppDocument.CurrentDatabaseModel.DeleteGroupsById(groudpIds);
                    treeView.DeleteGroupInTreeNode(tn);
                }
            }
        }

        public bool IsEnabledMenu(object parameter) => parameter is TreeNode tn && tn.Item.Id != 1;

        private static void AddToLists(int id, List<int> groupsList, List<int> recordsList)
        {
            var records = AppDocument.CurrentDatabaseModel.GetRecordsByPid(id);
            var groups = AppDocument.CurrentDatabaseModel.GetGroupsByPid(id);

            foreach (var rec in records)
            {
                recordsList.Add(rec.Id);
            }

            foreach (var gr in groups)
            {
                groupsList.Add(gr.Id);
                AddToLists(gr.Id, groupsList, recordsList);
            }
        }
    }
}

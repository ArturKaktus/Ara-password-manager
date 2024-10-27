using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APM.Main.Features.CatalogTable;

public class CatalogTableViewModel
{
    public ObservableCollection<RecordModel> Records { get; }
    public IRecord SelectedRecord { get; set; }
    public TreeNode SelectedTreeNode {  get; set; }

    public CatalogTableViewModel()
    {
        Records = [];
        AppDocument.NodeTransfer.PropertyChanged += TreeNodeSelected_PropertyChanged;
    }

    public void RefreshTable()
    {
        Records.Clear();
        if (SelectedTreeNode != null)
        {
            var listRecords = AppDocument.CurrentDatabaseModel.GetRecordsByPid(SelectedTreeNode.Item.Id);
            foreach (var a in listRecords)
            {
                Records.Add(a);
            }
        }
    }

    private void TreeNodeSelected_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is TreeNodeTransfer tnt)
        {
            SelectedTreeNode = tnt.SelectedTreeNode;
            RefreshTable();
        }
    }
}
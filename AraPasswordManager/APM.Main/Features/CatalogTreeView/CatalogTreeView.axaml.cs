using APM.Core;
using APM.Main;
using APM.Main.Features.CatalogTreeView;
using Avalonia.Controls;
using System.Collections.Generic;

namespace Ara_password_manager.Features.CatalogTreeView;

public partial class CatalogTreeView : UserControl
{
    private CatalogTreeViewViewModel viewModel = new();
    public CatalogTreeView()
    {
        this.DataContext = viewModel;
        InitializeComponent();
    }
    private void TreeView_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
    {
        TreeNode selectedItem;
        if (sender is TreeView tw)
        {
            AppDocument.NodeTransfer.SelectedTreeNode = tw.SelectedItem as TreeNode;
        }
    }

    private void AddItem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedItem = CatalogTree.SelectedItem;
        if (selectedItem is TreeNode tn)
        {
            var newGroup = AppDocument.CurrentDatabaseModel.AddGroup(tn.Item.Id, "New Folder");
            viewModel.AddGroupToTreeNode(tn, newGroup);
        }
    }

    private void DeleteItem_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var selectedItem = CatalogTree.SelectedItem;
        List<int> groudpIds = new();
        List<int> recordIds = new();
        if (selectedItem is TreeNode tn)
        {
            groudpIds.Add(tn.Item.Id);
            AddToLists(tn.Item.Id, groudpIds, recordIds);
            AppDocument.CurrentDatabaseModel.DeleteRecordsById(recordIds);
            AppDocument.CurrentDatabaseModel.DeleteGroupsById(groudpIds);
            viewModel.DeleteGroupInTreeNode(tn);
        }
    }

    private void AddToLists(int id,List<int> groupsList, List<int> recordsList)
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


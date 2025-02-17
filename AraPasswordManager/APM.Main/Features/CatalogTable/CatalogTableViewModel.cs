using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using APM.Desktop.Features.CatalogTable.Controls.RecordProps;
using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;

namespace APM.Desktop.Features.CatalogTable;

public partial class CatalogTableViewModel : ObservableObject
{
    public ObservableCollection<RecordModel> Records { get; }
    public IRecord SelectedRecord { get; set; }
    
    [ObservableProperty] private TreeNode _SelectedTreeNode;
    [ObservableProperty] private bool _IsEnabledCreateButton;
    public CatalogTableViewModel()
    {
        Records = [];
        AppDocument.NodeTransfer.PropertyChanged += TreeNodeSelected_PropertyChanged;
    }

    public async void CreateRecordCommamd()
    {
        var selectedItem = AppDocument.NodeTransfer.SelectedTreeNode;
        if (selectedItem is TreeNode tn)
        {
            var context = new RecordPropsView();
            var result = await WindowManager.ShowConfirmDialog(Application.Current, context, "Создание записи", 150, 320);
            if (result)
            {
                AppDocument.CurrentDatabaseModel.AddRecord(tn.Item.Id, ((RecordPropsViewModel)context.DataContext).Record);
                RefreshTable();
            }
        }
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
            IsEnabledCreateButton = true;
        }
        else
        {
            IsEnabledCreateButton = false;
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
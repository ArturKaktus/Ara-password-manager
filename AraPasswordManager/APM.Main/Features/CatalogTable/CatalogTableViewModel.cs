using System.Collections.ObjectModel;
using System.ComponentModel;
using APM.Core;
using APM.Core.Models;
using APM.Core.Models.Interfaces;

namespace APM.Main.Features.CatalogTable;

public class CatalogTableViewModel
{
    public ObservableCollection<RecordModel> Records { get; }

    public CatalogTableViewModel()
    {
        Records = [];
        AppDocument.NodeTransfer.PropertyChanged += TreeNodeSelected_PropertyChanged;
    }

    private void TreeNodeSelected_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        //Наполнение таблицы
        Records.Clear();
        var tnt = sender as TreeNodeTransfer;
        var db = AppDocument.CurrentDatabaseModel;
        if (tnt.SelectedTreeNode != null)
        {
            var asd = db.GetRecordsByPid(tnt.SelectedTreeNode.Item.Id);
            foreach (var a in asd)
            {
                Records.Add(a);
            }
        }
        
    }
}
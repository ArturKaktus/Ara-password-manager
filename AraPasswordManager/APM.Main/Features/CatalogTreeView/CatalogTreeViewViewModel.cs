using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using APM.Core;
using APM.Core.Models;
using Ara_password_manager;

namespace APM.Main.Features.CatalogTreeView;

public class CatalogTreeViewViewModel
{
    public ObservableCollection<TreeNode> TreeNodes { get; }

    public CatalogTreeViewViewModel()
    {
        TreeNodes = [];
        AppDocument.CurrentDatabaseModel.PropertyChanged += TreeObjets_PropertyChanged;
    }
    private void TreeObjets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "UpdateDatabase") return;
        TreeNodes.Clear();
        TreeNodes.Add(CreateTree(1));
    }
    private static TreeNode CreateTree(int startId)
    {
        TreeNode treeObject = new();
        var db = AppDocument.CurrentDatabaseModel;
        //Выбор нулевой группы
        treeObject.Item = db.GetGroupsById(startId);

        //Сбор групп
        var groups = db.GetGroupsByPid(startId);
        foreach (var groupModel1 in groups)
        {
            if (treeObject.Item != null)
            {
                treeObject.Child.Add(CreateTree(groupModel1.Id));
            }
        }

        //Сбор записей 
        //Удалено, так как записи не должны быть в ветви
        //var recordModels = db.GetRecordsByPid(startId);
        //foreach (var recordModel in recordModels)
        //{
        //    treeObject.Child.Add(new TreeNode() { Item = recordModel });
        //}

        return treeObject;
    }
}

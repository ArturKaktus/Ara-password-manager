using APM.Core;
using APM.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using APM.Core.Models.Interfaces;

namespace APM.Desktop.Features.CatalogTreeView;

public class CatalogTreeViewViewModel
{
    public ObservableCollection<TreeNode> TreeNodes { get; }

    public CatalogTreeViewViewModel()
    {
        TreeNodes = [];
        CreateRootFolder();
        AppDocument.CurrentDatabaseModel.PropertyChanged += TreeObjets_PropertyChanged;
    }
    private void TreeObjets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "UpdateDatabase") return;
        RefreshTree();
    }

    public void CreateRootFolder()
    {
        TreeNode treeObject = new();
        var group = AppDocument.CurrentDatabaseModel.AddGroup(0, "Корневая папка");
        treeObject.Item = group;
        TreeNodes.Add(treeObject);
    }
    
    public void RefreshTree()
    {
        TreeNodes.Clear();
        TreeNodes.Add(CreateTree(1));
    }
    public void AddGroupToTreeNode(TreeNode treeNode, IGroup group)
    {
        TreeNode treeObject = new();
        treeObject.ParentNode = treeNode;
        treeObject.Item = group;
        treeNode.Child.Add(treeObject);
    }
    public void DeleteGroupInTreeNode(TreeNode treeNode)
    {
        var parent = treeNode.ParentNode;
        parent.Child.Remove(treeNode);
    }
    private static TreeNode CreateTree(int startId, TreeNode? parentTreeNode = null)
    {
        TreeNode treeObject = new();
        var db = AppDocument.CurrentDatabaseModel;
        if (db.GroupsArrayList.Count == 0)
            AppDocument.CurrentDatabaseModel.AddGroup(0, "Корневая папка");
        //Выбор нулевой группы
        treeObject.Item = db.GetGroupById(startId);
        if(parentTreeNode != null)
            treeObject.ParentNode = parentTreeNode;

        //Сбор групп
        var groups = db.GetGroupsByPid(startId);
        foreach (var groupModel in groups)
        {
            if (treeObject.Item != null)
            {
                treeObject.Child.Add(CreateTree(groupModel.Id, treeObject));
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

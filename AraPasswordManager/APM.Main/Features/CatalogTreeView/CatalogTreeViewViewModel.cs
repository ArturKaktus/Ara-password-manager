using APM.Core;
using APM.Core.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace APM.Main.Features.CatalogTreeView;

public class CatalogTreeViewViewModel
{
    public ObservableCollection<TreeNode> TreeNodes { get; }

    public CatalogTreeViewViewModel()
    {
        TreeNodes = [];
        TreeNode treeObject = new();
        treeObject.Item = new GroupModel(1, 0, "Корневая папка");
        TreeNodes.Add(treeObject);
        AppDocument.CurrentDatabaseModel.PropertyChanged += TreeObjets_PropertyChanged;
    }
    private void TreeObjets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName != "UpdateDatabase") return;
        RefreshTree();
    }

    public void RefreshTree()
    {
        TreeNodes.Clear();
        TreeNodes.Add(CreateTree(1));
    }
    public void AddGroupToTreeNode(TreeNode treeNode, GroupModel group)
    {
        TreeNode treeObject = new();
        treeObject.ParentNode = treeNode;
        treeObject.Item = group;
        if (treeNode == null)
            TreeNodes.Add(treeObject);
        else
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

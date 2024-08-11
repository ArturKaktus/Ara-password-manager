using System.ComponentModel;
using System.Text.Json.Nodes;

namespace APM.Core.Models;

public class DatabaseModel
{
    private List<GroupModel> _groupsArrayList = [];
    private List<RecordModel> _recordsArrayList = [];

    public void FillLists(List<GroupModel> group, List<RecordModel> record)
    {
        _groupsArrayList = group;
        _recordsArrayList = record;
        OnPropertyChanged("UpdateDatabase");
    }
    public void Clear()
    {
        if (this._groupsArrayList.Count > 0)
            this._groupsArrayList.Clear();
        if (this._recordsArrayList.Count > 0)
            this._recordsArrayList.Clear();
    }
    public GroupModel GetGroupsById(int id)
    {
        return this._groupsArrayList.FirstOrDefault(groupModel => groupModel.Id == id)!;
    }
    public List<GroupModel> GetGroupsByPid(int id)
    {
        List<GroupModel> groupModelsById = [];
        groupModelsById.AddRange(this._groupsArrayList.Where(groupModel => groupModel.Pid == id));
        return groupModelsById;
    }
    public List<RecordModel> GetRecordsByPid(int groupId)
    {
        List<RecordModel> recordModel = [];
        recordModel.AddRange(this._recordsArrayList.Where(record => record.Pid == groupId));
        return recordModel;
    }
    public List<GroupModel> GroupsArrayList => _groupsArrayList;
    public List<RecordModel> RecordsArrayList => _recordsArrayList;


    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
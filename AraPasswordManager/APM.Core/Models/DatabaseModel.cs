using APM.Core.Utils;
using System.ComponentModel;

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
    public RecordModel GetRecordById(int id)
    {
        return this._recordsArrayList.FirstOrDefault(recordModel => recordModel.Id == id)!;
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

    public GroupModel AddGroup(int pid, string title)
    {
        int maxId = _groupsArrayList.Max(obj => obj.Id);
        var group = new GroupModel(maxId + 1, pid, title);
        _groupsArrayList.Add(group);
        return group;
    }

    public void DeleteGroupsById(List<int> ids)
    {
        foreach(var id in ids)
        {
            var group = GetGroupsById(id);
            _groupsArrayList.Remove(group);
        }
    }
    public void DeleteRecordsById(List<int> ids)
    {
        foreach(var id in ids)
        {
            var record = GetRecordById(id);
            _recordsArrayList.Remove(record);
        }
    }

    public void DeleteRecordById(int id)
    {
        var record = GetRecordById(id);
        _recordsArrayList.Remove(record);
    }

    public List<GroupModel> GroupsArrayList => _groupsArrayList; 
    public List<RecordModel> RecordsArrayList => _recordsArrayList;

    public void FillFromDevice(byte[,] buffer)
    {
        int length = buffer.Length;
        this.Clear();
        //Name = "device.kkd";
        byte[] tempBytes = new byte[48];
        _groupsArrayList.Add(new GroupModel(1, 0, "Корневая папка"/*this.bundle.GetString("main.pane.root")*/));
        byte[] tempName = new byte[48];
        byte[] tempLogin = new byte[48];
        byte[] tempPassword = new byte[48];
        byte[] tempUrl = new byte[48];
        var count = buffer.GetLength(0);
        for (int i = 0; i < count; i++)
        {
            byte[] bs = buffer.GetRow(i);
            string name;
            int pid;
            int id;
            if (bs[52] == 0)
            {
                id = ToDecFromBytes(bs[0], bs[1]) + 1;
                pid = ToDecFromBytes(bs[2], bs[3]) + 1;
                Array.Copy(bs, 4, tempBytes, 0, 48);
                byte[] nameBytes = DeleteZerosFromArray(tempBytes);
                try
                {
                    name = ByteUtils.ByteToUtf8String(nameBytes);
                    _groupsArrayList.Add(new GroupModel(id, pid, name));
                }
                catch (Exception e)
                {
                    //Console.Error.WriteLine(e);
                }
                continue;
            }
            id = ToDecFromBytes(bs[0], bs[1]) + 1;
            pid = ToDecFromBytes(bs[2], bs[3]) + 1;
            Array.Copy(bs, 4, tempName, 0, 48);
            Array.Copy(bs, 52, tempLogin, 0, 48);
            Array.Copy(bs, 100, tempPassword, 0, 48);
            Array.Copy(bs, 148, tempUrl, 0, 48);
            string afterLoginString = GetSymbolFromByteArray(tempLogin);
            string afterPasswordString = GetSymbolFromByteArray(tempPassword);
            string afterUrlString = GetSymbolFromByteArray(tempUrl);
            try
            {
                name = ByteUtils.ByteToUtf8String(DeleteZerosFromArray(tempName));
                string login = ByteUtils.ByteToUtf8String(DeleteZerosFromArray(tempLogin));
                string password = ByteUtils.ByteToUtf8String(DeleteZerosFromArray(tempPassword));
                string url = ByteUtils.ByteToUtf8String(DeleteZerosFromArray(tempUrl));
                _recordsArrayList.Add(new RecordModel(id, pid, name, login, StringToCharArray(password), url, afterLoginString, afterPasswordString, afterUrlString));
            }
            catch /*(UnsupportedEncodingException e)*/
            {
                //Console.Error.WriteLine(e);
            }
        }
        OnPropertyChanged("UpdateDatabase");
    }
    private static char[] StringToCharArray(string value)
    {
        return value.ToCharArray();
    }
    private string GetSymbolFromByteArray(byte[] byteArray)
    {
        string symbol = "NONE";
        foreach (byte b in byteArray)
        {
            if (b == 9)
            {
                symbol = "TAB";
            }
            if (b == 10)
            {
                symbol = "ENTER";
            }
        }
        return symbol;
    }
    public int ToDecFromBytes(byte b1, byte b2)
    {
        byte[] byteArray = { b1, b2 };

        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(byteArray);
        }

        short shortVal = BitConverter.ToInt16(byteArray, 0);
        int x = shortVal;

        return x;
    }
    private byte[] DeleteZerosFromArray(byte[] byteArr)
    {
        int count = 0;
        foreach (byte b in byteArr)
        {
            if (b != 0 && b != 9 && b != 10)
            {
                count++;
            }
        }
        byte[] noZeros = new byte[count];
        for (int i = 0, j = 0; i < byteArr.Length; i++)
        {
            if (byteArr[i] != 0 && byteArr[i] != 9 && byteArr[i] != 10)
            {
                noZeros[j] = byteArr[i];
                j++;
            }
        }
        return noZeros;
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
public static class ArrayExtensions
{
    public static T[] GetRow<T>(this T[,] data, int i)
    {
        return Enumerable.Range(0, data.GetLength(1)).Select(j => data[i, j]).ToArray();
    }
}
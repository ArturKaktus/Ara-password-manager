using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Resources;
using Avalonia.Markup.Xaml.Templates;
using System.Diagnostics;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using CryptoUSB.Controllers;
using System.Collections.ObjectModel;
using Avalonia.LogicalTree;
using System.ComponentModel;
using CryptoUSB.Models.Interfaces;
using Avalonia.Controls.Shapes;
using CryptoUSB.Utils;

namespace CryptoUSB.Models
{
    public class DatabaseModel : INotifyPropertyChanged
    {
        private ObservableCollection<TreeObject> _TreeObjects = new();
        public string Name { get; set; } = string.Empty;
        private readonly List<GroupModel> groupsArrayList = new();
        private readonly List<RecordModel> recordsArrayList = new();
        private readonly List<GroupModel> groupsBreadList = new();

        private void GroupItem_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {

        }
        public ObservableCollection<TreeObject> TreeObjects 
        { 
            get => _TreeObjects;
            set
            {
                _TreeObjects = value;
                OnPropertyChanged(nameof(TreeObjects));
            }
        }
        public bool Saved { get; set; } = true;
        private int groupsHashCode = 0;
        private int recordsHashCode = 0;
        public static DatabaseModel Instance = new();
        private readonly ResourceManager bundle;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private DatabaseModel()
        {
            this.bundle = LanguageController.INSTANCE.GetAppLanguageBundle();
            //CreateNewDatabase("New database");
            HashDatabase();
        }
        public bool IsSaved()
        {
            return this.groupsArrayList.GetHashCode() == this.groupsHashCode
                && this.recordsArrayList.GetHashCode() == this.recordsHashCode;
        }
        public void HashDatabase()
        {
            this.groupsHashCode = this.groupsArrayList.GetHashCode();
            this.recordsHashCode = this.recordsArrayList.GetHashCode();
        }
        public bool IsEmpty()
        {
            return Name == null || this.Name.Length == 0;
        }
        public bool HasRows()
        {
            return (this.groupsArrayList.Count > 1 || this.recordsArrayList.Count > 0)
                && (this.groupsArrayList.Count > 0 || this.recordsArrayList.Count > 0);
        }
        public void Clear()
        {
            Name = null;
            if (this.groupsArrayList.Count>0)
                this.groupsArrayList.Clear();
            if (this.recordsArrayList.Count>0)
                this.recordsArrayList.Clear();
        }
        public void CreateNewDatabase(string databaseName)
        {
            if (databaseName.Length > 0)
            {
                Clear();
                Name = databaseName + ".kkd";
                this.groupsArrayList.Add(new GroupModel(1, 0, this.bundle.GetString("main.pane.root")));
            }
            else
            {

            }
        }
        public int GetLastGroupID()
        {
            int lastId = 0;
            foreach (GroupModel groupModel in this.groupsArrayList)
            {
                if (lastId < groupModel.Id)
                    lastId = groupModel.Id;
            }
            return lastId;
        }
        public int GetLastRecordID()
        {
            int lastId = 0;
            foreach (RecordModel recordModel in this.recordsArrayList)
            {
                if (lastId < recordModel.Id)
                    lastId = recordModel.Id;
            }
            return lastId;
        }
        public void AddGroup(int pidInt, string nameString)
        {
            int lastId = GetLastGroupID();
            int nameCount = 0;
            bool contin = true;
            string tempString = string.Empty;
            if (lastId != 0)
            {
                List<GroupModel> temp = GetGroupsByPid(pidInt);
                foreach (GroupModel groupModel in temp) 
                {
                    if (groupModel.Name.Equals(nameString))
                    {
                        nameCount++;
                    }
                }
                if (nameCount > 0)
                {
                    string newString = nameString + " (" + nameCount + ")";
                    while (contin) 
                    {
                        if (!tempString.Equals(newString))
                        {
                            tempString = newString;
                            if (tempString.Length < 48)
                            {
                                foreach(GroupModel groupModel in temp)
                                {
                                    if (groupModel.Name.Equals(tempString))
                                    {
                                        nameCount++;
                                        newString = newString + " (" + nameCount + ")";
                                    }

                                }
                                continue;
                            }
                            contin = false;
                            tempString = newString;
                            continue;
                        }
                        contin = false;
                    }
                    this.groupsArrayList.Add(new GroupModel(lastId + 1, pidInt, tempString));
                }
                else
                {
                    this.groupsArrayList.Add(new GroupModel(lastId + 1, pidInt, tempString));
                }
            }
        }
        public void AddRecord(int pid, string name, string login, string password, string url, string afterLogin, string afterPassword, string afterUrl)
        {
            int lastId = GetLastRecordID();
            this.recordsArrayList.Add(new RecordModel(lastId + 1, pid, name, login, password.ToCharArray(), url, afterLogin, afterPassword, afterUrl));
        }
        public void EditRecord(int id, int pid, string name, string login, string password, string url, string afterLogin, string afterPassword, string afterUrl)
        {
            foreach(RecordModel recordModel in this.recordsArrayList)
            {
                if (recordModel.Id == id)
                {
                    recordModel.Name = name;
                    recordModel.Login = login;
                    recordModel.Password = password.ToCharArray();
                    recordModel.Url = url;
                    recordModel.SetAfterLoginSymbol(afterLogin);
                    recordModel.SetAfterPasswordSymbol(afterPassword);
                    recordModel.SetAfterUrlSymbol(afterUrl);
                }
            }
        }
        public void EditGroup(int id, int pid, string name)
        {
            foreach(GroupModel groupModel in this.groupsArrayList)
            {
                if (groupModel.Id == id) 
                {
                    groupModel.Pid = pid;
                    groupModel.Name = name;
                }
            }
        }
        public void DeleteGroupById(int id)
        {
            for (int i = 0; i < this.groupsArrayList.Count; i++)
            {
                if (this.groupsArrayList[i].Id == id)
                {
                    if (this.recordsArrayList.Count > 0)
                    {
                        DeleteRecordsByPid(id);
                    }
                    DeleteGroupChild(id);
                    this.groupsArrayList.RemoveAt(i);
                }
            }
            this.groupsArrayList.TrimExcess();
            if (this.recordsArrayList.Count > 0)
            {
                NormalizeGroups();
            }
        }
        public void CopyGroupById(int id, int newPid)
        {
            AddGroup(newPid, GetGroupById(id).Name);
            int lastIndex = GetLastGroupID();
            CopyRecords(id, lastIndex);
            RecursiveCopyGroups(id, lastIndex, new List<int>());
        }
        public void RecursiveCopyGroups(int pid, int newPid, List<int> integers)
        {
            List<GroupModel> list = GetGroupsByPid(pid);
            foreach(GroupModel groupModel in list)
            {
                if (integers.Contains(groupModel.Id))
                {
                    AddGroup(newPid, groupModel.Name);
                    int tempPid = GetLastGroupID();
                    integers.Add(groupModel.Id);
                    RecursiveCopyGroups(groupModel.Id, tempPid, integers);
                    CopyRecords(groupModel.Id, tempPid);
                }
            }
        }
        private void CopyRecords(int pid, int newPid)
        {
            List<RecordModel> list = GetRecordsByPid(pid);
            foreach(RecordModel recordModel in list) 
            {
                AddRecord(newPid, recordModel.Name, 
                    recordModel.Login, 
                    recordModel.GetPasswordString(), 
                    recordModel.Url, 
                    recordModel.GetAfterLoginString(), 
                    recordModel.GetAfterPasswordString(), 
                    recordModel.GetAfterUrlString());
            }
        }
        private List<GroupModel> GetChildGroupArray(int pid) 
        {
            List<GroupModel> list = new();
            foreach (GroupModel groupModel in this.groupsArrayList)
            {
                if (groupModel.Pid == pid)
                {
                    list.Add(groupModel);
                    List<GroupModel> temp = GetChildGroupArray(groupModel.Id);
                    if (temp.Count > 0)
                    {
                        list.AddRange(temp);
                    }
                }
            }
            return list;
        }
        private void DeleteGroupChild(int pid)
        {
            List<GroupModel> temp = new();
            for (int i = 0; i < this.groupsArrayList.Count; i++)
            {
                if (this.groupsArrayList[i].Pid == pid)
                {
                    temp.Add(this.groupsArrayList[i]);
                    int id = this.groupsArrayList[i].Id;
                    if (this.recordsArrayList.Count > 0)
                    {
                        DeleteRecordsByPid(id);
                    }
                    DeleteGroupChild(id);
                }
            }
            this.groupsArrayList.RemoveAll(item => temp.Contains(item));
            this.groupsArrayList.TrimExcess();
        }
        public void DeleteRecordById(int id)
        {
            for (int i = 0; i < this.recordsArrayList.Count; i++)
            {
                if (this.recordsArrayList[i].Id == id)
                {
                    this.recordsArrayList.RemoveAt(i);
                }
            }
            this.recordsArrayList.TrimExcess();
            if (this.recordsArrayList.Count > 0)
            {
                NormalizeRecords();
            }
        }
        public void DeleteRecordsByPid(int pid)
        {
            List<RecordModel> temp = new();
            for (int i = 0; i < this.recordsArrayList.Count; i++)
            {
                if (this.recordsArrayList[i].Pid == pid)
                {
                    temp.Add(this.recordsArrayList[i]);
                }
            }
            this.recordsArrayList.RemoveAll(item => temp.Contains(item));
            this.recordsArrayList.TrimExcess();
            if (this.recordsArrayList.Count > 0)
            {
                NormalizeRecords();
            }
        }
        public void SetRecordPidById(int id, int pid)
        {
            this.recordsArrayList.ForEach(recordModel =>
            {
                if (recordModel.Id == id)
                {
                    recordModel.Pid = pid;
                }
                else
                {
                    //не найдено
                }
            });
        }
        public void SetGroupPidById(int id, int pid)
        {
            this.groupsBreadList.ForEach(groupModel => { 
                if (groupModel.Id == id)
                {
                    groupModel.Pid = pid;
                }
                else
                {
                    //не найдено
                }
            });
        }
        private void NormalizeRecords()
        {
            this.recordsArrayList[0].Id = 1;
            for (int i = 0; i < this.recordsArrayList.Count - 1; i++)
            {
                if (this.recordsArrayList[i].Id + 1 != this.recordsArrayList[i + 1].Id)
                {
                    this.recordsArrayList[i + 1].Id = (this.recordsArrayList[i].Id + 1);
                }
            }
        }
        private void NormalizeGroups()
        {
            for (int i = 0; i < this.groupsArrayList.Count - 1; i++)
            {
                if (this.groupsArrayList[i].Id + 1 != this.groupsArrayList[i + 1].Id)
                {
                    int id = this.groupsArrayList[i + 1].Id;
                    this.groupsArrayList[i + 1].Id = (this.groupsArrayList[i].Id + 1);
                    foreach (RecordModel recordModel in this.recordsArrayList)
                    {
                        if (recordModel.Pid == id)
                        {
                            recordModel.Pid = this.groupsArrayList[i + 1].Id;
                        }
                    }
                }    
            }
        }
        public void FillFromFoxJson(string json)
        {

        }
        public void FillFromKakaduJSON(string json)
        {
            Clear();

            JsonNode? obj = JsonObject.Parse(json);
            JsonObject jsonObj = (JsonObject)obj;
            JsonArray groupArray = (JsonArray)jsonObj["groups"];
            int size = groupArray.Count;
            for (int i = 0; i < size; i++)
            {
                JsonObject groupObj = (JsonObject)groupArray[i];
                this.groupsArrayList.Add(new GroupModel(
                    Convert.ToInt32(groupObj["id"].ToString()),
                    Convert.ToInt32(groupObj["pid"].ToString()),
                    groupObj["name"].ToString()));
            }

            JsonArray recordArray = (JsonArray)jsonObj["records"];
            size = recordArray.Count;
            for (int i = 0; i < size; i++)
            {
                JsonObject recordObj = (JsonObject)recordArray[i];
                this.recordsArrayList.Add(new RecordModel(
                    Convert.ToInt32(recordObj["id"].ToString()),
                    Convert.ToInt32(recordObj["pid"].ToString()),
                    recordObj["name"].ToString(),
                    recordObj["login"].ToString(),
                    StringToCharArray(recordObj["password"].ToString()),
                    recordObj["url"].ToString(),
                    recordObj["loginSymbol"].ToString(),
                    recordObj["passwordSymbol"].ToString(),
                    recordObj["urlSymbol"].ToString()));
            }
        }
        public void FillFromDevice(byte[,] buffer)
        {
            int length = buffer.Length;
            this.Clear();
            Name = "device.kkd";
            byte[] tempBytes = new byte[48];
            this.groupsArrayList.Add(new GroupModel(1, 0, "ТЕСТ"/*this.bundle.GetString("main.pane.root")*/));
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
                        this.groupsArrayList.Add(new GroupModel(id, pid, name));
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
                    this.recordsArrayList.Add(new RecordModel(id, pid, name, login, StringToCharArray(password), url, afterLoginString, afterPasswordString, afterUrlString));
                }
                catch /*(UnsupportedEncodingException e)*/
                {
                    //Console.Error.WriteLine(e);
                }
            }
        }
        string HexStringToString(string HexString)
        {
            string stringValue = "";
            for (int i = 0; i < HexString.Length / 2; i++)
            {
                string hexChar = HexString.Substring(i * 2, 2);
                int hexValue = Convert.ToInt32(hexChar, 16);
                stringValue += Char.ConvertFromUtf32(hexValue);
            }
            return stringValue;
        }
        byte[] ConvertWin1251ToUtf8(byte[] hex)
        {
            byte[] returnHex = new byte[hex.Length*2];
            int ii = 0;
            for (int i = 0; i<hex.Length; i++)
            {
                if (hex[i] >= 192 && hex[i] <= 239)
                {
                    returnHex[ii] = 208;
                    returnHex[ii + 1] = (byte)(hex[i] - 48);
                }
                else if (hex[i] >= 240 && hex[i] <= 255)
                {
                    returnHex[ii] = 209;
                    returnHex[ii + 1] = (byte)(hex[i] - 112);
                }
                else if (hex[i] == 168)
                {
                    returnHex[ii] = 208;
                    returnHex[ii + 1] = 129;
                }
                else if (hex[i] == 184)
                {
                    returnHex[ii] = 209;
                    returnHex[ii + 1] = 145;
                }
                ii += 2;
            }
            return returnHex;
        }


        public void BuildTree()
        {
            foreach (var group in Instance.groupsArrayList)
            {
                group.PropertyChanged += GroupItem_PropertyChanged;
            }
            ObservableCollection<TreeObject> asd = new ObservableCollection<TreeObject>();
            asd.Add(Instance.CreateTree(1));
            TreeObjects = asd;
        }
        public string GetJSONString()
        {
            JsonArray groupsArray = new();
            JsonArray recordsArray = new();
            JsonObject backupObj = new();
            foreach (GroupModel groupModel in this.groupsArrayList)
            {
                JsonObject groupJSON = new()
                {
                    { "id", groupModel.Id },
                    { "pid", groupModel.Pid },
                    { "name", groupModel.Name }
                };
                groupsArray.Add(groupJSON);
            }
            foreach (RecordModel recordModel in this.recordsArrayList)
            {
                JsonObject recordJSON = new()
                {
                    { "id", recordModel.Id },
                    { "pid", recordModel.Pid },
                    { "name", recordModel.Name },
                    { "login", recordModel.Login },
                    { "password", recordModel.GetPasswordString() },
                    { "url", recordModel.Url },
                    { "loginSymbol", recordModel.GetAfterLoginString() },
                    { "passwordSymbol", recordModel.GetAfterPasswordString() },
                    { "urlSymbol", recordModel.GetAfterUrlString() }
                };
                recordsArray.Add(recordJSON);
            }
            backupObj.Add("groups", groupsArray);
            backupObj.Add("records", recordsArray);
            return backupObj.ToString();
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
        private static List<GroupModel> GetPrepareGroupArray()
        {
            List<GroupModel> newGroupModel = new();

            return newGroupModel;
        }
        private static List<RecordModel> GetPrepareRecordArray()
        {
            List<RecordModel> newRecordModel = new();

            return newRecordModel;
        }
        public byte[,] GetDeveiceArray()
        {
            List<GroupModel> readyGroupModels = GetPrepareGroupArray();
            List<RecordModel> readyRecordModels = GetPrepareRecordArray();
            byte[,] kakaduBytes = new byte[readyGroupModels.Count + readyRecordModels.Count, 196];
            return kakaduBytes;
        }
        private static char[] StringToCharArray(string value)
        {
            return value.ToCharArray();
        }
        public List<GroupModel> GetGroupsByPid(int id)
        {
            List<GroupModel> groupModelsById = new();
            foreach(GroupModel groupModel in this.groupsArrayList)
            {
                if (groupModel.Pid == id)
                {
                    groupModelsById.Add(groupModel);
                }
            }
            return groupModelsById;
        }
        private TreeObject CreateTree (int startId)
        {
            TreeObject treeObject = new();
            //Выбор нулевой группы
            treeObject.Item = groupsArrayList.FirstOrDefault(g => g.Id == startId); ;

            List<GroupModel> groups = GetGroupsByPid(startId);
            foreach (GroupModel groupModel1 in groups)
            {
                if (treeObject.Item != null )
                {
                    treeObject.Children.Add(CreateTree(groupModel1.Id));
                }
            }

            //Сбор записей в группу
            List<RecordModel> recordModels = GetRecordsByPid(startId);
            foreach (RecordModel recordModel in recordModels)
            {
                treeObject.Children.Add(new TreeObject() { Item = recordModel });
            }


            //ObservableCollection<TreeObject> tree = new ObservableCollection<TreeObject>();
            //tree.Add(treeObject);
            return treeObject;
        }
        private string GetSymbolFromByteArray(byte[] byteArray)
        {
            string symbol = "NONE";
            foreach(byte b in byteArray)
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
        public GroupModel GetGroupById(int findId)
        {
            foreach(GroupModel groupModel in this.groupsArrayList)
            {
                if (groupModel.Id == findId)
                {
                    return groupModel;
                }
            }
            return null;
        }
        public RecordModel GetRecordById(int findId)
        {
            foreach (RecordModel recordModel in this.recordsArrayList)
            {
                if (recordModel.Id == findId)
                    return recordModel;
            }
            return null;
        }
        public List<RecordModel> GetRecordsByPid(int groupId)
        {
            List<RecordModel> recordModel = new();
            foreach (RecordModel record in this.recordsArrayList)
            {
                if (record.Pid == groupId)
                {
                    recordModel.Add(record);
                }
            }
            return recordModel;
        }
        public List<GroupModel> GetGroupsByPis(int pid)
        {
            List<GroupModel> groupModels = new();
            foreach (GroupModel groupModel in this.groupsArrayList)
                if (groupModel.Pid == pid)
                    groupModels.Add(groupModel);
            return groupModels;
        }
        private void SetBreadCrumb(int id)
        {
            GroupModel gm = GetGroupById(id);
            this.groupsBreadList.Add(new GroupModel(gm.Id, gm.Pid, gm.Name));
            if (gm.Pid != 0)
                SetBreadCrumb(gm.Pid);
        }
        public List<GroupModel> GetGroupsBreadList(int id)
        {
            this.groupsBreadList.Clear();
            SetBreadCrumb(id);
            this.groupsBreadList.Reverse();
            return this.groupsBreadList;
        }
        public List<GroupModel> GetGroupsArrayList()
        {
            List<GroupModel> newGroup = new(this.groupsArrayList);
            return newGroup;
        }
        public int GetRowCount()
        {
            return this.groupsArrayList.Count + this.recordsArrayList.Count;
        }
        public byte[] GetRowCountByte()
        {
            return IntToDoubleByte(GetRowCount() - 1);
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
        public byte[] IntToDoubleByte(int toByte)
        {
            byte[] doubleByte = new byte[2];
            doubleByte[1] = (byte)toByte;
            doubleByte[0] = (byte)(toByte >> 8);
            return doubleByte;
        }
    }
    public class TreeObject 
    {
        public IObjectModel Item { get; set; }
        public string ImageType { get => Item is GroupModel ? "/Assets/folder.png" : "/Assets/file.png"; }
        public ObservableCollection<TreeObject> Children { get; set; } = new ObservableCollection<TreeObject>();
    }
    public static class ArrayExtensions
    {
        public static T[] GetRow<T>(this T[,] data, int i)
        {
            return Enumerable.Range(0, data.GetLength(1)).Select(j => data[i, j]).ToArray();
        }
    }
}

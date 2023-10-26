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

namespace CryptoUSB.Models
{
    public class DatabaseModel
    {
        private string Name { get; set; } = string.Empty;
        private List<GroupModel> groupsArrayList = new List<GroupModel>();
        private List<RecordModel> recordsArrayList = new List<RecordModel>();
        private List<GroupModel> groupsBreadList = new List<GroupModel>();
        private bool Saved { get; set; } = true;
        private int groupsHashCode = 0;
        private int recordsHashCode = 0;
        public static DatabaseModel INSTANCE = new DatabaseModel();
        private ResourceManager bundle;

        //private DatabaseModel()
        //{
        //    this.bundle = LanguageController.INSTANCE.getAppLanguageBundle();
        //    createNewDatabase("New database");
        //    hashDatabase();
        //}
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
                        DeleteRecordsById(id);
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
            int lastIndex = 0;
            AddGroup(newPid, GetGroupById(id).Name());
            lastIndex = GetLastGroupID();
            CopyRecords(id, lastIndex);
            RecursiveCopyGroups(id, lastIndex, new List<int>);
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
                    recordModel.getAfterLoginString(), 
                    recordModel.getAfterPasswordString(), 
                    recordModel.getAfterUrlString());
            }
        }
        private List<GroupModel> GetChildGroupArray(int pid) 
        {
            List<GroupModel> list = new List<GroupModel>();
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
            List<GroupModel> temp = new List<GroupModel>();
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
            List<RecordModel> temp = new List<RecordModel>();
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

        }
        public void FillFromDevice(byte[][] buffer)
        {

        }
        public string GetJSONString()
        {
            JsonArray groupsArray = new JsonArray();
            JsonArray recordsArray = new JsonArray();
            JsonObject backupObj = new JsonObject();
            foreach (GroupModel groupModel in this.groupsArrayList)
            {
                JsonObject groupJSON = new JsonObject();
                groupJSON.Add("id", groupModel.Id);
                groupJSON.Add("pid", groupModel.Pid);
                groupJSON.Add("name", groupModel.Name);
                groupsArray.Add(groupJSON);
            }
            foreach (RecordModel recordModel in this.recordsArrayList)
            {
                JsonObject recordJSON = new JsonObject();
                recordJSON.Add("id", recordModel.Id);
                recordJSON.Add("pid", recordModel.Pid);
                recordJSON.Add("name", recordModel.Name);
                recordJSON.Add("login", recordModel.Login);
                recordJSON.Add("password", recordModel.GetPasswordString());
                recordJSON.Add("url", recordModel.Url);
                recordJSON.Add("loginSymbol", recordModel.getAfterLoginString());
                recordJSON.Add("passwordSymbol", recordModel.getAfterPasswordString());
                recordJSON.Add("urlSymbol", recordModel.getAfterUrlString());
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
    }
}

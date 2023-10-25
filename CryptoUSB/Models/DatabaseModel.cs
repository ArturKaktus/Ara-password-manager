using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Resources;

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
    }
}

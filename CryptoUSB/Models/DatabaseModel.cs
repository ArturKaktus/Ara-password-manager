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
        private string name = string.Empty;
        private List<GroupModel> groupsArrayList = new List<GroupModel>();
        private List<RecordModel> recordsArrayList = new List<RecordModel>();
        private List<GroupModel> groupsBreadList = new List<GroupModel>();
        private bool saved = true;
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
    }
}

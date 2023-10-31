using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{
    public class OpenFromPCController
    {
        FoxPassBackupReaderModel foxBackup;
        KakaduBackupReaderModel kakaduBackup;
        public string FilePath { get; set; }
        public char[] Password { get; set; }

        public void Open()
        {
            if (IsKakaduBackup(FilePath))
            {
                kakaduBackup = new KakaduBackupReaderModel(FilePath, Password);
                if (kakaduBackup.ImportDatabase())
                {
                    DatabaseModel.INSTANCE.Name = "TEST";
                }
            }
        }
        private bool IsKakaduBackup(string backupPath)
        {
            string extension = backupPath.Substring(backupPath.Length - 3);
            return extension.Equals("kkd");
        }
    }
}

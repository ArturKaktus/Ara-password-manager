using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{
    public class OpenFromPCController
    {
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
                    FileInfo file = new(FilePath);
                    DatabaseModel.Instance.Name = file.Name;
                }
            }
        }
        private static bool IsKakaduBackup(string backupPath)
        {
            string extension = backupPath[^3..];
            return extension.Equals("kkd");
        }
    }
}

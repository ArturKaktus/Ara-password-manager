using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{
    public class OpenFromPCController
    {
        FoxPassBackupReaderModel foxBackup;
        KakaduBackupReaderModel kakaduBackup;

        private void Open()
        {
            char[] password = new char[] { '*' }; //ВНОС ПАРОЛЯ ИЗ ОКНА
        }
        private bool IsKakaduBackup(string backupPath)
        {
            string extension = backupPath.Substring(backupPath.Length - 3);
            return extension.Equals("kkd");
        }
    }
}

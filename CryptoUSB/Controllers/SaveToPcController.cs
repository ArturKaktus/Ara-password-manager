using CryptoUSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Controllers
{
    public class SaveToPcController
    {
        public string Password { get; set; }
        public string Path { get; set; }
        public void Save()
        {
            var kakaduBackupWriterModel = new KakaduBackupWriterModel(Password, Path);
            kakaduBackupWriterModel.ExportBackup();
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class KakaduBackupWriterModel
    {
        private readonly string pathString = string.Empty;
        private readonly char[] password;
        public KakaduBackupWriterModel(string password, string path)
        {
            this.pathString = path;
            this.password = password.ToCharArray();
        }
        public bool ExportBackup()
        {
            try
            {
                if (this.pathString != null)
                {
                    using (FileStream backUpStream = new(this.pathString, FileMode.Create))
                    {
                        Console.WriteLine(DatabaseModel.Instance.GetJSONString());
                        backUpStream.Write(EncryptBackup(DatabaseModel.Instance.GetJSONString()));
                        DatabaseModel.Instance.HashDatabase();
                    }
                    FileInfo save = new(this.pathString);
                    DatabaseModel.Instance.Name = save.Name;
                    return true;

                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    private byte[] EncryptBackup(string backupString)
    {
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(new string(this.password)));

            using Aes aes = Aes.Create();
            aes.Key = hash;
            aes.Mode = CipherMode.ECB;
            aes.Padding = PaddingMode.PKCS7;

            using ICryptoTransform encryptor = aes.CreateEncryptor();
            byte[] byteCipherText = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(backupString), 0, backupString.Length);
            return byteCipherText;
        }
    }
}

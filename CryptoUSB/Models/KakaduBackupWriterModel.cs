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
        private string pathString = string.Empty;
        private char[] password;
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
                    using (FileStream backUpStream = new FileStream(this.pathString, FileMode.Create))
                    {
                        Console.WriteLine(DatabaseModel.Instance.GetJSONString());
                        backUpStream.Write(EncryptBackup(DatabaseModel.Instance.GetJSONString()));
                        DatabaseModel.Instance.HashDatabase();
                    }
                    FileInfo save = new FileInfo(this.pathString);
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
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(this.password.ToString()));

            using (Aes aes = Aes.Create())
            {
                aes.Key = hash;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    byte[] byteCipherText = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(backupString), 0, backupString.Length);
                    return byteCipherText;
                }
            }
        }
    }
}
}

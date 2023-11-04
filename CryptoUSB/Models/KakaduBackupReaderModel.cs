using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class KakaduBackupReaderModel
    {
        private string path;
        private char[] password;

        public KakaduBackupReaderModel(string path, char[] password)
        {
            this.path = path;
            this.password = password;
        }
        public bool ImportDatabase()
        {
            string jsonString = DecryptFoxBackup(ReadFile());
            if (jsonString.Equals("error"))
                return false;
            DatabaseModel.Instance.FillFromKakaduJSON(jsonString);
            DatabaseModel.Instance.HashDatabase();
            DatabaseModel.Instance.BuildTree();
            return true;
        }
        private byte[] ReadFile()
        {
            byte[] error = new byte[1];
            try
            {
                string emptyStringArray = string.Empty;
                byte[] array = File.ReadAllBytes(Path.Combine(this.path, emptyStringArray));
                return array;
            }
            catch
            {
                return error;
            }
        }
        private string DecryptFoxBackup(byte[] sourceArray)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(this.password));
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = hash;
                        aesAlg.Mode = CipherMode.ECB;
                        aesAlg.Padding = PaddingMode.PKCS7;

                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (MemoryStream ms = new MemoryStream(sourceArray))
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        using (StreamReader sr = new StreamReader(cs, Encoding.UTF8))
                        {
                            string decryptedText = sr.ReadToEnd();
                            return decryptedText;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return "error";
            }
        }
    }
}

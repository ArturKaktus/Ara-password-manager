/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 20:48
 *  Статус: ОК - Класс переведен
 */

using System;
using System.Text;

namespace CryptoUSB.Models
{
    public class RecordModel : IObjectModel
    {
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        private readonly SymbolModel _afterLoginSymbol = new();
        public char[] Password { get; set; }
        private readonly SymbolModel _afterPasswordSymbol = new();
        public string Url { get; set; }
        private readonly SymbolModel _afterUrlSymbol = new();
        public RecordModel(int id, int pid, string name, string login, string url)
        {
            Id = id;
            Pid = pid;
            Name = name;
            Url = url;
        }
        public RecordModel(int id, int pid, string name, string login, char[] password, string url, string symbolLogin, string symbolPassword, string symbolUrl)
        {
            Id = id;
            Pid = pid;
            Name = name;
            Login = login;
            Password = password;
            Url = url;
            this._afterLoginSymbol.SetSymbolValueFromString(symbolLogin);
            this._afterPasswordSymbol.SetSymbolValueFromString(symbolPassword);
            this._afterUrlSymbol.SetSymbolValueFromString(symbolUrl);
        }
        public new string GetType()
        {
            return "";
        }
        public string GetAfterLoginString()
        {
            return this._afterLoginSymbol.GetSymbolStringValue();
        }
        public string GetAfterPasswordString()
        {
            return this._afterPasswordSymbol.GetSymbolStringValue();
        }
        public string GetAfterUrlString()
        {
            return this._afterUrlSymbol.GetSymbolStringValue();
        }
        public void SetAfterLoginSymbol(string afterLoginSymbol)
        {
            this._afterLoginSymbol.SetSymbolValueFromString(afterLoginSymbol);
        }
        public void SetAfterPasswordSymbol(string afterPasswordSymbol)
        {
            this._afterPasswordSymbol.SetSymbolValueFromString(afterPasswordSymbol);
        }
        public void SetAfterUrlSymbol(string afterUrlSymbol)
        {
            this._afterUrlSymbol.SetSymbolValueFromString(afterUrlSymbol);
        }
        public string GetPasswordString()
        {
            return new string(Password); ;
        }
        public byte[] GetKakaduBytes()
        {
            byte[] idBytes = IntToDoubleByte(Id);
            byte[] pidBytes = IntToDoubleByte(Pid);
            byte[] nameBytes = GetNameBytes();
            byte[] loginBytes = GetLoginBytes();
            byte[] passwordBytes = GetPasswordBytes();
            byte[] urlBytes = GetUrlBytes();
            byte[] kakakaduBytes = ConcatTwoArrays(ConcatTwoArrays(ConcatTwoArrays(ConcatTwoArrays(ConcatTwoArrays(idBytes, pidBytes), nameBytes), loginBytes), passwordBytes), urlBytes);
            return kakakaduBytes;
        }
        private byte[] GetNameBytes()
        {
            byte[] nameBytes = GetPrepArray(48);
            try
            {
                Encoding encoding = Encoding.GetEncoding("Windows-1251");
                byte[] tempBytes = encoding.GetBytes(Name);
                if (tempBytes.Length <= 48)
                    for (int i = 0; i < tempBytes.Length; i++)
                        nameBytes[i] = tempBytes[i];
            }
            catch (Exception e)
            {

            }
            return nameBytes;
        }
        private byte[] GetLoginBytes()
        {
            byte[] loginBytes = GetPrepArray(48);
            try
            {
                Encoding encoding = Encoding.GetEncoding("Windows-1251");
                byte[] tempBytes = encoding.GetBytes(Login);
                if (tempBytes.Length <= 48)
                {
                    int i;
                    for (i = 0; i < tempBytes.Length; i++)
                        loginBytes[i] = tempBytes[i];
                    if (i < 47)
                        loginBytes[i] = this._afterLoginSymbol.GetSymbolByteValue();
                }
            }
            catch (Exception e)
            {
            }
            return loginBytes;
        }
        private byte[] GetPasswordBytes()
        {
            byte[] passwordBytes = GetPrepArray(48);
            try
            {
                Encoding encoding = Encoding.GetEncoding("Windows-1251");
                byte[] tempBytes = encoding.GetBytes(GetPasswordString());
                if (tempBytes.Length <= 48)
                {
                    int i;
                    for (i = 0; i < tempBytes.Length; i++)
                        passwordBytes[i] = tempBytes[i];
                    if (i < 47)
                        passwordBytes[i] = this._afterPasswordSymbol.GetSymbolByteValue();
                }
            }
            catch (Exception e)
            {
            }
            return passwordBytes;
        }
        private byte[] GetUrlBytes()
        {
            byte[] urlBytes = GetPrepArray(48);
            try
            {
                Encoding encoding = Encoding.GetEncoding("Windows-1251");
                byte[] tempBytes = encoding.GetBytes(Url);
                if (tempBytes.Length <= 48)
                {
                    int i;
                    for (i = 0; i < tempBytes.Length; i++)
                        urlBytes[i] = tempBytes[i];
                    if (i < 47)
                        urlBytes[i] = this._afterUrlSymbol.GetSymbolByteValue();
                }
            }
            catch (Exception e)
            {
            }
            return urlBytes;
        }
        private static byte[] GetPrepArray(int len)
        {
            byte[] prepArray = new byte[len];
            for (int i = 0; i < prepArray.Length; i++)
            {
                prepArray[i] = 0;
            }
            return prepArray;
        }
        private static byte[] IntToDoubleByte(int toByte)
        {
            byte[] doubleByte = new byte[2];
            doubleByte[1] = (byte)toByte;
            doubleByte[0] = (byte)(toByte >>> 8);
            return doubleByte;
        }
        private static byte[] ConcatTwoArrays(byte[] firstArray, byte[] secondArray)
        {
            int firstLen = firstArray.Length;
            int secondLen = secondArray.Length;
            byte[] concat = new byte[firstLen + secondLen];
            Array.Copy(firstArray, 0, concat, 0, firstLen);
            Array.Copy(secondArray, 0, concat, firstLen, secondLen);
            return concat;
        }
        public override string ToString()
        {
            return Name;
        }
    }
}

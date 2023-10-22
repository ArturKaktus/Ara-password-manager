/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 20:48
 *  Статус: ОК - Класс переведен
 */

using System;
using System.Text;

namespace CryptoUSB.Models
{
    public class RecordModel
    {
        private int _id;
        private int _pid;
        private string _name;
        private string _login;
        private SymbolModel _afterLoginSymbol = new SymbolModel();
        private char[] _password;
        private SymbolModel _afterPasswordSymbol = new SymbolModel();
        private string _url;
        private SymbolModel _afterUrlSymbol = new SymbolModel();
        public RecordModel(int id, int pid, string name, string login, string url)
        {
            this._id = id;
            this._pid = pid;
            this._name = name;
            this._url = url;
        }
        public RecordModel(int id, int pid, string name, string login, char[] password, string url, string symbolLogin, string symbolPassword, string symbolUrl)
        {
            this._id = id;
            this._pid = pid;
            this._name = name;
            this._login = login;
            this._password = password;
            this._url = url;
            this._afterLoginSymbol.SetSymbolValueFromString(symbolLogin);
            this._afterPasswordSymbol.SetSymbolValueFromString(symbolPassword);
            this._afterUrlSymbol.SetSymbolValueFromString(symbolUrl);
        }
        public int GetId()
        {
            return this._id;
        }
        public int GetPid()
        {
            return this._pid;
        }
        public char[] GetPassword()
        {
            return this._password;
        }
        public new string GetType()
        {
            return "";
        }
        public string GetName()
        {
            return this._name;
        }
        public string GetLogin()
        {
            return this._login;
        }
        public string GetUrl()
        {
            return this._url;
        }
        public string getAfterLoginString()
        {
            return this._afterLoginSymbol.GetSymbolStringValue();
        }
        public string getAfterPasswordString()
        {
            return this._afterPasswordSymbol.GetSymbolStringValue();
        }
        public string getAfterUrlString()
        {
            return this._afterUrlSymbol.GetSymbolStringValue();
        }
        public void SetId(int id)
        {
            this._id = id;
        }
        public void SetPid(int pid)
        {
            this._pid = pid;
        }
        public void SetName(string name)
        {
            this._name = name;
        }
        public void SetLogin(string login)
        {
            this._login = login;
        }
        public void SetPassword(char[] password)
        {
            this._password = password;
        }
        public void SetUrl(string url)
        {
            this._url = url;
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
            return new string(this._password); ;
        }
        public byte[] GetKakaduBytes()
        {
            byte[] idBytes = IntToDoubleByte(this._id);
            byte[] pidBytes = IntToDoubleByte(this._pid);
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
                byte[] tempBytes = encoding.GetBytes(_name);
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
                byte[] tempBytes = encoding.GetBytes(_login);
                if (tempBytes.Length <= 48)
                {
                    int i;
                    for (i = 0; i < tempBytes.Length; i++)
                        loginBytes[i] = tempBytes[i];
                    if (i < 47)
                        loginBytes[i] = this._afterLoginSymbol.getSymbolByteValue();
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
                        passwordBytes[i] = this._afterPasswordSymbol.getSymbolByteValue();
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
                byte[] tempBytes = encoding.GetBytes(_url);
                if (tempBytes.Length <= 48)
                {
                    int i;
                    for (i = 0; i < tempBytes.Length; i++)
                        urlBytes[i] = tempBytes[i];
                    if (i < 47)
                        urlBytes[i] = this._afterUrlSymbol.getSymbolByteValue();
                }
            }
            catch (Exception e)
            {
            }
            return urlBytes;
        }
        private byte[] GetPrepArray(int len)
        {
            byte[] prepArray = new byte[len];
            for (int i = 0; i < prepArray.Length; i++)
            {
                prepArray[i] = 0;
            }
            return prepArray;
        }
        private byte[] IntToDoubleByte(int toByte)
        {
            byte[] doubleByte = new byte[2];
            doubleByte[1] = (byte)toByte;
            doubleByte[0] = (byte)(toByte >>> 8);
            return doubleByte;
        }
        private byte[] ConcatTwoArrays(byte[] firstArray, byte[] secondArray)
        {
            int firstLen = firstArray.Length;
            int secondLen = secondArray.Length;
            byte[] concat = new byte[firstLen + secondLen];
            Array.Copy(firstArray, 0, concat, 0, firstLen);
            Array.Copy(secondArray, 0, concat, firstLen, secondLen);
            return concat;
        }
    }
}

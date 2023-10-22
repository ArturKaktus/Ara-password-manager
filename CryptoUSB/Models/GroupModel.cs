using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class GroupModel
    {
        private int _id;

        private int _pid;

        private string _name;
        public GroupModel(int id, int pid, string name)
        {
            this._id = id;
            this._pid = pid;
            this._name = name;
        }
        public string GetName()
        {
            return this._name;
        }

        public int GetId()
        {
            return this._id;
        }

        public int GetPid()
        {
            return this._pid;
        }

        public void SetId(int id)
        {
            this._id = id;
        }

        public void SetPid(int pid)
        {
            this._pid = pid;
        }

        public string getType()
        {
            return ""; //Декомпиляция показывает бред, будем смотреть
        }

        public void SetName(string name)
        {
            this._name = name;
        }

        public string ToString()
        {
            return this._name;
        }
        public byte[] GetKakaduBytes()
        {
            byte[] kakaduBytes = GetPrepArray(196);
            byte[] idBytes = IntToDoubleByte(this._id);
            byte[] pidBytes = IntToDoubleByte(this._pid);
            byte[] nameBytes = GetNameBytes();
            byte[] groupBytes = ConcatTwoArrays(ConcatTwoArrays(idBytes, pidBytes), nameBytes);
            for (int i = 0; i < groupBytes.Length; i++)
                kakaduBytes[i] = groupBytes[i];
            return kakaduBytes;
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
            doubleByte[0] = (byte)(toByte >> 8);
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

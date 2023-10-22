using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models.Storage
{
    public class KakaduRow
    {
        protected int id;
        protected int pid;
        protected string name;

        public KakaduRow(int id, int pid, string name)
        {
            this.id = id;
            this.pid = pid;
            this.name = name;
        }

        public KakaduRow(byte[] kakaduBytes)
        {
            byte[] tempName = new byte[48];
            this.id = ToDecFromBytes(kakaduBytes[0], kakaduBytes[1]) + 1;
            this.pid = ToDecFromBytes(kakaduBytes[0], kakaduBytes[1]) + 1;
            this.name = System.Text.Encoding.GetEncoding("Windows-1251").GetString(tempName);
        }
        public static int ToDecFromBytes(byte b1, byte b2)
        {
            byte[] byteArray = new byte[] { b1, b2 };

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(byteArray);
            }

            short shortVal = BitConverter.ToInt16(byteArray, 0);
            int x = shortVal;
            return x;
        }
        public static byte[] IntToDoubleByte(int toByte)
        {
            byte[] doubleByte = new byte[2];
            doubleByte[1] = (byte)toByte;
            doubleByte[0] = (byte)(toByte >> 8);
            return doubleByte;
        }
        public string GetName()
        {
            return this.name;
        }
        public int GetId()
        {
            return this.id;
        }
        public int GetPid()
        {
            return this.pid;
        }
        protected byte[] GetIdBytes()
        {
            return IntToDoubleByte(this.id);
        }
        protected byte[] GetPidBytes()
        {
            return IntToDoubleByte(this.id);
        }
    }
}

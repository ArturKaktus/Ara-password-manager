using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryptoUSB.Utils
{
    public static class ByteUtils
    {
        public static byte[] ConvertWin1251ToUtf8(byte[] win1251Bytes)
        {
            List<byte> utf8Bytes = new List<byte>();

            foreach (byte b in win1251Bytes)
            {
                if (b >= 192 && b <= 239)
                {
                    utf8Bytes.Add(208);
                    utf8Bytes.Add((byte)(b - 48));
                }
                else if (b >= 240 && b <= 255)
                {
                    utf8Bytes.Add(209);
                    utf8Bytes.Add((byte)(b - 112));
                }
                else if (b == 168)
                {
                    utf8Bytes.Add(208);
                    utf8Bytes.Add(129);
                }
                else if (b == 184)
                {
                    utf8Bytes.Add(209);
                    utf8Bytes.Add(145);
                }
                else if (b >= 32 && b <= 127)
                {
                    utf8Bytes.Add(b);
                }
                else
                {
                    utf8Bytes.Add(32);
                }
            }

            return [.. utf8Bytes];
        }

        public static string ByteToUtf8String(byte[] hex)
        {
            return Encoding.UTF8.GetString(ConvertWin1251ToUtf8(hex));
        }
    }
}

using System.Text;

namespace APM.Core.Utils
{
    public static class ByteUtils
    {
        public static byte[] ConvertWin1251ToUtf8(byte[] win1251Bytes)
        {
            List<byte> utf8Bytes = [];

            foreach (var b in win1251Bytes)
            {
                switch (b)
                {
                    case >= 192 and <= 239:
                        utf8Bytes.Add(208);
                        utf8Bytes.Add((byte)(b - 48));
                        break;
                    case >= 240 and <= 255:
                        utf8Bytes.Add(209);
                        utf8Bytes.Add((byte)(b - 112));
                        break;
                    case 168:
                        utf8Bytes.Add(208);
                        utf8Bytes.Add(129);
                        break;
                    case 184:
                        utf8Bytes.Add(209);
                        utf8Bytes.Add(145);
                        break;
                    case >= 32 and <= 127:
                        utf8Bytes.Add(b);
                        break;
                    default:
                        utf8Bytes.Add(32);
                        break;
                }
            }

            return [.. utf8Bytes];
        }

        public static byte[] ConvertUtf8ToWin1251(byte[] utf8Bytes)
        {
            List<byte> win1251Bytes = [];

            for (var i = 0; i < utf8Bytes.Length; i++)
            {
                switch (utf8Bytes[i])
                {
                    case 208:
                    {
                        i++;
                        if (utf8Bytes[i] >= 128 && utf8Bytes[i] <= 143)
                        {
                            win1251Bytes.Add((byte)(utf8Bytes[i] + 48));
                        }
                        else if (utf8Bytes[i] == 129)
                        {
                            win1251Bytes.Add(168);
                        }
                        else
                        {
                            win1251Bytes.Add(32);
                        }

                        break;
                    }
                    case 209:
                    {
                        i++;
                        if (utf8Bytes[i] >= 144 && utf8Bytes[i] <= 159)
                        {
                            win1251Bytes.Add((byte)(utf8Bytes[i] + 112));
                        }
                        else if (utf8Bytes[i] == 145)
                        {
                            win1251Bytes.Add(184);
                        }
                        else
                        {
                            win1251Bytes.Add(32);
                        }

                        break;
                    }
                    case >= 32 and <= 127:
                        win1251Bytes.Add(utf8Bytes[i]);
                        break;
                    default:
                        win1251Bytes.Add(32);
                        break;
                }
            }

            return [.. win1251Bytes];
        }

        /// <summary>
        /// Следующее преобразование
        /// string utf8 -> byte[] utf8 -> byte[] win1251
        /// </summary>
        /// <param name="utfString">Строка в формате UTF8</param>
        /// <returns>Массив byte[] от формата win1251</returns>
        public static byte[] Utf8ToByteString(string utfString)
        {
            var bytesUtf8 = Encoding.UTF8.GetBytes(utfString);
            return ConvertUtf8ToWin1251(bytesUtf8);
        }
        
        /// <summary>
        /// Следующее преобразование
        /// byte[] win1251 -> byte[] utf8 -> string utf8
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static string ByteToUtf8String(byte[] hex)
        {
            return Encoding.UTF8.GetString(ConvertWin1251ToUtf8(hex));
        }
    }
}

using CryptoUSB.Models;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Services
{
    public class DeviceUpdater
    {
        private DeviceDriveModel driveModel;
        private readonly int sizeOfBoot = 81920;
        byte[] ToBytes(char[] chars)
        {
            char[] newChars = new char[chars.Length];
            Array.Copy(chars, newChars, chars.Length);
            Encoding utf8 = Encoding.UTF8;
            byte[] bytes = utf8.GetBytes(newChars);
            Array.Clear(newChars, 0, newChars.Length);
            return bytes;
        }
        string ArrToStringView(byte[] arr)
        {
            string s = string.Join("|", arr);
            return s;
        }
    }
}

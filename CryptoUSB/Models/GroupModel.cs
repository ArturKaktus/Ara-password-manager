/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 20:41
 *  Статус: ОК - Класс переведен
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using CryptoUSB.Models.Interfaces;

namespace CryptoUSB.Models
{
    public class GroupModel : IGroupModel, INotifyPropertyChanged
    {
        private string _Name;
        public int Id { get; set; }
        public int Pid { get; set; }
        public string Name
        {
            get => _Name;
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public GroupModel(int id, int pid, string name)
        {
            Id = id;
            Pid = pid;
            Name = name;
        }
        public new string GetType()
        {
            return ""; //Декомпиляция показывает бред, будем смотреть
        }
        public override string ToString()
        {
            return Name;
        }
        public byte[] GetKakaduBytes()
        {
            byte[] kakaduBytes = GetPrepArray(196);
            byte[] idBytes = IntToDoubleByte(Id);
            byte[] pidBytes = IntToDoubleByte(Pid);
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

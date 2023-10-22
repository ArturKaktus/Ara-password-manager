/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 01:34
 *  Статус: ОК - Класс переведен
 */

using System;
using System.Collections.Generic;

namespace CryptoUSB.Services
{
    public class PasswordGenerator
    {
        private readonly string _alphabet = "abcdefghijklmnopqrstuvwxyz";
        private readonly string _nums = "1234567890";
        private readonly string _symbols = "!@#$%^&*()-_+=";
        char[] sourceArray;
        public PasswordGenerator() 
        {
            this.sourceArray = (_alphabet + _alphabet.ToUpper() + _nums + _symbols).ToCharArray();
            List<char> temp = new List<char>();
            temp.AddRange(this.sourceArray);
            Shuffle(temp);
            this.sourceArray = temp.ToArray();
        }

        /// <summary>
        /// Вывод сгенерированного пароля
        /// </summary>
        /// <param name="length">Длина пароля</param>
        public string GetPassword(int length)
        {
            string password = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int randomNum = random.Next(0, this.sourceArray.Length);
                password += this.sourceArray[randomNum];
            }

            return password;
        }

        // Функция для перемешивания элементов в списке
        private static void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

    }
}

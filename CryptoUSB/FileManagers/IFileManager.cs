using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.FileManagers
{
    internal interface IFileManager
    {
        /// <summary>
        /// Расширения файла
        /// </summary>
        string[] Extensions { get; }
        
        /// <summary>
        /// Фильтры расширения в проводнике
        /// </summary>
        string[] Filters { get; }

        /// <summary>
        /// Путь до файла
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Имя файла
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Пароль к файлу
        /// </summary>
        string Password { get; }

        /// <summary>
        /// Чтение файла
        /// </summary>
        void Open();

        /// <summary>
        /// Сохранение файла
        /// </summary>
        void Save();
    }
}

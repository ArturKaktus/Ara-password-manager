using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models.Interfaces
{
    /// <summary>
    /// Интерфейс для объекта записи
    /// </summary>
    public interface IRecordModel : IObjectModel
    {
        /// <summary>
        /// Логин
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Пароль
        /// </summary>
        public char[] Password { get; set; }
        /// <summary>
        /// URL
        /// </summary>
        public string Url { get; set; }
    }
}

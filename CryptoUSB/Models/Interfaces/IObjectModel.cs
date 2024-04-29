using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models.Interfaces
{
    /// <summary>
    /// Общий интерфейс для группы или объекта записи
    /// </summary>
    public interface IObjectModel
    {
        /// <summary>
        /// ID записи
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Родительский ID
        /// </summary>
        public int Pid { get; set; }
        /// <summary>
        /// Наименование записи или группы
        /// </summary>
        public string Name { get; set; }
    }
}

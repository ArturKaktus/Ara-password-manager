using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models.Interfaces
{
    /// <summary>
    /// Интерфейс для проверки валидности
    /// </summary>
    internal interface IValidateContext
    {
        /// <summary>
        /// Метод для проверки валидности
        /// </summary>
        bool IsValidate(out string errorMessage);
    }
}

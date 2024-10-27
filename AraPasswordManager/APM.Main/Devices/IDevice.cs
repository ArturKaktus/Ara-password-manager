using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.Main.Devices
{
    public interface IDevice
    {
        /// <summary>
        /// Пинг устройства для идентификации
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public bool PingDevice(string port);

        /// <summary>
        /// Чтение с устройства
        /// </summary>
        public void ReadDevice();

        /// <summary>
        /// Запись в устройство
        /// </summary>
        public void SaveDevice();
    }
}

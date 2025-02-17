using System;
using System.IO.Ports;
using System.Threading;

namespace APM.Desktop.Devices.CryptoKakadu
{
    public class KakaduDeviceSerialPort : SerialPort
    {
        private readonly object _lock = new object();
        private readonly TimeSpan _writeTimeout = TimeSpan.FromMilliseconds(500); // Таймаут для записи
        private readonly TimeSpan _readTimeout = TimeSpan.FromMilliseconds(500); // Таймаут для чтения
        
        public KakaduDeviceSerialPort(string port) : base()
        {
            //Настройки подключения девайса Какаду
            PortName = port;
            BaudRate = 115200;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
            WriteTimeout = (int)_writeTimeout.TotalMilliseconds;
            ReadTimeout = (int)_readTimeout.TotalMilliseconds;
        }

        public void OpenPort()
        {
            lock (_lock)
            {
                if (!IsOpen) Open();
            }
        }

        public void ClosePort()
        {
            lock (_lock)
            {
                if (IsOpen) Close();
            }
        }
        
        public void WriteData(byte[] data)
        {
            lock (_lock)
            {
                try
                {
                    Write(data, 0, data.Length);
                    //Thread.Sleep(300); // Задержка между записями для предотвращения перегрузки
                }
                catch (TimeoutException)
                {

                }
            }
        }
        
        public byte[] ReadData(byte[] buffer)
        {
            lock (_lock)
            {
                try
                {
                    Read(buffer, 0, buffer.Length);
                }
                catch (TimeoutException)
                {

                }
            }
            return buffer;
        }
    }
}

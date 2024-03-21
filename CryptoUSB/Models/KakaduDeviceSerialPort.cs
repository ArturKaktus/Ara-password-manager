using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Models
{
    public class KakaduDeviceSerialPort : SerialPort
    {
        public KakaduDeviceSerialPort(string port) : base()
        {
            //Настройки подключения девайся Какаду
            base.PortName = port;
            base.BaudRate = 115200;
            base.DataBits = 8;
            base.StopBits = StopBits.One;
            base.Parity = Parity.None;
        }

        public void OpenPort(/*string portName*/)
        {
            if (!base.IsOpen)
            {
                //base.Close();
                base.Open();
            }
            //base.PortName = portName;
            //base.Open();
        }

        public void ClosePort()
        {
            if (base.IsOpen)
            {
                base.Close();
            }
        }

    }
}

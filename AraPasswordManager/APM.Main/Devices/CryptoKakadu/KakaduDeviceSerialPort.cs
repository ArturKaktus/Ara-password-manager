using System.IO.Ports;

namespace APM.Main.Devices.CryptoKakadu
{
    public class KakaduDeviceSerialPort : SerialPort
    {
        public KakaduDeviceSerialPort(string port) : base()
        {
            //Настройки подключения девайса Какаду
            PortName = port;
            BaudRate = 115200;
            DataBits = 8;
            StopBits = StopBits.One;
            Parity = Parity.None;
        }

        public void OpenPort()
        {
            if (!IsOpen) Open();
        }

        public void ClosePort()
        {
            if (IsOpen) Close();
        }
    }
}

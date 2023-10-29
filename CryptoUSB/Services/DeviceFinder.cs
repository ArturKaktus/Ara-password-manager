//Поиск и подключение к девайсу

using System;
using System.ComponentModel;
using System.IO.Ports;
using System.Threading;

namespace CryptoUSB.Services
{
    public class DeviceFinder
    {
        public static DeviceFinder Instance = new DeviceFinder();
        private KakaduCommander _commander = new KakaduCommander();
        private FindDevice _findDevice = new FindDevice();
        public FindDevice Find { get { return _findDevice; } }
        public KakaduCommander Commander { get { return _commander; } }
        public bool ContinFind { get; set; } = true;
        public string Port { get; set; } = string.Empty;
        public string KakaduVersion { get; set; } = string.Empty;
        public bool IsConnected { get; set; }
        public void StartSearch()
        {
            ContinFind = true;
            Thread findDeviceThread = new Thread(_findDevice.Run);
            findDeviceThread.Start();
        }
        private void IsConnected_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsConnected))
                IsConnected = ((FindDevice)sender).IsConnected;
        }
        public int GetVersion()
        {
            return Commander.GetFWVersion();
        }
        public void StopSearch() 
        {
            ContinFind = false;
        }
        
        public bool PingDevice(string port)
        {
            try
            {
                Commander.SetPort(port);
                return Commander.SendWAY();
            }
            catch 
            {
                return false;
            }
        }
    }
    public class FindDevice : INotifyPropertyChanged
    {
        private string _answerString = "";
        private bool _connected = false;
        public bool IsConnected
        {
            get => _connected;
            protected set
            {
                if (value != _connected)
                {
                    _connected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Run()
        {
            while (DeviceFinder.Instance.ContinFind)
            {
                if (IsConnected)
                {
                    IsConnected = DeviceFinder.Instance.PingDevice(DeviceFinder.Instance.Port);
                }
                else
                {
                    try
                    {
                        IsConnected = SearchInAllPorts();
                    }
                    catch (Exception e)
                    {
                        Run();
                    }
                }
                DeviceFinder.Instance.KakaduVersion = DeviceFinder.Instance.Commander.GetKakaduVersion();
            }
        }

        public string GetPort()
        {
            return DeviceFinder.Instance.Port;
        }

        private bool SearchInAllPorts()
        {
            string[] portsNames = SerialPort.GetPortNames();
            for (int i = 0; i < portsNames.Length; i++)
            {
                DeviceFinder.Instance.Commander.SetPort(portsNames[i]);
                try
                {
                    if (DeviceFinder.Instance.Commander.SendWAY())
                    {
                        DeviceFinder.Instance.Port = portsNames[i];
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            return false;
        }
    }
}

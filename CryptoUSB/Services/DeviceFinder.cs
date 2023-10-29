using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CryptoUSB.Services
{
    public class DeviceFinder : INotifyPropertyChanged
    {
        public static DeviceFinder Instance = new DeviceFinder();
        private KakaduCommander _commander = new KakaduCommander();
        private FindDevice _findDevice = new FindDevice();
        private bool _isConnected = false;
        public FindDevice Find { get { return _findDevice; } }
        public KakaduCommander Commander { get { return _commander; } }
        public bool ContinFind { get; set; } = true;
        public string Port { get; set; } = string.Empty;
        public string KakaduVersion { get; set; } = string.Empty;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged(nameof(IsConnected));
                }
            }
        }
        public void StartSearch()
        {
            ContinFind = true;
            //_findDevice = new FindDevice();
            _findDevice.PropertyChanged += IsConnected_PropertyChange;
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
            return _commander.GetFWVersion();
        }
        public void StopSearch() 
        {
            ContinFind = false;
        }
        
        public bool PingDevice(string port)
        {
            try
            {
                _commander.SetPort(port);
                return _commander.SendWAY();
            }
            catch 
            {
                return false;
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
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

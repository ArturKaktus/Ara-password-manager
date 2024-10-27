using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO.Ports;
using System.Threading;

namespace APM.Main.Devices
{
    public partial class DeviceFinder : ObservableObject
    {
        public static DeviceFinder Instance = new();

        [ObservableProperty]
        public bool _isConnected = false;
        [ObservableProperty]
        public bool _continueFind = true;
        [ObservableProperty]
        public IDevice _selectedDevice;
        [ObservableProperty]
        public string _portOfSelectedDevice;
        private Thread findDeviceThread;
        public void StartSearch()
        {
            ContinueFind = true;
            findDeviceThread = new(Run);
            findDeviceThread.Start();
        }

        public void StopSearch(bool forExit = false)
        {
            ContinueFind = false;
            if (forExit)
                findDeviceThread?.Join();
        }

        public void Run()
        {
            while (ContinueFind)
            {
                if (IsConnected)
                {
                    if (!SelectedDevice.PingDevice(PortOfSelectedDevice))
                    {
                        PortOfSelectedDevice = string.Empty;
                        SelectedDevice = null;
                        IsConnected = false;
                    }
                }
                else
                {
                    try
                    {
                        IsConnected = SearchInAllPorts();
                    }
                    catch (Exception)
                    {
                        Run();
                    }
                }
            }
        }

        private bool SearchInAllPorts()
        {
            string[] portsNames = SerialPort.GetPortNames();
            for (int i = 0; i < portsNames.Length; i++)
            {
                foreach (var device in AppDocument.DeviceInstances)
                {
                    try
                    {
                        if (device.PingDevice(portsNames[i]))
                        {
                            IsConnected = true;
                            SelectedDevice = device;
                            PortOfSelectedDevice = portsNames[i];
                            return true;
                        }
                    }
                    catch { }
                }
            }
            return false;
        }
    }
}

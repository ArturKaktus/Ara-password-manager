using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO.Ports;
using System.Threading;
using APM.Main.Devices.CryptoKakadu;

namespace APM.Main.Devices
{
    public partial class DeviceFinder : ObservableObject
    {
        public static DeviceFinder Instance = new();

        [ObservableProperty] private bool _isConnected = false;
        [ObservableProperty] private bool _continueFind = true;
        [ObservableProperty] private IDevice _selectedDevice;
        [ObservableProperty] private string _portOfSelectedDevice;
        [ObservableProperty] private bool _stopPing = false;
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
                    if (!_stopPing && !SelectedDevice.PingDevice(PortOfSelectedDevice))
                    {
                        PortOfSelectedDevice = string.Empty;
                        SelectedDevice = null;
                        AppDocument.SelectedDeviceSerialPort = null;
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
                        if (!_stopPing)
                        {
                            if (!device.PingDevice(portsNames[i])) continue;
                            IsConnected = true;
                            SelectedDevice = device;
                            AppDocument.SelectedDeviceSerialPort ??= new KakaduDeviceSerialPort(portsNames[i]);
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

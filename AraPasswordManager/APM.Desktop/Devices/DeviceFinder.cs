using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.IO.Ports;
using System.Threading;
using APM.Desktop.Devices.CryptoKakadu;

namespace APM.Desktop.Devices
{
    public partial class DeviceFinder : ObservableObject
    {
        public static readonly DeviceFinder Instance = new();
        private static readonly ManualResetEvent PauseEvent = new(true);
        
        [ObservableProperty] private bool _isConnected = false;
        [ObservableProperty] private bool _continueFind = true;
        [ObservableProperty] private IDevice _selectedDevice = null!;
        [ObservableProperty] private string _portOfSelectedDevice = null!;
        [ObservableProperty] private bool _stopPing = false;
        private readonly Thread _findDeviceThread;

        private DeviceFinder()
        {
            _findDeviceThread = new Thread(Run);
        }
        
        public void InitSearch()
        {
            _findDeviceThread.Start();
        }

        public void StartSearch()
        {
            PauseEvent.Set();
        }

        public void StopSearch(bool forExit = false)
        {
            PauseEvent.Reset();

            if (!forExit) return;
            ContinueFind = false;
            _findDeviceThread.Join();
        }

        private void Run()
        {
            while (ContinueFind)
            {
                PauseEvent.WaitOne();
                if (IsConnected)
                {
                    if (!StopPing && SelectedDevice != null && !SelectedDevice.PingDevice(PortOfSelectedDevice))
                    {
                        PortOfSelectedDevice = string.Empty;
                        SelectedDevice = null!;
                        AppDocument.SelectedDeviceSerialPort = null!;
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
                Thread.Sleep(1000);
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
                        if (StopPing) continue;
                        if (!device.PingDevice(portsNames[i])) continue;
                        IsConnected = true;
                        SelectedDevice = device;
                        AppDocument.SelectedDeviceSerialPort ??= new KakaduDeviceSerialPort(portsNames[i]);
                        PortOfSelectedDevice = portsNames[i];
                        return true;
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            return false;
        }
    }
}

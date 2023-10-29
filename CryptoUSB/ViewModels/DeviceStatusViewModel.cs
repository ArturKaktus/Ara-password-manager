using Avalonia.Input;
using Avalonia.Interactivity;
using CryptoUSB.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class DeviceStatusViewModel : ViewModelBase
    {
        public DeviceStatusViewModel() 
        {
            DeviceFinder.Instance.Find.PropertyChanged += DeviceStatus_PropertyChange;
        }
        private string _status = "Не подключен";
        public string Status { get { return _status; } set { this.RaiseAndSetIfChanged(ref _status, value); } }
        private void DeviceStatus_PropertyChange(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsConnected")
            {
                if (sender is FindDevice fd)
                    if (fd.IsConnected)
                        Status = "Подключен";
                    else
                        Status = "Не подключен";
            }
        }
        public void GitHubPressed()
        {
            string url = "https://github.com/ArturKaktus/CryptoUSB";
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

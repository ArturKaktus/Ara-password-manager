using Avalonia.Interactivity;
using CryptoUSB.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class DeviceStatusModel : ViewModelBase
    {
        public DeviceStatusModel() 
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
    }
}

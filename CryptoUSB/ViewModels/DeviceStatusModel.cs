using CryptoUSB.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        private string _status = string.Empty;
        public string Status { get => _status; set { if (_status != value) { _status = value; OnPropertyChanged("Status"); } } }
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

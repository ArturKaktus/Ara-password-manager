using Avalonia.Controls;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class DeviceStatus : UserControl
    {
        public DeviceStatus()
        {
            InitializeComponent();
            var d = new DeviceStatusModel();
            this.DataContext = d;
        }
    }
}

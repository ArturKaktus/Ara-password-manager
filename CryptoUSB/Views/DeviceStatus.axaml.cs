using Avalonia.Controls;
using Avalonia.Input;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class DeviceStatus : UserControl
    {
        readonly DeviceStatusViewModel viewModel;
        public DeviceStatus()
        {
            InitializeComponent();
            viewModel = new DeviceStatusViewModel();
            this.DataContext = viewModel;
        }
        public void GitHubPressed(object sender, PointerPressedEventArgs e)
        {
            viewModel?.GitHubPressed();
        }
    }
}

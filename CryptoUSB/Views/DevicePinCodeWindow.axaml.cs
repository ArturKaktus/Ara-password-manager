using Avalonia.Controls;
using Avalonia.Interactivity;
using CryptoUSB.ViewModels;
using System;

namespace CryptoUSB.Views
{
    public partial class DevicePinCodeWindow : UserControl
    {
        readonly DevicePinCodeViewModel viewModel;
        public event EventHandler AcceptButtonClicked;
        public DevicePinCodeWindow()
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

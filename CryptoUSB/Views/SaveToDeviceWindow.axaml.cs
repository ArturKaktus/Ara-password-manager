using Avalonia.Controls;
using Avalonia.Interactivity;
using CryptoUSB.ViewModels;
using System;

namespace CryptoUSB.Views
{
    public partial class SaveToDeviceWindow : UserControl
    {
        readonly SaveToDeviceViewModel viewModel;
        public event EventHandler AcceptButtonClicked;
        public SaveToDeviceWindow()
        {
            InitializeComponent();
            viewModel = new SaveToDeviceViewModel();
            this.DataContext = viewModel;
        }
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

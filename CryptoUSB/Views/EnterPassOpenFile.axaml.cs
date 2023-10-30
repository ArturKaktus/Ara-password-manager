using Avalonia.Controls;
using Avalonia.Interactivity;
using CryptoUSB.ViewModels;
using System;

namespace CryptoUSB.Views
{
    public partial class EnterPassOpenFile : UserControl
    {
        EnterPassOpenFileViewModel viewModel;
        public event EventHandler AcceptButtonClicked;

        public EnterPassOpenFile(string path)
        {
            InitializeComponent();
            viewModel = new EnterPassOpenFileViewModel(path);
            this.DataContext = viewModel;
        }
        
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}

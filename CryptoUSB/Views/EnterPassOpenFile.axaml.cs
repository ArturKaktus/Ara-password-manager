using Avalonia.Controls;
using Avalonia.Interactivity;
using CryptoUSB.Models.Interfaces;
using CryptoUSB.ViewModels;
using System;

namespace CryptoUSB.Views
{
    public partial class EnterPassOpenFile : UserControl, IValidateContext
    {
        readonly EnterPassOpenFileViewModel viewModel;
        public event EventHandler AcceptButtonClicked;

        public EnterPassOpenFile() 
        {
            InitializeComponent();
        }
        public EnterPassOpenFile(string path):this()
        {
            viewModel = new EnterPassOpenFileViewModel(path);
            this.DataContext = viewModel;
        }
        
        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        public bool IsValidate(out string errorMessage)
        {
            errorMessage = string.Empty;
            return !string.IsNullOrEmpty(viewModel.Password);
        }
    }
}

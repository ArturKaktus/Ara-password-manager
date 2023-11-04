using Avalonia.Interactivity;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class EnterPassOpenFileViewModel : ViewModelBase
    {
        public EnterPassOpenFileViewModel(string path) 
        {
            PathString = path;
        }
        private string _PathString = string.Empty;
        private string _Password = string.Empty;
        private bool _IsEnableButton;
        
        public string PathString { get => _PathString; set => this.RaiseAndSetIfChanged(ref _PathString, value); }
        public string Password
        {
            get => _Password;
            set
            {
                if ( _Password != value)
                {
                    this.RaiseAndSetIfChanged(ref _Password, value);
                    IsEnableButton = !string.IsNullOrEmpty(_Password);
                }
            }
        }
        public bool IsEnableButton { get => _IsEnableButton; set { this.RaiseAndSetIfChanged(ref _IsEnableButton, value); } }
    }
}

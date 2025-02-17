using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace APM.Desktop.FileTypes.Kakadu.PasswordVerify
{
    public partial class PasswordVerifyViewModel : ObservableObject
    {
        public PasswordVerifyViewModel()
        {

        }
        public PasswordVerifyViewModel(string path):this()
        {
            PathString = Uri.UnescapeDataString(path);
        }

        [ObservableProperty]
        private string _pathString;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsEnableButton))]
        private string _password;

        public bool IsEnableButton => !string.IsNullOrEmpty(Password) && Password.Length >= 10;
    }
}

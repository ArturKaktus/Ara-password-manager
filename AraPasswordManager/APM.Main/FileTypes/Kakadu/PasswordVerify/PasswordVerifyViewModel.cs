using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace APM.Main.FileTypes.Kakadu.PasswordVerify
{
    public partial class PasswordVerifyViewModel : ObservableObject
    {
        public PasswordVerifyViewModel(string path)
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

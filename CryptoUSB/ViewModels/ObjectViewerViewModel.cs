using CryptoUSB.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class ObjectViewerViewModel : ViewModelBase
    {
        private string _Name = string.Empty;
        private string _Url = string.Empty;
        private string _Login = string.Empty;
        private string _Password = string.Empty;
        private bool _IsChanged = false;
        public string Name { get => _Name; set { this.RaiseAndSetIfChanged(ref _Name, value); IsChanged = true; } }
        public string Url { get => _Url; set { this.RaiseAndSetIfChanged(ref _Url, value); IsChanged = true; } }
        public string Login { get => _Login; set { this.RaiseAndSetIfChanged(ref _Login, value); IsChanged = true; } }
        public string Password { get => _Password; set { this.RaiseAndSetIfChanged(ref _Password, value); IsChanged = true; } }
        public IObjectModel Object { get; set; }
        public bool IsChanged { get => _IsChanged; set { this.RaiseAndSetIfChanged(ref _IsChanged, value); } }

        public ObjectViewerViewModel() { }
        public ObjectViewerViewModel(IObjectModel objectModel) 
        {
            Object = objectModel;
            Name = objectModel.Name;
            if (objectModel is RecordModel record)
            {
                Url = record.Url;
                Login = record.Login;
                Password = new string(record.Password);
            }
            IsChanged = false;
        }
        public async Task SaveChanges()
        {
            if (IsChanged)
            {
                Object.Name = Name;
                IsChanged = false;
            }
        }
    }
}

using ReactiveUI;
using System.ComponentModel;

namespace CryptoUSB.ViewModels;

public class ViewModelBase : ReactiveObject, INotifyPropertyChanged
{
    public new event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using System.ComponentModel;
using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public sealed class GroupModel(int id, int pid, string name) :  IGroup, INotifyPropertyChanged
{
    public int Id { get; set; } = id;
    public int Pid { get; set; } = pid;

    private string _title = name;

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public override string ToString()
    {
        return Title;
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
using System.ComponentModel;
using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public sealed class GroupModel(int id, int pid, string name) :  IGroup, INotifyPropertyChanged
{
    [Browsable(false)]
    public int Id { get; set; } = id;
    [Browsable(false)]
    public int Pid { get; set; } = pid;

    private string _title = name;

    [Browsable(true)]
    [DisplayName("Имя")]
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged(nameof(Title));
        }
    }

    public override string ToString() => Title;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
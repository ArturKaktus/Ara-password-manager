using System.Windows.Input;

namespace APM.Core;

public interface IContextMenu
{
    public string Title { get; }
    public bool CanExecute(object parameter);
    public bool IsEnabledMenu(object parameter);
    public void Execute(object parameter);
    public ICommand ExecuteCommand { get; }
}
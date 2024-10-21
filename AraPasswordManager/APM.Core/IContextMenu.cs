using Avalonia.Controls;
using System.Windows.Input;

namespace APM.Core;

public interface IContextMenu
{
    /// <summary>
    /// Может ли быть использоваться
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool CanExecute(object parameter);
    /// <summary>
    /// Включенный
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public bool IsEnabledMenu(object parameter);

    public ICommand? Execute { get; }
    /// <summary>
    /// Асинхронный метод при нажатии
    /// </summary>
    /// <param name="parameter"></param>
    public Task ExecAsync(object parameter);
    /// <summary>
    /// Метод при нажатии
    /// </summary>
    /// <param name="parameter"></param>
    public void Exec(object parameter);
    /// <summary>
    /// Порядок
    /// </summary>
    public int Order {  get; }

    /// <summary>
    /// Возврат MenuItem
    /// </summary>
    /// <param name="mainObj"></param>
    /// <param name="obj"></param>
    /// <returns></returns>
    public MenuItem ReturnMenuItem(object? mainObj, object? obj);
}

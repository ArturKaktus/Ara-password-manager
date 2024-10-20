using System.Windows.Input;

namespace APM.Core;

public interface IContextMenu
{
    /// <summary>
    /// Название
    /// </summary>
    public string Title { get; }
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
    /// Метод при нажатии
    /// </summary>
    /// <param name="parameter"></param>
    public void Exec(object parameter);
    /// <summary>
    /// Порядок
    /// </summary>
    public int Order {  get; }
}

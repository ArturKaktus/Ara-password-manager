namespace APM.Core.Models.Interfaces;

/// <summary>
/// Интерфейс для объекта записи
/// </summary>
public interface IRecord : IObject
{
    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; }
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; }
    /// <summary>
    /// URL
    /// </summary>
    public string Url { get; set; }
}
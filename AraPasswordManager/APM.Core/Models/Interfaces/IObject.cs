namespace APM.Core.Models.Interfaces;

/// <summary>
/// Общий интерфейс для группы или объекта записи
/// </summary>
public interface IObject
{
    /// <summary>
    /// ID записи
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Родительский ID
    /// </summary>
    public int Pid { get; set; }
    /// <summary>
    /// Наименование записи или группы
    /// </summary>
    public string Title { get; set; }
}
namespace APM.Core.ProviderInterfaces;

public interface IFileProperty
{
    /// <summary>
    /// Название файла
    /// </summary>
    public string? FileTitle { get; }

    /// <summary>
    /// Расширения для файла
    /// </summary>
    public List<string> FileExtension { get; }

    /// <summary>
    /// Паказатель защищенности
    /// </summary>
    public bool IsSecure { get; }
}
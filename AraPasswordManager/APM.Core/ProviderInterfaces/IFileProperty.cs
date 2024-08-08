namespace APM.Core.ProviderInterfaces;

public interface IFileProperty
{
    public string? FileTitle { get; }
    public List<string> FileExtension { get; }
    public bool IsSecure { get; }
}
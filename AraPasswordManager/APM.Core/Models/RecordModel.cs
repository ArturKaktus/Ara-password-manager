using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public class RecordModel : IRecord
{
    public string Login { get; set; }
    public char[] Password { get; set; }
    public string Url { get; set; }
    public int Id { get; set; }
    public int Pid { get; set; }
    public string Title { get; set; }
    public string Notes { get; set; }

    private readonly SymbolModel _afterLoginSymbol = new();
    private readonly SymbolModel _afterPasswordSymbol = new();
    private readonly SymbolModel _afterUrlSymbol = new();
    public RecordModel(int id, int pid, string name, string login, char[] password, string url, string symbolLogin, string symbolPassword, string symbolUrl)
    {
        Id = id;
        Pid = pid;
        Title = name;
        Login = login;
        Password = password;
        Url = url;
        this._afterLoginSymbol.SetSymbolValueFromString(symbolLogin);
        this._afterPasswordSymbol.SetSymbolValueFromString(symbolPassword);
        this._afterUrlSymbol.SetSymbolValueFromString(symbolUrl);
    }
    public override string ToString()
    {
        return Title;
    }
} 
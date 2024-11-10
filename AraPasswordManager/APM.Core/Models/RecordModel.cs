using System.ComponentModel;
using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public class RecordModel : IRecord, INotifyPropertyChanged, ICloneable
{
    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged(nameof(Login));
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            OnPropertyChanged(nameof(Url));
        }
    }

    public int Id { get; set; }
    public int Pid { get; set; }
    public string Title { get; set; }

    private readonly SymbolModel _afterLoginSymbol = new();
    private readonly SymbolModel _afterPasswordSymbol = new();
    private readonly SymbolModel _afterUrlSymbol = new();
    private string _login;
    private string _password;
    private string _url;

    public RecordModel(int id, int pid, string name, string login, string password, string url, string symbolLogin,
        string symbolPassword, string symbolUrl)
    {
        Id = id;
        Pid = pid;
        Title = name;
        Login = login;
        Password = password;
        Url = url;
        _afterLoginSymbol.SetSymbolValueFromString(symbolLogin);
        _afterPasswordSymbol.SetSymbolValueFromString(symbolPassword);
        _afterUrlSymbol.SetSymbolValueFromString(symbolUrl);
    }

    public override string ToString()
    {
        return Title;
    }

    public object Clone()
    {
        return new RecordModel(Id, Pid, Title, Login, Password, Url, _afterLoginSymbol.GetSymbolStringValue(), _afterPasswordSymbol.GetSymbolStringValue(), _afterUrlSymbol.GetSymbolStringValue());
    }

    public string GetAfterLoginString()
    {
        return _afterLoginSymbol.GetSymbolStringValue();
    }

    public string GetAfterPasswordString()
    {
        return _afterPasswordSymbol.GetSymbolStringValue();
    }

    public string GetAfterUrlString()
    {
        return _afterUrlSymbol.GetSymbolStringValue();
    }

    public byte GetAfterLoginByte()
    {
        return _afterLoginSymbol.GetSymbolByteValue();
    }

    public byte GetAfterPasswordByte()
    {
        return _afterPasswordSymbol.GetSymbolByteValue();
    }

    public byte GetAfterUrlByte()
    {
        return _afterUrlSymbol.GetSymbolByteValue();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
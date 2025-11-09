using System.ComponentModel;
using APM.Core.Models.Interfaces;

namespace APM.Core.Models;

public class RecordModel : IRecord, INotifyPropertyChanged, ICloneable
{
    [DisplayName("Логин")]
    [Browsable(true)]
    public string Login
    {
        get => _login;
        set
        {
            _login = value;
            OnPropertyChanged(nameof(Login));
        }
    }

    [DisplayName("Пароль")]
    [Browsable(true)]
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged(nameof(Password));
        }
    }

    [DisplayName("URL")]
    [Browsable(true)]
    public string Url
    {
        get => _url;
        set
        {
            _url = value;
            OnPropertyChanged(nameof(Url));
        }
    }

    [Browsable(false)]
    public int Id { get; set; }
    [Browsable(false)]
    public int Pid { get; set; }
    
    [DisplayName("Название")]
    [Browsable(true)]
    public string Title { get; set; }

    [DisplayName("Символ после логина")]
    [Browsable(true)]
    public SymbolValue AfterLoginSymbol { get; set; }
    
    [DisplayName("Символ после логина")]
    [Browsable(true)]
    public SymbolValue AfterPasswordSymbol { get; set; }
    
    [DisplayName("Символ после URL")]
    [Browsable(true)]
    public SymbolValue AfterUrlSymbol { get; set; }

    private string _login;
    private string _password;
    private string _url;

    public RecordModel()
    {}
    public RecordModel(int id, int pid, string name, string login, string password, string url, string symbolLogin,
        string symbolPassword, string symbolUrl)
    {
        Id = id;
        Pid = pid;
        Title = name;
        Login = login;
        Password = password;
        Url = url;
        AfterLoginSymbol = SymbolModelHelper.GetSymbolByStringValue(symbolLogin);
        AfterPasswordSymbol = SymbolModelHelper.GetSymbolByStringValue(symbolPassword);
        AfterUrlSymbol = SymbolModelHelper.GetSymbolByStringValue(symbolUrl);
    }

    public override string ToString() => Title;
    public object Clone() => new RecordModel(
        Id, 
        Pid, 
        Title, 
        Login, 
        Password, 
        Url, 
        SymbolModelHelper.GetStringBySymbolValue(AfterLoginSymbol), 
        SymbolModelHelper.GetStringBySymbolValue(AfterPasswordSymbol), 
        SymbolModelHelper.GetStringBySymbolValue(AfterUrlSymbol));

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
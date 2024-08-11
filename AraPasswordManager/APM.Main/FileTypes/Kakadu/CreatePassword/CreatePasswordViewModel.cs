using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Text.RegularExpressions;

namespace APM.Main.FileTypes.Kakadu.CreatePassword;

public partial class CreatePasswordViewModel : ObservableObject
{
    public CreatePasswordViewModel()
    {

    }
    public CreatePasswordViewModel(string path):this()
    {
        PathString = Uri.UnescapeDataString(path);
    }

    [ObservableProperty]
    private string _pathString;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableButton))]
    private string _password;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableButton))]
    private string _passwordVerify;

    public bool IsEnableButton
    {
        get
        {
            return IsPasswordValid(_password) && _password == _passwordVerify;
        }
    }
    private bool IsPasswordValid(string password)
    {
        if (string.IsNullOrEmpty(password))
            return false;

        // Проверка длины пароля
        if (password.Length < 10 || password.Length > 19)
            return false;

        // Проверка на наличие хотя бы одной цифры
        if (!Regex.IsMatch(password, @"\d"))
            return false;

        // Проверка на наличие хотя бы одной строчной буквы
        if (!Regex.IsMatch(password, @"[a-z]"))
            return false;

        // Проверка на наличие хотя бы одной заглавной буквы
        if (!Regex.IsMatch(password, @"[A-Z]"))
            return false;

        return true;
    }
}
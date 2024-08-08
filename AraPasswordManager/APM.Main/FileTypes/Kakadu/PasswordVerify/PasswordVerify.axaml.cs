using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace APM.Main.FileTypes.Kakadu.PasswordVerify;

public partial class PasswordVerify : UserControl
{
    public event EventHandler AcceptButtonClicked;
    public PasswordVerify(string path)
    {
        this.DataContext = new PasswordVerifyViewModel(path);
        InitializeComponent();
    }
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
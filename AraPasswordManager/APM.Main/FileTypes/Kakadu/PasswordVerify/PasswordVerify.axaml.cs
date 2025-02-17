using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace APM.Desktop.FileTypes.Kakadu.PasswordVerify;

public partial class PasswordVerify : UserControl
{
    public event EventHandler AcceptButtonClicked;

    public PasswordVerify() => InitializeComponent();
    public PasswordVerify(string path):this()
    {
        this.DataContext = new PasswordVerifyViewModel(path);
        
    }
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
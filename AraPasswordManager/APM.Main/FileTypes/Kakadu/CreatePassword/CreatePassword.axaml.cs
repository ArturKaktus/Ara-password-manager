using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace APM.Main.FileTypes.Kakadu.CreatePassword;

public partial class CreatePassword : UserControl
{
    public event EventHandler AcceptButtonClicked;
    public CreatePassword() => InitializeComponent();

    public CreatePassword(string path):this()
    {
        this.DataContext = new CreatePasswordViewModel(path);
    }
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
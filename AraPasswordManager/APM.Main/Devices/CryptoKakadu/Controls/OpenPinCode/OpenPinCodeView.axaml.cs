using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace APM.Main.Devices.CryptoKakadu.Controls.OpenPinCode;

public partial class OpenPinCodeView : UserControl
{
    public event EventHandler AcceptButtonClicked;
    public OpenPinCodeView(object model)
    {
        this.DataContext = model;
        InitializeComponent();
    }
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
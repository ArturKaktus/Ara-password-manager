using System;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace APM.Desktop.Devices.CryptoKakadu.Controls.SavePinCode;

public partial class SavePinCodeView : UserControl
{
    public event EventHandler AcceptButtonClicked;
    public SavePinCodeView(object model)
    {
        DataContext = model;
        InitializeComponent();
    }
    
    private void AcceptButton_Click(object sender, RoutedEventArgs e)
    {
        AcceptButtonClicked?.Invoke(this, EventArgs.Empty);
    }
}
using CommunityToolkit.Mvvm.ComponentModel;

namespace APM.Desktop.Devices.CryptoKakadu.Controls.SavePinCode;

public partial class SavePinCodeViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableButton))]
    private string _PinCode;
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableButton))]
    private string _CheckPinCode;

    [ObservableProperty]
    private bool _IsEnableButton;
    
    partial void OnPinCodeChanged(string value)
    {
        IsEnableButton = CheckPinCode == value;
    }
    
    partial void OnCheckPinCodeChanged(string value)
    {
        IsEnableButton = PinCode == value;
    }
}
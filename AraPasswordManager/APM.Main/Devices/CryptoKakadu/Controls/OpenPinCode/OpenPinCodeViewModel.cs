using CommunityToolkit.Mvvm.ComponentModel;

namespace APM.Main.Devices.CryptoKakadu.Controls.OpenPinCode;

public partial class OpenPinCodeViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsEnableButton))]
    private string _PinCode;

    [ObservableProperty]
    private bool _IsEnableButton;

    partial void OnPinCodeChanged(string value)
    {
        IsEnableButton = !string.IsNullOrEmpty(value);
    }
}

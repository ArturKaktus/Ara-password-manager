namespace CryptoUSB.ViewModels;

public class MenuViewModel : ViewModelBase
{
    public string Settings => char.ConvertFromUtf32(0x2699);
    public string Infos => char.ConvertFromUtf32(0x1F6C8);
}

using Avalonia.Controls;
using ReactiveUI;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CryptoUSB.ViewModels;

public class MenuViewModel : ViewModelBase
{
    private string _data;

    public string Settings => char.ConvertFromUtf32(0x2699);
    public string Infos => char.ConvertFromUtf32(0x1F6C8);
    public string Data
    {
        get => _data;
        set => this.RaiseAndSetIfChanged(ref _data, value);
    }
    public async Task OpenKKDFromPC()
    {
        var dialog = new OpenFileDialog();
        string[] result = null;
        dialog.Filters.Add(new FileDialogFilter() { Name = "Резервная копия Kakadu", Extensions = { "kkd" } });
        result = await dialog.ShowAsync(new Window());
        if (result != null)
        {
            Data = File.ReadAllText(result.First());
        }
    }
}

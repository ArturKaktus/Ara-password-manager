using APM.Main.Devices;
using Avalonia.Controls;
using Avalonia;

namespace APM.Main.Features.MainWindow
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;

            #if DEBUG
                this.AttachDevTools();
            #endif
        }
        
        private void MainWindow_Closing(object? sender, WindowClosingEventArgs windowClosingEventArgs)
        {
            DeviceFinder.Instance.StopSearch(true);
        }
    }
}
using APM.Main.Devices;
using Avalonia.Controls;

namespace APM.Main.Features.MainWindow
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DeviceFinder.Instance.StopSearch(true);
        }
    }
}
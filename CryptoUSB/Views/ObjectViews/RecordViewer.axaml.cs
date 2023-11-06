using Avalonia.Controls;
using CryptoUSB.Models;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class RecordViewer : UserControl
    {
        ObjectViewerViewModel model;
        public RecordViewer()
        {
            InitializeComponent();
        }
        public RecordViewer(IObjectModel objectModel):this()
        {
            model = new ObjectViewerViewModel(objectModel);
            this.DataContext = model;
        }
    }
}

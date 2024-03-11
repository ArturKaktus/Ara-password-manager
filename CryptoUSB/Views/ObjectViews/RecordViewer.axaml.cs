using Avalonia.Controls;
using CryptoUSB.Models.Interfaces;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class RecordViewer : UserControl
    {
        readonly ObjectViewerViewModel model;
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

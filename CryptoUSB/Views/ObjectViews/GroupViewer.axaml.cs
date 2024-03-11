using Avalonia.Controls;
using CryptoUSB.Models.Interfaces;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class GroupViewer : UserControl
    {
        readonly ObjectViewerViewModel model;
        public GroupViewer()
        {
            InitializeComponent();
        }
        public GroupViewer(IObjectModel objectModel):this()
        {
            model = new ObjectViewerViewModel(objectModel);
            this.DataContext = model;
        }
    }
}

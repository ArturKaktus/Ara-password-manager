using Avalonia.Controls;
using CryptoUSB.ViewModels;

namespace CryptoUSB.Views
{
    public partial class CatalogTreeView : UserControl
    {
        CatalogTreeViewViewModel viewModel;
        public CatalogTreeView()
        {
            InitializeComponent();
            viewModel = new CatalogTreeViewViewModel();
            this.DataContext = viewModel;
        }
    }
}

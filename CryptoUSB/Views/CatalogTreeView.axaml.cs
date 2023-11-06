using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Templates;
using CryptoUSB.Models;
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

        private void TreeView_SelectionChanged(object? sender, Avalonia.Controls.SelectionChangedEventArgs e)
        {
            TreeObject TreeObject = ((TreeView)sender).SelectedItem as TreeObject;
            if (TreeObject.Item is RecordModel)
                ViewContainer.Content = new RecordViewer();
            else if (TreeObject.Item is GroupModel)
                ViewContainer.Content = new GroupViewer();
        }
    }
}

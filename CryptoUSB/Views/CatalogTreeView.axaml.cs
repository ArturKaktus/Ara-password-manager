using Avalonia.Controls;
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
            if (ViewContainer.Content != null && 
                ((ObjectViewerViewModel)((UserControl)ViewContainer.Content).DataContext).IsChanged)
            {
                //TODO: если запись изменена, но не сохранена, то вопрос
            }
            TreeObject TreeObject = ((TreeView)sender).SelectedItem as TreeObject;
            if (TreeObject.Item is RecordModel recordModel)
                ViewContainer.Content = new RecordViewer(recordModel);
            else if (TreeObject.Item is GroupModel groupModel)
                ViewContainer.Content = new GroupViewer(groupModel);
        }
    }
}

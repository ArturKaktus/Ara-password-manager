using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class CatalogTreeViewViewModel : ViewModelBase
    {
        public ObservableCollection<CatalogView> Catalog { get; }
        public CatalogTreeViewViewModel() 
        {
            Catalog = new ObservableCollection<CatalogView>();
        }
        public class CatalogView
        {
            public string CatalogName { get; set; }
            public ObservableCollection<CatalogView> FoldersViews { get; set; }
            public ObservableCollection<ItemView> ItemViews { get; set; }
        }
        public class ItemView
        {
            public string ItemName { get; set; }
            public string Url { get; set; }
            public byte AfterUrl { get; set; }
            public string Login { get; set; }
            public byte AfterLogin { get; set; }
            public string Password { get; set; }
            public byte AfterPassword { get; set; }
        }
    }
    
}

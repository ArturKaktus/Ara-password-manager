using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using CryptoUSB.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.ViewModels
{
    public class CatalogTreeViewViewModel : ViewModelBase
    {
        public ObservableCollection<TreeObject> Catalog { get; set; } = new ObservableCollection<TreeObject>();
        public CatalogTreeViewViewModel() 
        {
            DatabaseModel.Instance.PropertyChanged += TreeObjets_PropertyChanged;
        }

        private void TreeObjets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Catalog.Clear();
            Catalog.Add(DatabaseModel.Instance.TreeObjects);
        }
    }
    

}

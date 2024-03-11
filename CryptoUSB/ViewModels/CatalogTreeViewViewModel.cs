using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using CryptoUSB.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReactiveUI;
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
        private ObservableCollection<TreeObject> _Catalog;
        public ObservableCollection<TreeObject> Catalog 
        { 
            get => _Catalog;
            set => this.RaiseAndSetIfChanged(ref _Catalog, value);
        }
        public CatalogTreeViewViewModel() 
        {
            Catalog = new ObservableCollection<TreeObject>();
            DatabaseModel.Instance.PropertyChanged += TreeObjets_PropertyChanged;
        }

        private void TreeObjets_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Catalog = DatabaseModel.Instance.TreeObjects;

        }
    }
}

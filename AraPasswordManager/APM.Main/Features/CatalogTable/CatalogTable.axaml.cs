using APM.Main;
using APM.Main.Features.CatalogTable;
using Avalonia.Controls;
using Avalonia.Input;
using System.Linq;
using System.Reflection;
using APM.Core;
using System;
using APM.Core.Enums;
using Avalonia.Interactivity;

namespace APM.Main.Features.CatalogTable;

public partial class CatalogTable : UserControl
{
    public CatalogTable()
    {
        this.DataContext = new CatalogTableViewModel();
        InitializeComponent();
    }
}

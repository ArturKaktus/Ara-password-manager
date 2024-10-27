using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace APM.Main.Features.CatalogTreeView.Controls.NewGroup;

public partial class NewGroupView : UserControl
{
    public NewGroupView()
    {
        this.DataContext = new NewGroupViewModel();
        InitializeComponent();
    }
}
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;

namespace APM.Main.Features.CatalogTreeView.Controls.NewGroup;

public partial class NewGroupView : UserControl
{
    public NewGroupView(string name)
    {
        this.DataContext = new NewGroupViewModel(name);
        InitializeComponent();
    }
    public NewGroupView()
    {
        this.DataContext = new NewGroupViewModel();
        InitializeComponent();
    }
}
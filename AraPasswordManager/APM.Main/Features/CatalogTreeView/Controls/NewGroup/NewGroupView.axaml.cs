using Avalonia.Controls;

namespace APM.Main.Features.CatalogTreeView.Controls.NewGroup;

public partial class NewGroupView : UserControl
{
    public NewGroupView(string name)
    {
        DataContext = new NewGroupViewModel(name);
        InitializeComponent();
    }

    public NewGroupView()
    {
        DataContext = new NewGroupViewModel();
        InitializeComponent();
    }
}
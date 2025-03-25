using Avalonia.Controls;

namespace APM.Desktop.Features.CatalogTreeView.Controls.NewGroup;

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
using APM.Core.Models.Interfaces;
using Avalonia.Controls;

namespace APM.Main.Features.CatalogTable.Controls.RecordProps;

public partial class RecordPropsView : UserControl
{
    public RecordPropsView()
    {
        DataContext = new RecordPropsViewModel();
        InitializeComponent();
    }

    public RecordPropsView(IRecord record)
    {
        DataContext = new RecordPropsViewModel(record);
        InitializeComponent();
    }
}
using APM.Core.Models.Interfaces;

namespace APM.Main.Features.CatalogTable.Controls.RecordProps;

public class RecordPropsViewModel
{
    private IRecord _record;
    public RecordPropsViewModel()
    {
        
    }

    public RecordPropsViewModel(IRecord record)
    {
        Record = record;
    }

    public IRecord Record { get => _record; set {_record = value;} }
}
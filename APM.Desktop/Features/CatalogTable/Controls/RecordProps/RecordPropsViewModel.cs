using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using Avalonia.Data.Converters;

namespace APM.Desktop.Features.CatalogTable.Controls.RecordProps;

public class RecordPropsViewModel
{
    private RecordModel _record;
    private List<SymbolValue> _symbols;
    public RecordPropsViewModel()
    {
        Record = new RecordModel();
        this._symbols = new List<SymbolValue>()
        {
            SymbolValue.Tab,
            SymbolValue.Enter,
            SymbolValue.None
        };
    }

    public RecordPropsViewModel(IRecord record)
    {
        //TODO если добовлять новые типы данных, то надо избавиться от каста
        Record = record as RecordModel;
        this._symbols = new List<SymbolValue>()
        {
            SymbolValue.Tab,
            SymbolValue.Enter,
            SymbolValue.None
        };
    }

    public List<SymbolValue> Symbols => _symbols;
    public RecordModel Record { get => _record; set {_record = value;} }
}
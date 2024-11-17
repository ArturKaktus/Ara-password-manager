using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using APM.Core.Models;
using APM.Core.Models.Interfaces;
using Avalonia.Data.Converters;

namespace APM.Main.Features.CatalogTable.Controls.RecordProps;

public class RecordPropsViewModel
{
    private RecordModel _record;
    private List<SymbolModel.SymbolValue> _symbols;
    public RecordPropsViewModel()
    {
        Record = new RecordModel();
        this._symbols = new List<SymbolModel.SymbolValue>()
        {
            SymbolModel.SymbolValue.TAB,
            SymbolModel.SymbolValue.ENTER,
            SymbolModel.SymbolValue.NONE
        };
    }

    public RecordPropsViewModel(IRecord record)
    {
        //TODO если добовлять новые типы данных, то надо избавиться от каста
        Record = record as RecordModel;
        this._symbols = new List<SymbolModel.SymbolValue>()
        {
            SymbolModel.SymbolValue.TAB,
            SymbolModel.SymbolValue.ENTER,
            SymbolModel.SymbolValue.NONE
        };
    }

    public List<SymbolModel.SymbolValue> Symbols => _symbols;
    public RecordModel Record { get => _record; set {_record = value;} }
}
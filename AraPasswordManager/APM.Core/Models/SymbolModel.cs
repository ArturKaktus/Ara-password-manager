﻿using System.ComponentModel;

namespace APM.Core.Models;

public class SymbolModel
{
    public enum SymbolValue
    {
        [Description("Tab")]
        TAB, 
        [Description("Enter")]
        ENTER, 
        [Description("Ничего")]
        NONE
    }
    private SymbolValue _symbolValue = SymbolValue.NONE;
    public SymbolValue Symbol { get => _symbolValue; set => _symbolValue = value; }
    public void SetSymbolValueFromByte(byte value)
    {
        switch (value)
        {
            case 9:
                this._symbolValue = SymbolValue.TAB;
                return;
            case 10:
                this._symbolValue = SymbolValue.ENTER;
                return;
            case 14:
                this._symbolValue = SymbolValue.ENTER;
                return;
            case 0:
                this._symbolValue = SymbolValue.NONE;
                return;
        }
        this._symbolValue = SymbolValue.NONE;
    }
    public void SetSymbolValueFromString(string value)
    {
        switch (value)
        {
            case "TAB":
                this._symbolValue = SymbolValue.TAB;
                return;
            case "ENTER":
                this._symbolValue = SymbolValue.ENTER;
                return;
            case "NONE":
                this._symbolValue = SymbolValue.NONE;
                return;
        }
        this._symbolValue = SymbolValue.NONE;
    }
    public string GetSymbolStringValue()
    {
        return this._symbolValue switch
        {
            SymbolValue.TAB => "TAB",
            SymbolValue.ENTER => "ENTER",
            SymbolValue.NONE => "NONE",
            _ => "NONE",
        };
    }
    public byte GetSymbolByteValue()
    {
        return this._symbolValue switch
        {
            SymbolValue.TAB => 9,
            SymbolValue.ENTER => 10,
            SymbolValue.NONE => 0,
            _ => 0,
        };
    }
}
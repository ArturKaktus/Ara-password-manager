using System.ComponentModel;
using static APM.Core.Models.SymbolValue;

namespace APM.Core.Models;

public enum SymbolValue
{
    [Description("Tab")]
    Tab, 
    [Description("Enter")]
    Enter, 
    [Description("Ничего")]
    None
}

public static class SymbolModelHelper
{
    public static SymbolValue GetSymbolByByteValue(byte value)
    {
        return value switch
        {
            9 => Tab,
            10 or 14 => Enter,
            0 => None,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    public static SymbolValue GetSymbolByStringValue(string value)
    {
        return value switch
        {
            "TAB" => Tab,
            "ENTER" => Enter,
            "NONE" => None,
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
        };
    }
    public static string GetStringBySymbolValue(SymbolValue symbolValue)
    {
        return symbolValue switch
        {
            Tab => "TAB",
            Enter => "ENTER",
            None => "NONE",
            _ => throw new ArgumentOutOfRangeException(nameof(symbolValue), symbolValue, null)
        };
    }
    public static byte GetByteBySymbolValue(SymbolValue symbolValue)
    {
        return symbolValue switch
        {
            Tab => 9,
            Enter => 10,
            None => 0,
            _ => throw new ArgumentOutOfRangeException(nameof(symbolValue), symbolValue, null)
        };
    }
}
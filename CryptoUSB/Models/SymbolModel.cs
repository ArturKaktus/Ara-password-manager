/*  
 *  Автор: Миловидов Артур
 *  Время: 22.10.2023 20:09
 *  Статус: ОК - Класс переведен
 */

using System;

namespace CryptoUSB.Models
{
    public class SymbolModel
    {
        enum SymbolValue
        {
            TAB, ENTER, NONE
        }
        private SymbolValue _symbolValue = SymbolValue.NONE;
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
            switch (this._symbolValue)
            {
                case SymbolValue.TAB:
                    return "TAB";
                case SymbolValue.ENTER:
                    return "ENTER";
                case SymbolValue.NONE:
                    return "NONE";
            }
            return "NONE";
        }
        public byte GetSymbolByteValue()
        {
            switch (this._symbolValue)
            {
                case SymbolValue.TAB:
                    return 9;
                case SymbolValue.ENTER:
                    return 10;
                case SymbolValue.NONE:
                    return 0;
            }
            return 0;
        }
    }
}

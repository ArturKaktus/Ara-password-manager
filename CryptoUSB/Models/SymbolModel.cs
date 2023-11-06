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
}

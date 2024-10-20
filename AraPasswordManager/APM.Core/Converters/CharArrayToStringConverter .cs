using Avalonia.Data.Converters;
using System.Globalization;

namespace APM.Core.Converters
{
    public class CharArrayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is char[] charArray)
            {
                return new string(charArray);
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}

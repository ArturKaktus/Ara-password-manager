using Avalonia.Data.Converters;
using Avalonia.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoUSB.Models;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia;

namespace CryptoUSB.ViewModels
{
    public class ObjectTypeToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (((TreeObject)value).Item is GroupModel)
                return new Bitmap(AssetLoader.Open(new Uri("avares://CryptoUSB/Assets/folder.png")));
            else if (((TreeObject)value).Item is RecordModel)
                return new Bitmap(AssetLoader.Open(new Uri("avares://CryptoUSB/Assets/file.png")));
            throw new NotImplementedException();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

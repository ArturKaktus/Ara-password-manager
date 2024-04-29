using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoUSB.Utils
{
    public static class WindowUtils
    {
        private static void ShowDialog (WindowParameter windowParameter)
        {
            Window window = new()
            {
                Title = windowParameter.Title,
                Height = windowParameter.Height,
                Width = windowParameter.Width,
                Content = windowParameter.Content,
            };
            
        }
    }
    public class WindowParameter()
    {
        public Window? Owner { get; set; }
        public string Title { get; set; }
        public double Height { get; set; }
        public double Width { get; set; } 
        public object Content { get; set; }
        public EventHandler Closed {  get; set; }
    }
}

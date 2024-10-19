using APM.Main.FileTypes.Kakadu.CreatePassword;
using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APM.Main
{
    /// <summary>
    /// Менеджер создания окон
    /// </summary>
    public class WindowManager
    {
        public static Window NewWindow(string title, object? content)
        {
            return new Window
            {
                Title = title,
                Height = 200,
                Width = 300,
                Content = content
            };
        }
    }
}

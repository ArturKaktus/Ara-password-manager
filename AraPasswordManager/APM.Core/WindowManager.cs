using Avalonia.Controls;

namespace APM.Core
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

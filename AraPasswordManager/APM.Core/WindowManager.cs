using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;

namespace APM.Core
{
    /// <summary>
    /// Менеджер создания окон
    /// </summary>
    public class WindowManager
    {
        public enum WindowButtons
        {
            OkCancel,
            Ok,
            None
        }
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

        public static async Task<bool> ShowConfirmDialog(Application application, Control content, 
            string title, double? height = null, double? width = null, WindowButtons buttons = WindowButtons.OkCancel, 
            string okString = "Ок", string cancelString = "Отмена")
        {
            Window owner = ((ClassicDesktopStyleApplicationLifetime)application.ApplicationLifetime).MainWindow;

            var window = new Window
            {
                Title = title,
                Height = height ?? 600,
                Width = width ?? 800,
                Topmost = true,
                CanResize = false
            };

            var okButton = new Button { Content = okString };
            var cancelButton = new Button { Content= cancelString };

            var buttonsPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 10, 10)
            };

            if (buttons == WindowButtons.OkCancel)
            {
                okButton.Click += (sender, e) => { window.Close(true); };
                cancelButton.Click += (sender, e) => { window.Close(false); };
                okButton.Margin = new Thickness(0, 0, 10, 0);
                buttonsPanel.Children.Add(okButton);
                buttonsPanel.Children.Add(cancelButton);
            }
            else if (buttons == WindowButtons.Ok)
            {
                okButton.Click += (sender, e) => { window.Close(true); };
                buttonsPanel.Children.Add(okButton);
            }
            
            var contentPanel = new Grid();

            // Добавляем строку для кнопок
            contentPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            contentPanel.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            // Добавляем содержимое в Grid
            content.HorizontalAlignment = HorizontalAlignment.Stretch;
            content.VerticalAlignment = VerticalAlignment.Stretch;
            contentPanel.Children.Add(content);
            Grid.SetRow(content, 0);
            Grid.SetRowSpan(content, 2);

            // Добавляем кнопки в Grid и прикрепляем их к низу
            contentPanel.Children.Add(buttonsPanel);
            Grid.SetRow(buttonsPanel, 1);

            window.Content = contentPanel;
            return await window.ShowDialog<bool>(owner);
        }
    }
}

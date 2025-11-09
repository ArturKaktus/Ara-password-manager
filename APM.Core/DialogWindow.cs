using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;

namespace APM.Core;

/// <summary>
/// Базовый класс диалогового окна с кнопками OK и Cancel
/// Создается полностью программно без использования XAML
/// </summary>
public class DialogWindow : Window
{
    private readonly Control _content;
    private readonly Button _okButton;
    private readonly Button _cancelButton;

    /// <summary>
    /// Создает новое диалоговое окно с кнопками OK и Cancel
    /// </summary>
    /// <param name="content">Контент, который будет отображаться в окне</param>
    /// <param name="title">Заголовок окна</param>
    /// <param name="width">Ширина окна</param>
    /// <param name="height">Высота окна</param>
    /// <param name="okText">Текст кнопки OK</param>
    /// <param name="cancelText">Текст кнопки Cancel</param>
    /// <param name="showCancelButton">Показывать ли кнопку Cancel</param>
    public DialogWindow(
        Control content,
        string title = "Диалог",
        double? width = null,
        double? height = null,
        string okText = "Ок",
        string cancelText = "Отмена",
        bool showCancelButton = true)
    {
        _content = content ?? throw new ArgumentNullException(nameof(content));
        
        Title = title;
        Width = width ?? (content.Width > 0 ? content.Width : 400);
        Height = height ?? (content.Height > 0 ? content.Height : 300);
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        CanResize = false;
        Topmost = true;

        // Создаем кнопки
        _okButton = new Button
        {
            Content = okText,
            Margin = new Thickness(0, 0, showCancelButton ? 10 : 0, 0),
            MinWidth = 80
        };

        _cancelButton = new Button
        {
            Content = cancelText,
            MinWidth = 80
        };

        // Обработчики событий
        _okButton.Click += (sender, e) =>
        {
            Close(true);
        };

        _cancelButton.Click += (sender, e) =>
        {
            Close(false);
        };

        // Создаем панель кнопок
        var buttonsPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Bottom,
            Margin = new Thickness(0, 0, 10, 10),
            Spacing = 0
        };

        buttonsPanel.Children.Add(_okButton);
        if (showCancelButton)
        {
            buttonsPanel.Children.Add(_cancelButton);
        }

        // Создаем основной контейнер
        var mainGrid = new Grid();
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

        // Настраиваем контент
        _content.HorizontalAlignment = HorizontalAlignment.Stretch;
        _content.VerticalAlignment = VerticalAlignment.Stretch;
        _content.Margin = new Thickness(10);
        
        // Добавляем элементы в Grid
        mainGrid.Children.Add(_content);
        Grid.SetRow(_content, 0);
        
        mainGrid.Children.Add(buttonsPanel);
        Grid.SetRow(buttonsPanel, 1);

        Content = mainGrid;
    }

    /// <summary>
    /// Показывает диалоговое окно и возвращает результат
    /// </summary>
    /// <param name="owner">Окно-владелец (если null, используется главное окно приложения)</param>
    /// <returns>true, если нажата кнопка OK, false - если Cancel</returns>
    public async Task<bool> ShowDialogAsync(Window? owner = null)
    {
        if (owner == null && Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            owner = desktop.MainWindow;
        }

        var result = await ShowDialog<bool>(owner);
        return result;
    }

    /// <summary>
    /// Статический метод для быстрого создания и отображения диалогового окна
    /// </summary>
    /// <param name="content">Контент, который будет отображаться в окне</param>
    /// <param name="title">Заголовок окна</param>
    /// <param name="width">Ширина окна</param>
    /// <param name="height">Высота окна</param>
    /// <param name="okText">Текст кнопки OK</param>
    /// <param name="cancelText">Текст кнопки Cancel</param>
    /// <param name="showCancelButton">Показывать ли кнопку Cancel</param>
    /// <param name="owner">Окно-владелец (если null, используется главное окно приложения)</param>
    /// <returns>true, если нажата кнопка OK, false - если Cancel</returns>
    public static async Task<bool> ShowAsync(
        Control content,
        string title = "Диалог",
        double? width = null,
        double? height = null,
        string okText = "Ок",
        string cancelText = "Отмена",
        bool showCancelButton = true,
        Window? owner = null)
    {
        var dialog = new DialogWindow(content, title, width, height, okText, cancelText, showCancelButton);
        return await dialog.ShowDialogAsync(owner);
    }
}


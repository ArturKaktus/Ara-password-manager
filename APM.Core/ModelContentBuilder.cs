using System.ComponentModel;
using System.Reflection;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Layout;

namespace APM.Core;

/// <summary>
/// Создатель контента на основе модели
/// </summary>
public static class ModelContentBuilder
{
    /// <summary>
    /// Создает Control для отображения и редактирования свойств модели
    /// </summary>
    /// <param name="model">Модель для отображения</param>
    /// <param name="spacing">Расстояние между элементами</param>
    /// <param name="margin">Отступы для элементов</param>
    /// <returns>Control с полями для редактирования свойств модели</returns>
    public static Control CreateContent(object model, double spacing = 5, Thickness? margin = null)
    {
        if (model == null)
            throw new ArgumentNullException(nameof(model));

        var actualMargin = margin ?? new Thickness(10);
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Spacing = spacing,
            Margin = actualMargin
        };

        var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            // Проверяем атрибут Browsable
            var browsableAttr = property.GetCustomAttribute<BrowsableAttribute>();
            
            // Если атрибут Browsable явно установлен в false, пропускаем свойство
            if (browsableAttr != null && !browsableAttr.Browsable)
                continue;
            
            // Если атрибут Browsable отсутствует, показываем свойство (поведение по умолчанию)
            // Если нужно скрывать свойства без атрибута, раскомментируйте следующую строку:
            // if (browsableAttr == null) continue;

            // Получаем DisplayName или используем имя свойства
            var displayNameAttr = property.GetCustomAttribute<DisplayNameAttribute>();
            var displayName = displayNameAttr?.DisplayName ?? property.Name;

            // Проверяем, можно ли редактировать свойство (есть setter)
            bool isReadOnly = !property.CanWrite || property.SetMethod == null || !property.SetMethod.IsPublic;

            // Создаем метку
            var label = new TextBlock
            {
                Text = displayName + ":",
                Margin = new Thickness(0, spacing, 0, 0)
            };

            stackPanel.Children.Add(label);

            // Создаем контрол для редактирования в зависимости от типа свойства
            Control editor = CreateEditorForProperty(property, model, isReadOnly);
            stackPanel.Children.Add(editor);
        }

        return stackPanel;
    }

    /// <summary>
    /// Создает редактор для свойства в зависимости от его типа
    /// </summary>
    private static Control CreateEditorForProperty(PropertyInfo property, object model, bool isReadOnly)
    {
        var propertyType = property.PropertyType;

        // Для строк - TextBox
        if (propertyType == typeof(string))
        {
            var textBox = new TextBox
            {
                IsReadOnly = isReadOnly,
                Watermark = $"Введите {property.Name}"
            };

            // Создаем привязку данных
            var binding = new Binding(property.Name)
            {
                Source = model,
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            textBox.Bind(TextBox.TextProperty, binding);

            return textBox;
        }

        // Для чисел - TextBox с валидацией
        if (propertyType == typeof(int) || propertyType == typeof(int?)
            || propertyType == typeof(double) || propertyType == typeof(double?))
        {
            var textBox = new TextBox
            {
                IsReadOnly = isReadOnly,
                Watermark = "Введите число"
            };

            var binding = new Binding(property.Name)
            {
                Source = model,
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay,
                Converter = new NumericConverter(propertyType)
            };
            textBox.Bind(TextBox.TextProperty, binding);

            return textBox;
        }

        // Для bool - CheckBox
        if (propertyType == typeof(bool) || Nullable.GetUnderlyingType(propertyType) == typeof(bool))
        {
            var checkBox = new CheckBox
            {
                IsEnabled = !isReadOnly
            };

            var binding = new Binding(property.Name)
            {
                Source = model,
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            checkBox.Bind(CheckBox.IsCheckedProperty, binding);

            return checkBox;
        }

        // Для enum - ComboBox
        if (propertyType.IsEnum)
        {
            var comboBox = new ComboBox
            {
                IsEnabled = !isReadOnly,
                ItemsSource = Enum.GetValues(propertyType)
            };

            var binding = new Binding(property.Name)
            {
                Source = model,
                Mode = isReadOnly ? BindingMode.OneWay : BindingMode.TwoWay
            };
            comboBox.Bind(ComboBox.SelectedItemProperty, binding);

            return comboBox;
        }

        // Для остальных типов - TextBox с ToString
        var defaultTextBox = new TextBox
        {
            IsReadOnly = true,
            Text = property.GetValue(model)?.ToString() ?? string.Empty
        };

        if (!isReadOnly)
        {
            // Попытка создать привязку даже для неизвестных типов
            try
            {
                var binding = new Binding(property.Name)
                {
                    Source = model,
                    Mode = BindingMode.TwoWay,
                    Converter = new ToStringConverter()
                };
                defaultTextBox.Bind(TextBox.TextProperty, binding);
                defaultTextBox.IsReadOnly = false;
            }
            catch
            {
                
            }
        }

        return defaultTextBox;
    }

    /// <summary>
    /// Простой конвертер для числовых типов
    /// </summary>
    private class NumericConverter : IValueConverter
    {
        private readonly Type _targetType;

        public NumericConverter(Type targetType)
        {
            _targetType = targetType;
        }

        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return _targetType.IsValueType && Nullable.GetUnderlyingType(_targetType) == null 
                    ? Activator.CreateInstance(_targetType) 
                    : null;

            var stringValue = value.ToString();
            
            if (_targetType == typeof(int) || _targetType == typeof(int?))
            {
                if (int.TryParse(stringValue, out var intValue))
                    return intValue;
            }
            else if (_targetType == typeof(double) || _targetType == typeof(double?))
            {
                if (double.TryParse(stringValue, System.Globalization.NumberStyles.Any, culture, out var doubleValue))
                    return doubleValue;
            }

            return _targetType.IsValueType && Nullable.GetUnderlyingType(_targetType) == null 
                ? Activator.CreateInstance(_targetType) 
                : null;
        }
    }

    /// <summary>
    /// Конвертер для преобразования в строку и обратно
    /// </summary>
    private class ToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value?.ToString();
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}


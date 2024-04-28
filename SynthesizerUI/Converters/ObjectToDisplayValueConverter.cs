using System.Globalization;
using System.Windows.Data;

namespace SynthesizerUI.Converters;

public class ObjectToDisplayValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null || parameter is not string displayProperty) return string.Empty;

        var property = value.GetType().GetProperty(displayProperty);
        if (property != null)
        {
            return property.GetValue(value) ?? string.Empty;
        }

        return string.Empty;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Not required for one-way binding (combo box selection)
        throw new NotImplementedException();
    }
}
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace SynthesizerUI.Converters;

public class VisibilityToBooleanConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null) return false;

        return (Visibility)value == Visibility.Visible;
    }

    public object ConvertBack(object? value, Type targetType, object parameter, CultureInfo culture)
    {
        if(value == null) return Visibility.Collapsed;

        return (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }
}
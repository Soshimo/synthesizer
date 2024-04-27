using System.Globalization;
using System.Windows.Data;

namespace SynthesizerUI.Converters;

public class DefaultValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return default(double);

        var objectValue = (double)(float)value;

        if (Math.Abs(objectValue) >= 1000)
        {
            return $"{(objectValue / 1000):0.00}k";
        }

        return objectValue;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
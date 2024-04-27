using System.Globalization;
using System.Windows.Data;

namespace SynthesizerUI.Converters;

public class RoundingValueConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value == null) return default(double);
        
        var doubleValue = (double)((float)value);
        if (int.TryParse(parameter?.ToString(), out var decimalPlaces))
        {
            //return Math.Round(doubleValue, decimalPlaces);
            var format = "{0:f" + decimalPlaces + "}";
            return string.Format(format, doubleValue);
        }
        else
        {
            return doubleValue;
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
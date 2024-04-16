using System.Globalization;
using System.Windows.Data;

namespace SynthesizerUI.Converters;

public class NoteToZIndexConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        switch (value)
        {
            case null:
                throw new ArgumentNullException(nameof(value));
            case string noteAndOctave:
            {
                return noteAndOctave[^1] == '#' ? 999 : 1;
            }
            default:
                throw new ArgumentException($"Invalid type specified.  Expecting string and received {value.GetType()}");
        }
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SynthesizerUI.Converters;

public class NoteToCanvasLocationConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (!CheckArguments(values)) throw new ArgumentException("Invalid arguments specified.");

        var note = (string)values[0];
        var totalWidth = (double)values[1];
        var octave = (int)values[2];
        var containerWidth = (double)values[3];
        var alignment = (HorizontalAlignment)values[4];

        // Calculate the original left position based on the noteAndOctave property
        var leftPosition = CalculateLeftPositionBasedOnNoteAndOctave(note, octave);

        // Calculate the offset based on the alignment
        double offset = 0;
        switch (alignment)
        {
            case HorizontalAlignment.Center:
                offset = (containerWidth - totalWidth) / 2;
                break;
            case HorizontalAlignment.Right:
                offset = containerWidth - totalWidth;
                break;
            case HorizontalAlignment.Left:
            case HorizontalAlignment.Stretch:
            default:
                break;
        }

        // Add the offset to the original left position
        return leftPosition + offset;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private readonly Dictionary<string, int> _offsetDictionary = new()
    {
        {"C" , 0},
        {"C#", 25},
        { "D", 40 },
        { "D#", 65 },
        { "E", 80 },
        { "F", 120 },
        { "F#", 145 },
        { "G", 160 },
        { "G#", 185 },
        { "A", 200 },
        { "A#", 225 },
        { "B", 240 },
    };



    private double CalculateLeftPositionBasedOnNoteAndOctave(string note, int noteIndex)
    {

        //var octave = upperRegister ? 1 : 0;

//        var secondKeyboard = 

        var octave = noteIndex >= 12 ? 1 : 0;

        double baseOffset = 40 * octave * 7;// * index;

        if (!_offsetDictionary.TryGetValue(note, out var value))
            throw new ArgumentException($"Invalid note string: {note}", nameof(note));

        return value + baseOffset;
    }

    private static bool CheckArguments(object[] values)
    {
        return values is [string, double, int, double, HorizontalAlignment, ..];
    }
}
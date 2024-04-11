using SythesizerLibrary.Tuning;

namespace SythesizerLibrary.Scale;

public class Scale
{
    private readonly List<int> _degrees;
    private readonly TuningBase _tuning;
    private readonly double _rootFrequency;

    protected Scale(string rootNote, List<int> degrees, TuningBase tuning)
    {
        _degrees = degrees;
        _tuning = tuning;

        if (!_tuning.NoteNameToIndex.TryGetValue(rootNote, out var rootNoteIndex))
        {
            throw new ArgumentException($"Invalid root note: {rootNote}");
        }

        // Default octave adjustment based on the tuning system's starting note
        //int octaveAdjustment = 0;

        //// Special case for flats that technically belong to the previous octave
        //if (rootNote.EndsWith("b") && rootNote.StartsWith("A"))
        //{
        //    octaveAdjustment = -1;
        //}

        // Use the count of Ratios to determine the number of semitones per octave in the tuning system
        var semitonesPerOctave = _tuning.Ratios.Count;

        // Adjust keyIndex for flats to ensure they are correctly placed within the octave cycle
        // rootNoteIndex += semitonesPerOctave; // * octaveAdjustment;

        // Adjust the key index based on the semitones per octave to ensure it wraps correctly
        rootNoteIndex %= semitonesPerOctave;

        // Calculate the root frequency based on the adjusted key index, considering the tuning's semitone count
        _rootFrequency = 27.5 * Math.Pow(2, (double)rootNoteIndex / semitonesPerOctave);
    }

    protected Scale(int rootNoteIndex, List<int> degrees, TuningBase tuning)
    {
        _degrees = degrees;
        _tuning = tuning;
        // Use the count of Ratios to determine the number of semitones per octave in the tuning system
        var semitonesPerOctave = _tuning.Ratios.Count;
        // Adjust the key index based on the semitones per octave to ensure it wraps correctly
        rootNoteIndex %= semitonesPerOctave;
        // Calculate the root frequency based on the adjusted key index, considering the tuning's semitone count
        _rootFrequency = 27.5 * Math.Pow(2, (double)rootNoteIndex / semitonesPerOctave);
    }

    public double GetFrequency(int scaleDegree, int octave)
    {
        octave += (int)Math.Floor((double)scaleDegree / _degrees.Count);
        scaleDegree %= _degrees.Count;
        return _rootFrequency * Math.Pow(2, octave) * _tuning.Ratios[_degrees[scaleDegree]];
    }
}
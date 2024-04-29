namespace SynthesizerLibrary.Tuning;

public class WesternTuning : TuningBase
{
    public override Dictionary<string, int> NoteNameToIndex => new Dictionary<string, int>
    {
        {"A", 0}, {"A#", 1}, {"Bb", 1}, {"B", 2},
        {"C", 3}, {"C#", 4}, {"Db", 4}, {"D", 5},
        {"D#", 6}, {"Eb", 6}, {"E", 7}, {"F", 8},
        {"F#", 9}, {"Gb", 9}, {"G", 10}, {"G#", 11}, {"Ab", 11}
    };

    public WesternTuning() : base(InitializeSemiToneList(12))
    {
    }

    private static List<double> InitializeSemiToneList(int pitchesPerOctave)
    {
        var list = new List<double>();
        for (var i = 0; i < pitchesPerOctave; i++)
        {
            list.Add(i);
        }

        return list;
    }
}
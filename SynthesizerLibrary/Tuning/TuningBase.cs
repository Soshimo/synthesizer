namespace SynthesizerLibrary.Tuning;

public abstract class TuningBase
{
    public List<double> Ratios { get; }
    public abstract Dictionary<string, int> NoteNameToIndex { get; }

    protected TuningBase(List<double> semitones)
    {
        Ratios = new List<double>();

        var tuningLength = semitones.Count;
        for (var i = 0; i < tuningLength; i++)
        {
            Ratios.Add(Math.Pow(2, semitones[i] / tuningLength));
        }
    }
}
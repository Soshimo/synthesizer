using SynthesizerLibrary.Tuning;

namespace SynthesizerLibrary.Scale;

public class MajorScale : Scale
{
    public MajorScale(int keyIndex, TuningBase tuning) : base(keyIndex, new List<int> { 0, 2, 4, 5, 7, 9, 11 }, tuning)
    {}

    public MajorScale(string rootNote, TuningBase tuning) : base(rootNote, new List<int> { 0, 2, 4, 5, 7, 9, 11 }, tuning)
    {}

}
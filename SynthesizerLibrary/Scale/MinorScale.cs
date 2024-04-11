using SythesizerLibrary.Tuning;

namespace SythesizerLibrary.Scale;

public class MinorScale : Scale
{
    public MinorScale(int keyIndex, TuningBase tuning) : base(keyIndex, new List<int> { 0, 2, 3, 5, 7, 8, 10 }, tuning)
    {}

    public MinorScale(string rootNote, TuningBase tuning) : base(rootNote, new List<int> { 0, 2, 3, 5, 7, 8, 10 }, tuning)
    { }

}
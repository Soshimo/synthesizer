namespace SynthesizerUI.Model;

public class MidiDeviceInfoComparer : IEqualityComparer<MidiDeviceInfo>
{
    public bool Equals(MidiDeviceInfo? x, MidiDeviceInfo? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x == null || y == null) return false;
        return x.Name == y.Name;
    }

    public int GetHashCode(MidiDeviceInfo obj)
    {
        return obj.Name?.GetHashCode() ?? 0;
    }
}
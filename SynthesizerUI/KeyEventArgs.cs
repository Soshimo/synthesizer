namespace SynthesizerUI;

public class KeyEventArgs(string note, int octave) : EventArgs
{
    public string Note { get; } = note;
    public int Octave { get; } = octave;
}
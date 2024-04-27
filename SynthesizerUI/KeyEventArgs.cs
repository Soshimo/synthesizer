namespace SynthesizerUI;

public class KeyPressedEventArgs(string note, int octave) : EventArgs
{
    public string Note { get; } = note;
    public int Octave { get; } = octave;
}
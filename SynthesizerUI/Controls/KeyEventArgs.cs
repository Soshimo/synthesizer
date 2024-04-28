namespace SynthesizerUI.Controls;

public class KeyPressedEventArgs : EventArgs
{
    public KeyPressedEventArgs(string note, int octave)
    {
        Note = note;
        Octave = octave;
    }

    public string Note { get; }
    public int Octave { get; }
}
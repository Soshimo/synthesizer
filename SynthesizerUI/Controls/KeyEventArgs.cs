namespace SynthesizerUI.Controls;

public class KeyPressedEventArgs : EventArgs
{
    public KeyPressedEventArgs(string note, int index)
    {
        Note = note;
        Index = index;
    }

    public int Index { get;  }
    public string Note { get; }
}
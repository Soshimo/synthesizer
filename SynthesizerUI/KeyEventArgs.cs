namespace SynthesizerUI;

public class KeyPressedEventArgs(string note, bool upperRegister) : EventArgs
{
    public string Note { get; } = note;
    public bool UpperRegister { get; } = upperRegister;
}
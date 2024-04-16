namespace SynthesizerUI;

public interface ISynthesizerService
{
    void NoteOn(string note);
    void NoteOff(string note);
}
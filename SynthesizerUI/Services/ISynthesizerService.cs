namespace SynthesizerUI.Services;

public interface ISynthesizerService
{
    void NoteOn(string note, int octaveBase);
    void NoteOff(string note, int octaveBase);
}
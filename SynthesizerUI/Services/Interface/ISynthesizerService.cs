using SynthesizerUI.Model;

namespace SynthesizerUI.Services.Interface;

public interface ISynthesizerService
{
    void NoteOn(int noteIndex, VoiceData data);
    void NoteOff(int noteIndex);
    void SetDrive(double value);
}
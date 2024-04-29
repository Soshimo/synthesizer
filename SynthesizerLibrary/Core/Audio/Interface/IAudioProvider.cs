namespace SynthesizerLibrary.Core.Audio.Interface;

public interface IAudioProvider
{
    bool NeedTraverse { get; set; }
    int Channels { get; }
    int SampleRate { get; }
    int TotalWriteTime { get; }
    void ConnectToOutput(IAudioNode? node);
}
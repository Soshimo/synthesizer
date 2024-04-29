using NAudio.Wave;

namespace SynthesizerLibrary.Core.Audio;

public class WasapiAudioProvider : AudioProvider
{

    private readonly WasapiOut _audioOutput;

    public WasapiAudioProvider()
    {
        _audioOutput = new WasapiOut();
        _audioOutput.Init(this);
    }

    public void Play()
    {
        _audioOutput.Play();
    }
}
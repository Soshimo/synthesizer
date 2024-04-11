using NAudio.Wave;
using SynthesizerLibrary.Core.Audio;

namespace SythesizerLibrary.Core.Audio;

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
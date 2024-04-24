using SynthesizerLibrary.Core.Audio;
using SynthesizerUI.Model;

namespace SynthesizerUI.Services;

public class VoiceFactory(AudioProvider provider, VoiceData data) : IObjectFactory<Voice>
{
    public Voice CreateInstance()
    {
        return new Voice(provider, data);
    }
}
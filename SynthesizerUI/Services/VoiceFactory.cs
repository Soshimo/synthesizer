using SynthesizerLibrary.Core.Audio;
using SynthesizerUI.Model;

namespace SynthesizerUI.Services;

public class VoiceFactory : IObjectFactory<Voice>
{
    private readonly AudioProvider _provider;
    private readonly VoiceData _data;

    public VoiceFactory(AudioProvider provider, VoiceData data)
    {
        _provider = provider;
        _data = data;
    }

    public Voice CreateInstance()
    {
        return new Voice(_provider, _data);
    }
}
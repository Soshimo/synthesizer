using SynthesizerLibrary.Core.Audio;
using SynthesizerUI.Model;

namespace SynthesizerUI.Services;

public class VoiceWrapper : IDisposable
{
    public AutomationNode FrequencyAutomationNode { get; init; }
    public Voice Voice { get; init; }

    public VoiceWrapper(AudioProvider provider, Voice voice)
    {
        FrequencyAutomationNode = new AutomationNode(provider);
        Voice = voice;
        FrequencyAutomationNode.Connect(voice);
    }

    public void Dispose()
    {
        FrequencyAutomationNode.Disconnect(Voice);
    }
}
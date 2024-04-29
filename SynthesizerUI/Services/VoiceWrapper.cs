using SynthesizerLibrary.Core.Audio;
using SynthesizerUI.Model;

namespace SynthesizerUI.Services;

public class VoiceWrapper : IDisposable
{
    public AutomationNode Oscillator1Frequency { get; init; }
    public AutomationNode Oscillator2Frequency { get; init; }
    public AutomationNode Oscillator1Detune { get; init; }
    public AutomationNode Oscillator2Detune { get; init; }

    public Voice Voice { get; init; }

    public VoiceWrapper(AudioProvider provider, Voice voice)
    {
        Oscillator1Frequency = new AutomationNode(provider);
        Oscillator2Frequency = new AutomationNode(provider);
        Oscillator1Detune = new AutomationNode(provider);
        Oscillator2Detune = new AutomationNode(provider);
        
        Voice = voice;
        //Oscillator1Frequency.Connect(voice);
        //Oscillator2Frequency.Connect(voice, 1);
        //Oscillator1Detune.Connect(voice, 2);
        //Oscillator2Detune.Connect(voice, 3);
    }

    public void Dispose()
    {
        //Oscillator2Detune.Disconnect(Voice);
        //Oscillator1Detune.Disconnect(Voice);
        //Oscillator2Frequency.Disconnect(Voice);
        //Oscillator1Frequency.Disconnect(Voice);
    }
}
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.DSP;
using SynthesizerUI.Services;
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;

namespace SynthesizerUI.Model;

public class Voice : GroupNode, IPoolObject
{
    private readonly ADSREnvelope _envelope;

    public bool IsComplete { get; private set; }

    public event EventHandler VoiceComplete;
    public Voice(IAudioProvider provider, VoiceData voiceData) : base(provider, 5, 5)
    {
        IsComplete = true;

        var osc1 = new Oscillator(provider, voiceData.Frequency, WaveShape.Sine);

        // TODO: get values from voice constructor
        _envelope = new ADSREnvelope(provider, 0, voiceData.Attack, voiceData.Decay, voiceData.Sustain, voiceData.Release);

        _envelope.Complete += (sender, args) =>
        {
            Reset();

            OnVoiceComplete();
        };

        var osc1Gain = new Gain(provider, 0.5);
        var masterGain = new Gain(provider);

        InputPassThroughNodes[0].Connect(osc1);

        osc1.Connect(osc1Gain);

        _envelope.Connect(osc1Gain, 1);

        osc1Gain.Connect(masterGain);

        masterGain.Connect(OutputPassThroughNodes[0]);
    }

    public void NoteOn()
    {
        if (!IsComplete) return;

        IsComplete = false;
        _envelope.Gate.SetValue(1);
    }

    public void NoteOff()
    {
        _envelope.Gate.SetValue(0);
    }

    public void Reset()
    {
        IsComplete = true;
    }

    protected virtual void OnVoiceComplete()
    {
        VoiceComplete?.Invoke(this, EventArgs.Empty);
    }
}
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;
using SynthesizerLibrary.DSP;
using SynthesizerUI.Services;

namespace SynthesizerUI.Model;

public sealed class Voice : GroupNode, IPoolObject
{
    private readonly ADSREnvelope _envelope;

    public bool IsComplete { get; private set; }

    public event EventHandler? VoiceComplete;

    private VoiceData _voiceData;

    private readonly Oscillator _oscillator1;
    private readonly Oscillator _oscillator2;

    public Voice(IAudioProvider provider, VoiceData voiceData) : base(provider, 10, 1)
    {
        IsComplete = true;

        _voiceData = voiceData;

        var osc1 = new Oscillator(provider, voiceData.Oscillator1Frequency);
        var osc2 = new Oscillator(provider, voiceData.Oscillator2Frequency);

        _oscillator1 = osc1;
        _oscillator2 = osc2;

        _envelope = new ADSREnvelope(provider, 0, voiceData.Attack, voiceData.Decay, voiceData.Sustain, voiceData.Release);

        _envelope.Complete += (sender, args) =>
        {
            Reset();
            OnVoiceComplete();
        };

        var osc1Gain = new Gain(provider, 0.5);
        var osc2Gain = new Gain(provider, 0.35);

        var masterGain = new Gain(provider);

        InputPassThroughNodes[0].Connect(osc1);
        InputPassThroughNodes[1].Connect(osc2);
        InputPassThroughNodes[2].Connect(osc1, 1);
        InputPassThroughNodes[3].Connect(osc2, 1);

        osc1.Connect(osc1Gain);
        osc2.Connect(osc2Gain);

        _envelope.Connect(osc1Gain, 1);
        _envelope.Connect(osc2Gain, 1);

        osc1Gain.Connect(masterGain);
        osc2Gain.Connect(masterGain);


        masterGain.Connect(OutputPassThroughNodes[0]);
    }

    public void InitializeVoice(VoiceData voiceData)
    {
        _voiceData = voiceData;

        _envelope.SetAttack(voiceData.Attack);
        _envelope.SetDecay(voiceData.Decay);
        _envelope.SetRelease(voiceData.Release);
        _envelope.SetSustain(voiceData.Sustain);

        
        _oscillator1.SetFrequency(voiceData.Oscillator1Frequency);
        _oscillator2.SetFrequency(voiceData.Oscillator2Frequency);

        _oscillator1.SetDetune(voiceData.Oscillator1Detune);
        _oscillator2.SetDetune(voiceData.Oscillator2Detune);
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

    private void OnVoiceComplete()
    {
        VoiceComplete?.Invoke(this, EventArgs.Empty);
    }

    internal void SetVolumeEnvelopeAttack(float attack)
    {
        _envelope.SetAttack(attack);
    }

    internal void SetVolumeEnvelopeDecay(float attack)
    {
        _envelope.SetDecay(attack);
    }

    internal void SetVolumeEnvelopeRelease(float attack)
    {
        _envelope.SetRelease(attack);
    }

    internal void SetVolumeEnvelopeSustain(float attack)
    {
        _envelope.SetSustain(attack);
    }
}
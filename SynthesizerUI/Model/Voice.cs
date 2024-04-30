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
    private readonly LP12Filter _filterOscillator1;
    private readonly LP12Filter _filterOscillator2;

    private readonly Oscillator _modOscillator;
    private readonly Gain _modOscillatorGain1;
    private readonly Gain _modOscillatorGain2;

    private readonly WaveShaper _waveShaper;
    private readonly Gain _waveShaperPreGain;
    private readonly Gain _waveShaperPostGain;

    public Voice(IAudioProvider provider, VoiceData voiceData) : base(provider, 0, 1)
    {
        IsComplete = true;

        _voiceData = voiceData;

        var osc1 = new Oscillator(provider, voiceData.Oscillator1Frequency);
        var osc2 = new Oscillator(provider, voiceData.Oscillator2Frequency);

        _oscillator1 = osc1;
        _oscillator2 = osc2;

        _modOscillator = new Oscillator(provider, voiceData.ModFrequency, voiceData.ModWaveShape);
        _modOscillatorGain1 = new Gain(provider, voiceData.ModOscillator1);
        _modOscillatorGain2 = new Gain(provider, voiceData.ModOscillator2);

        _modOscillator.Connect(_modOscillatorGain1);
        _modOscillator.Connect(_modOscillatorGain2);

        _modOscillatorGain1.Connect(osc1, 1); // tremolo
        _modOscillatorGain2.Connect(osc2, 1); // tremolo

        _envelope = new ADSREnvelope(provider, 0, voiceData.VolumeEnvelopeAttack, voiceData.VolumeEnvelopeDecay, voiceData.VolumeEnvelopeSustain, voiceData.VolumeEnvelopeRelease);
        _envelope.Complete += (sender, args) =>
        {
            Reset();
            OnVoiceComplete();
        };

        _filterOscillator1 = new LP12Filter(provider, voiceData.FilterCutoff, voiceData.FilterResonance);
        _filterOscillator2 = new LP12Filter(provider, voiceData.FilterCutoff, voiceData.FilterResonance);

        var osc1Gain = new Gain(provider, 0.5);
        var osc2Gain = new Gain(provider, 0.35);

        var masterGain = new Gain(provider);

        osc1.Connect(_filterOscillator1);
        osc2.Connect(_filterOscillator2);

        _filterOscillator1.Connect(osc1Gain);
        _filterOscillator2.Connect(osc2Gain);

        _envelope.Connect(osc1Gain, 1);
        _envelope.Connect(osc2Gain, 1);

        osc1Gain.Connect(masterGain);
        osc2Gain.Connect(masterGain);

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
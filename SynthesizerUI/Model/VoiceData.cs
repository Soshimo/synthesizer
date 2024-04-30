using SynthesizerLibrary.DSP;

namespace SynthesizerUI.Model;

public class VoiceData
{
    public float RootFrequency { get; set; }
    public float Oscillator1Frequency { get; set; }
    public float Oscillator2Frequency { get; set; }
    public float VolumeEnvelopeAttack { get; set; }
    public float VolumeEnvelopeDecay { get; set; }
    public float VolumeEnvelopeSustain { get; set; }
    public float VolumeEnvelopeRelease { get; set; }
    public float FilterCutoff { get; set; }
    public float FilterResonance { get; set; }
    public float Oscillator2Detune { get; set; }
    public float Oscillator1Detune { get; set; }
    public WaveShape ModWaveShape { get; set; }
    public float ModFrequency { get; set; }
    public float ModOscillator1 { get; set; }
    public float ModOscillator2 { get; set; }

    public VoiceData()
    {
    }
}
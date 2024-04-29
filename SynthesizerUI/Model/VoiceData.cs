namespace SynthesizerUI.Model;

public class VoiceData
{
    public float RootFrequency { get; set; }
    public float Oscillator1Frequency { get; set; }
    public float Oscillator2Frequency { get; set; }
    public float Attack { get; set; }
    public float Decay { get; set; }
    public float Sustain { get; set; }
    public float Release { get; set; }

    public float Oscillator2Detune { get; set; }
    public float Oscillator1Detune { get; set; }


    public VoiceData()
    {

    }

    public VoiceData(float rootFrequency, float oscillator1Frequency, float oscillator2Frequency, float oscillator1Detune, float oscillator2Detune, float attack, float decay, float sustain, float release)
    {
        RootFrequency = rootFrequency;
        Oscillator1Frequency = oscillator1Frequency;
        Oscillator2Frequency = oscillator2Frequency;
        Attack = attack;
        Decay = decay;
        Sustain = sustain;
        Release = release;
        Oscillator1Detune = oscillator1Detune;
        Oscillator2Detune = oscillator2Detune;
    }
}
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class ADSREnvelope : Envelope
{
    private const int AttackIndex = 0;
    private const int DecayIndex = 1;
    private const int ReleaseIndex = 2;

    public ADSREnvelope(IAudioProvider provider, double gate, double attack, double decay, double sustain, double release) 
        : base(provider, new[] { 0, 1, sustain, 0}, new[]  {attack, decay, release}, gate, 2)
    {
    }

    public void SetAttack(double value)
    {
        Times[AttackIndex] = value;
    }

    public void SetDecay(double value)
    {
        Times[DecayIndex] = value;
    }

    public void SetRelease(double value) { Times[ReleaseIndex] = value; }

    public void SetSustain(double value)
    {
        Levels[2] = value;
    }
}
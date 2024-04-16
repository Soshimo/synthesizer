using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class ADSREnvelope : Envelope
{
    public ADSREnvelope(IAudioProvider provider, double gate, double attack, double decay, double sustain, double release) 
        : base(provider, new[] { 0, 1, sustain, 0}, new[]  {attack, decay, release}, gate, 2)
    {
    }
}
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.Operators;

namespace SynthesizerLibrary.DSP;

public sealed class Gain : Multiply
{
    public Gain(IAudioProvider provider, double gain = 1) : base(provider, gain)
    {
    }
}
using SynthesizerLibrary.Core.Audio.Interface;
using SynthesizerLibrary.Operators;

namespace SynthesizerLibrary.DSP;

public sealed class Gain : Multiply
{
    public Gain(IAudioProvider provider, double gain = 1) : base(provider, gain)
    {
    }
}
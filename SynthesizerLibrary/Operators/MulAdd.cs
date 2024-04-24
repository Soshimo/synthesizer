using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SythesizerLibrary.Core;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SythesizerLibrary.Operators;

public class MulAdd : AudioNode
{
    public readonly Automation Multiplier;
    public readonly Automation Add;

    public MulAdd(IAudioProvider provider, double multiplier = 1, double addend = 0) : base(provider, 3, 1)
    {
        Multiplier = new Automation(this, 1, multiplier);
        Add = new Automation(this, 2, addend);
    }

    protected override void GenerateMix()
    {
        var input = Inputs[0];
        var output = Outputs[0];

        var numberOfChannels = input.Samples.Count;
        for (var i = 0; i < numberOfChannels; i++)
        {
            output.Samples[i] = input.Samples[i] * Multiplier + Add;
        }

    }
}
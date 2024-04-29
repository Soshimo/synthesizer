using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Operators;

public class Multiply : AudioNode
{
    public readonly Automation Multiplier;

    protected Multiply(IAudioProvider provider, double gain = 1.0) : base(provider, 2, 1)
    {
        Multiplier = new Automation(this, 1, gain);
    }

    protected override void GenerateMix()
    {
        double value = Multiplier;
        var input = Inputs[0];
        var numberOfChannels = input.Samples.Count;

        for (var i = 0; i < numberOfChannels; i++)
        {
            Outputs[0].Samples[i] = input.Samples[i] * value;
        }

    }
}
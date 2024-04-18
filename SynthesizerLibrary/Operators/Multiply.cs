using SythesizerLibrary.Core;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SythesizerLibrary.Operators;

public class Multiply : AudioNode
{
    private readonly Automation _gainValue;

    protected Multiply(IAudioProvider provider, double gain = 1.0) : base(provider, 2, 1)
    {
        _gainValue = new Automation(this, 1, gain);
    }

    protected override void GenerateMix()
    {
        double value = _gainValue;
        var input = Inputs[0];
        var numberOfChannels = input.Samples.Count;

        for (var i = 0; i < numberOfChannels; i++)
        {
            Outputs[0].Samples[i] = input.Samples[i] * value;
        }

    }
}
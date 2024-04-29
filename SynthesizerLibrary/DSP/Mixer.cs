using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class Mixer : AudioNode
{
    public Mixer(IAudioProvider provider, int numberOfInputs) : base(provider, numberOfInputs, 1)
    {
    }

    protected override void GenerateMix()
    {
        var mix = new double[Outputs[0].Channels];

        foreach (var input in Inputs)
        {
            for (var i = 0; i < input.Samples.Count; i++)
            {
                mix[i] += (float)input.Samples[i];
            }
        }

        var maxSample = mix.Max(Math.Abs);
        if (maxSample > 1.0f)
        {
            for (var i = 0; i < mix.Length; i++)
            {
                mix[i] /= maxSample;
            }
        }

        Outputs[0].Samples = mix.ToList();
    }

}
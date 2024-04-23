using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class PassThroughNode : AudioNode
{
    public PassThroughNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs, string name) 
        : base(provider, numberOfInputs, numberOfOutputs, name)
    {
        IsVirtual = true;
    }

    // Copy input samples directly to output samples
    protected override void GenerateMix()
    {
        for (int i = 0; i < Inputs.Count; i++)
        {
            var inputSamples = Inputs[i].Samples;
            if (Outputs.Count > i)
            {
                Outputs[i].Samples = new List<double>(inputSamples);
            }
        }
    }

}
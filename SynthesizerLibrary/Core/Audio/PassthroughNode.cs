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

    protected override void MigrateOutputSamples()
    {
        base.MigrateOutputSamples();
        //var numberOfOutputs = Outputs.Count;
        //for (var i = 0; i < numberOfOutputs; i++)
        //{
        //    var input = Inputs[i];
        //    var output = Outputs[i];

        //    if (input.Samples.Count != 0)
        //    {
        //        output.Samples = input.Samples;
        //    }
        //    else
        //    {
        //        var numberOfChannels = output.Channels;
        //        if (output.Samples.Count == numberOfChannels)
        //        {
        //            continue;
        //        }

        //        if (output.Samples.Count > numberOfChannels)
        //        {
        //            output.Samples.RemoveRange(numberOfChannels, output.Samples.Count - numberOfChannels);
        //            continue;

        //        }

        //        for (var j = output.Samples.Count; j < numberOfChannels; j++)
        //        {
        //            output.Samples.Add(0);
        //        }
        //    }

        //}
    }
}
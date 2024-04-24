using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class AudioNode : IAudioNode
{
    public IList<IChannel> Inputs { get; }
    public IList<IChannel> Outputs { get; }


    protected readonly IAudioProvider AudioProvider;

    public List<IAudioNode> InputPassThroughNodes { get; }
    public List<IAudioNode> OutputPassThroughNodes { get; set; }

    protected bool IsVirtual { get; init; }

    public bool IsAggregate { get; }
    public bool NeedsTraverse { get; set; }

    public int GetWriteTime()
    {
        throw new NotImplementedException();
    }

    protected AudioNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs, bool isAggregate = false)
    {
        AudioProvider = provider;

        Inputs = new List<IChannel>();
        for (var i = 0; i < numberOfInputs; i++)
        {
            Inputs.Add(new InputChannel(this, i));
        }

        Outputs = new List<IChannel>();
        for (var i = 0; i < numberOfOutputs; i++)
        {
            Outputs.Add(new OutputChannel(this, i));
        }

        InputPassThroughNodes = new List<IAudioNode>();
        OutputPassThroughNodes = new List<IAudioNode>();

        IsAggregate = isAggregate;

        if (!IsAggregate) return;

        // If we are an aggregate we need to create some virtual nodes
        // for the pass through nodes
        for (var i = 0; i < numberOfInputs; i++)
        {
            var passthrough = new PassThroughNode(AudioProvider, 1, 1);
            InputPassThroughNodes.Add(passthrough);
        }

        for (var i = 0; i < numberOfOutputs; i++)
        {
            OutputPassThroughNodes.Add(new PassThroughNode(AudioProvider, 1, 1));
        }
    }

    public virtual void Connect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    {
        if (node.IsAggregate)
        {
            node = node.InputPassThroughNodes[inputIndex];
            inputIndex = 0;
        }

        var inputPin = node.Inputs[inputIndex];
        var outputPin = IsAggregate ? OutputPassThroughNodes[outputIndex].Outputs[0] : Outputs[outputIndex];

        outputPin.Connect(inputPin);
        inputPin.Connect(outputPin);

        AudioProvider.NeedTraverse = true;
    }

    public virtual void Disconnect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    {
        if (node.IsAggregate)
        {
            node = node.InputPassThroughNodes[inputIndex];
            inputIndex = 0;
        }

        var inputPin = node.Inputs[inputIndex];
        var outputPin = IsAggregate ? OutputPassThroughNodes[outputIndex].Outputs[0] : Outputs[outputIndex];

        inputPin.Disconnect(outputPin);
        outputPin.Disconnect(inputPin);


        AudioProvider.NeedTraverse = true;
    }

    public virtual void Tick()
    {
        MigrateInputSamples();
        MigrateOutputSamples();

        GenerateMix();
    }

    protected virtual void MigrateOutputSamples()
    {
        var numberOfOutputs = Outputs.Count;

        for (var i = 0; i < numberOfOutputs; i++)
        {
            var output = Outputs[i];
            var numberOfChannels = output.Channels;


            if (output.Samples.Count < numberOfChannels)
            {
                for (int j = output.Samples.Count; j < numberOfChannels; j++)
                {
                    output.Samples.Add(0);
                }
            }
            else if (output.Samples.Count > numberOfChannels)
            {
                output.Samples.RemoveRange(numberOfChannels, output.Samples.Count - numberOfChannels);
            }
        }
    }



    protected virtual void MigrateInputSamples()
    {
        for (var i = 0; i < Inputs.Count; i++)
        {
            var input = Inputs[i];

            var numberOfConnectedInputChannels = 0;

            input.Samples.Clear();

            for (var j = 0; j < input.Connected.Count; j++)
            {
                var output = input.Connected[j];

                for (var k = 0; k < output.Samples.Count; k++)
                {
                    var sample = output.Samples[k];
                    if (k < numberOfConnectedInputChannels)
                    {
                        input.Samples[k] += sample;
                    }
                    else
                    {
                        input.Samples.Add(sample);
                        numberOfConnectedInputChannels++;
                    }
                }
            }

            if (input.Samples.Count > numberOfConnectedInputChannels)
            {
                input.Samples.RemoveRange(numberOfConnectedInputChannels, input.Samples.Count - numberOfConnectedInputChannels);
            }
        }
    }
    public void Remove()
    {
        // Disconnect inputs
        foreach (var input in Inputs)
        {
            foreach (var outputPin in input.Connected)
            {
                var output = outputPin.Node;
                output.Disconnect(this, outputPin.Index, input.Index);
            }
        }

        // Disconnect outputs
        foreach (var output in Outputs)
        {
            foreach (var inputPin in output.Connected)
            {
                var input = inputPin.Node;
                Disconnect(input, output.Index, inputPin.Index);
            }
        }

        foreach (var passThrough in InputPassThroughNodes)
        {
            passThrough.Remove();
        }

        foreach (var passThrough in OutputPassThroughNodes)
        {
            passThrough.Remove();
        }
    }
    public virtual List<IAudioNode> Traverse(List<IAudioNode> nodes)
    {
        if (nodes.Contains(this)) return nodes;

        nodes.Add(this);
        nodes = TraverseParents(nodes);
        return nodes;
    }
    protected virtual List<IAudioNode> TraverseParents(List<IAudioNode> nodes)
    {
        var current = nodes;

        // First, loop through each input
        foreach (var inputChannel in Inputs)
        {
            // Then loop through each 'Connected' entry in the current input
            foreach (var outputChannel in inputChannel.Connected)
            {
                // Perform the 'Traverse' method on the node of the current stream,
                // and update 'current' with the result
                current = outputChannel.Node.IsAggregate ? outputChannel.Node.OutputPassThroughNodes[outputChannel.Index].Traverse(current) : outputChannel.Node.Traverse(current);
            }
        }

        // Return the final collection of nodes after traversal
        return current;

    }

    protected virtual void GenerateMix()
    {
    }

    protected void SetNumberOfOutputChannels(int output, int numberOfChannels)
    {
        Outputs[output].Channels = numberOfChannels;
    }

}
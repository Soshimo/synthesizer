using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SynthesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SythesizerLibrary.Core.Audio;

public class AudioNode : IAudioNode
{
    public IList<IChannel> Inputs { get; }
    public IList<IChannel> Outputs { get; }


    protected readonly IAudioProvider AudioProvider;

    public List<IAudioNode> InputPassThroughNodes { get; }
    public List<IAudioNode> OutputPassThroughNodes { get; set; }

    protected bool IsVirtual { get; init; }

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
            InputPassThroughNodes.Add(new AudioNode(AudioProvider, 1, 1){ IsVirtual = true });
        }

        for (var i = 0; i < numberOfOutputs; i++)
        {
            OutputPassThroughNodes.Add(new AudioNode(AudioProvider, 1, 1) { IsVirtual = true });
        }
    }

    public void Connect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    {
        if (IsAggregate)
        {
            var psNode = OutputPassThroughNodes[outputIndex];
            psNode.Connect(node, 0, inputIndex);
        }
        else
        {
            var inputPin = node.IsAggregate ? node.InputPassThroughNodes[inputIndex].Inputs[0] : node.Inputs[inputIndex];
            var outputPin = Outputs[outputIndex];

            outputPin.Connect(inputPin);
            inputPin.Connect(outputPin);
        }

        AudioProvider.NeedTraverse = true;
    }

    public void Disconnect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    {
        if (IsAggregate)
        {
            OutputPassThroughNodes[outputIndex].Disconnect(node, 0, inputIndex);
        }
        else
        {
            var inputPin = node.IsAggregate ? node.InputPassThroughNodes[inputIndex].Inputs[0] : node.Inputs[inputIndex];
            var outputPin = Outputs[outputIndex];

            inputPin.Disconnect(outputPin);
            outputPin.Disconnect(inputPin);
        }

        AudioProvider.NeedTraverse = true;
    }

    public void Tick()
    {
        MigrateInputSamples();
        MigrateOutputSamples();

        GenerateMix();
    }

    private void MigrateOutputSamples()
    {
        if (IsVirtual)
        {
            CreateVirtualOutputSamples();
            return;
        }

        foreach (var output in Outputs)
        {

            output.Samples.Clear();
            var numberOfConnectedChannels = output.Channels;
            if (output.Samples.Count == numberOfConnectedChannels)
            {
                continue;
            }

            if (output.Samples.Count > numberOfConnectedChannels)
            {
                output.Samples.RemoveRange(numberOfConnectedChannels, output.Samples.Count - numberOfConnectedChannels);
                continue;
            }

            for (var j = output.Samples.Count; j < numberOfConnectedChannels; j++)
            {
                output.Samples.Add(0);
            }
        }
    }

    private void CreateVirtualOutputSamples()
    {
        var numberOfOutputs = Outputs.Count;
        for (var i = 0; i < numberOfOutputs; i++)
        {
            var input = Inputs[i];
            var output = Outputs[i];

            if (input.Samples.Count != 0)
            {
                output.Samples = input.Samples;
            }
            else
            {
                var numberOfChannels = output.Channels;
                if (output.Samples.Count == numberOfChannels)
                {
                    continue;
                }

                if (output.Samples.Count > numberOfChannels)
                {
                    output.Samples.RemoveRange(numberOfChannels, output.Samples.Count - numberOfChannels);
                    continue;

                }

                for (var j = output.Samples.Count; j < numberOfChannels; j++)
                {
                    output.Samples.Add(0);
                }
            }
      
        }
    }

    public void MigrateInputSamples()
    {
        foreach (var input in Inputs)
        {
            var numberOfConnectedInputChannels = 0;

            input.Samples.Clear();

            foreach (var output in input.Connected)
            {
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

    public List<IAudioNode> Traverse(List<IAudioNode> nodes)
    {
        if (nodes.Contains(this)) return nodes;

        nodes.Add(this);
        nodes = TraverseParents(nodes);
        return nodes;
    }

    private List<IAudioNode> TraverseParents(List<IAudioNode> nodes)
    {
        return Inputs
            .SelectMany(input => input.Connected)
            .Aggregate(nodes, (current, stream) => stream.Node.Traverse(current));
    }

    protected virtual void GenerateMix()
    {

    }

    protected void SetNumberOfOutputChannels(int output, int numberOfChannels)
    {
        Outputs[output].Channels = numberOfChannels;
    }

}
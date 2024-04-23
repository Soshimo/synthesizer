using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class GroupNode : AudioNode
{
    protected GroupNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs, string name) 
        : base(provider, numberOfInputs, numberOfOutputs, name, true)
    {
        for (var i = 0; i < numberOfInputs; i++)
        {
            Inputs[i].Connect(InputPassThroughNodes[i].Inputs[0]);
        }

        for (var i = 0; i < numberOfOutputs; i++)
        {
            Outputs[i].Connect(OutputPassThroughNodes[i].Outputs[0]);
        }
    }

    public override List<IAudioNode> Traverse(List<IAudioNode> nodes)
    {
        if (nodes.Contains(this)) return nodes;

        // Add the group node itself to the list
        nodes.Add(this);

        // Continue traversing connected nodes
        nodes = TraverseParents(nodes);
        return nodes;
    }

    //public override void Disconnect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    //{
    //    OutputPassThroughNodes[outputIndex].Disconnect(node, inputIndex, 0);
    //}

    //public override void Connect(IAudioNode node, int inputIndex = 0, int outputIndex = 0)
    //{
    //    var psNode = OutputPassThroughNodes[outputIndex];
    //    psNode.Connect(node, inputIndex, 0);


    //    //if (node instanceof AudioletGroup) {
    //    //    // Connect to the pass-through node rather than the group
    //    //    node = node.inputs[input || 0];
    //    //    input = 0;
    //    //}
    //    //var outputPin = this.outputs[output || 0];
    //    //var inputPin = node.inputs[input || 0];
    //    //outputPin.connect(inputPin);
    //    //inputPin.connect(outputPin);
    //}
}
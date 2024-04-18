using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class GroupNode : AudioNode
{
    protected GroupNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs) 
        : base(provider, numberOfInputs, numberOfOutputs, true)
    {
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
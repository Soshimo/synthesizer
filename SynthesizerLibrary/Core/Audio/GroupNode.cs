using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class GroupNode : AudioNode
{
    protected GroupNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs) 
        : base(provider, numberOfInputs, numberOfOutputs, true)
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
}
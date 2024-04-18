using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class GroupNode : AudioNode
{
    protected GroupNode(IAudioProvider provider, int numberOfInputs, int numberOfOutputs) 
        : base(provider, numberOfInputs, numberOfOutputs, true)
    {
    }
}
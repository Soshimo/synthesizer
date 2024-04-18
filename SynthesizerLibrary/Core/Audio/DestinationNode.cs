using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.DSP;
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;

namespace SythesizerLibrary.Core.Audio;

public class DestinationNode : GroupNode
{
    public DestinationNode(IAudioProvider provider, IAudioNode device) : base(provider, 1, 0)
    {
        var mixer = new UpMixer(provider);

        var scheduler = new Scheduler(provider);

        InputPassThroughNodes[0].Connect(scheduler, 0, 0);
        scheduler.Connect(mixer);
        mixer.Connect(device);
    }
}

using SynthesizerLibrary.DSP;

namespace SynthesizerLibrary.Core.Audio;

public sealed class DestinationNode : GroupNode
{
    public DestinationNode(AudioProvider provider, AudioDevice? device) : base(provider, 1, 0)
    {
        var mixer = new UpMixer(provider);
        var scheduler = provider.Scheduler;

        InputPassThroughNodes[0].Connect(scheduler, 0, 0);

        scheduler?.Connect(mixer);
        mixer.Connect(device);
    }
}

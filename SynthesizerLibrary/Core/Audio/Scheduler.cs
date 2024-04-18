using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class Scheduler : PassThroughNode
{
    public Scheduler(IAudioProvider provider, double bpm = 120) 
        : base(provider, 1, 1)
    {
    }
}
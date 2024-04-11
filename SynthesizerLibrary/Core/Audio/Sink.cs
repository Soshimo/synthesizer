using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;

namespace SythesizerLibrary.Core.Audio;

public class Sink : AudioNode
{
    public Sink(IAudioProvider provider, IAudioNode device) : base(provider, 1, 0, true)
    {
        var mixer = new UpMixer(provider);

        InputPassThroughNodes[0].Connect(mixer, 0, 0);
        mixer.Connect(device);
    }
}
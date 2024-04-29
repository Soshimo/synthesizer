using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class InputChannel : IChannel
{
    public IAudioNode? Node { get; }
    public int Index { get; }
    public List<IChannel> Connected { get; }
    public int Channels { get; set; }

    public List<double> Samples { get; set; }

    public InputChannel(IAudioNode? node, int index)
    {
        Node = node;
        Index = index;
        Connected = new List<IChannel>();
        Samples = new List<double>();
    }

    public void Connect(IChannel from)
    {
        Connected.Add(from);
    }

    public void Disconnect(IChannel from)
    {
        Connected.Remove(from);
        if (Connected.Count == 0)
        {
            Samples = new List<double>();
        }
    }

    public void LinkNumberOfChannels(IChannel input)
    {
        throw new NotImplementedException();
    }


}
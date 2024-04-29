using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class OutputChannel : IChannel
{
    private int _numberChannels;

    public IAudioNode? Node { get; }
    public int Index { get; }
    public List<IChannel> Connected { get; }
    public int Channels { get => GetNumberOfChannels(); set => _numberChannels = value; }
    public List<double> Samples { get; set; }


    public OutputChannel(IAudioNode? node, int index)
    {
        Node = node;
        Index = index;
        Connected = new List<IChannel>();
        Samples = new List<double>();

        _numberChannels = 1;
    }

    public void Connect(IChannel from)
    {
        Connected.Add(from);
    }

    public void Disconnect(IChannel from)
    {
        Connected.Remove(from);
    }


    private int GetNumberOfChannels()
    {
        return _numberChannels;
    }
}
namespace SynthesizerLibrary.Core.Audio.Interface;

public interface IChannel
{
    IAudioNode? Node { get; }
    int Index { get; }
    List<IChannel> Connected { get;  }
    int Channels { set; get; }
    void Connect(IChannel from);
    void Disconnect(IChannel from);
    List<double> Samples { get; set; }
}
using NAudio.Wave;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class AudioProvider : WaveProvider32, IAudioProvider
{
    private readonly Device _device;
    private readonly Sink _sink;

    public AudioProvider()
    {
        _device = new Device(this);
        _sink = new Sink(this, _device);
    }

    public override int Read(float[] buffer, int offset, int sampleCount)
    {
        _device.Read(buffer, offset, sampleCount);
        return sampleCount;
    }


    public bool NeedTraverse 
    { 
        set => _device.NeedsTraverse = value;
        get => _device.NeedsTraverse;
    }

    public int Channels => WaveFormat.Channels;
    public int SampleRate => WaveFormat.SampleRate;
    public int TotalWriteTime => _device.GetWriteTime();
    public void ConnectToOutput(IAudioNode node)
    {
        node.Connect(_sink, 0, 0);
    }
}
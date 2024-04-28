using NAudio.CoreAudioApi;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI.Services;

public class AudioDeviceService : IAudioDeviceService
{

    private readonly MMDeviceEnumerator _deviceEnumerator = new();

    public AudioDeviceService()
    {
    }
    public IEnumerable<(string, string)> GetActiveDevices()
    {
        return Array.Empty<(string, string)>();
    }
}
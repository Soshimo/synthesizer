namespace SynthesizerUI.Services.Interface;

public interface IAudioDeviceService
{
    public IEnumerable<(string, string)> GetActiveDevices();

}
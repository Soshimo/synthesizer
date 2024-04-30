namespace SynthesizerUI.Services.Interface;

public interface IMIDIDeviceService
{
    event EventHandler<MidiDeviceEventArgs> DeviceConnected;
    event EventHandler<MidiDeviceEventArgs> DeviceRemoved;

    void StartDevice(int deviceIndex);
    void StopDevice(int deviceIndex);
}
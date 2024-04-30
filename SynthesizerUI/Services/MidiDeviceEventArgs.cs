using SynthesizerUI.Model;

namespace SynthesizerUI.Services;

public class MidiDeviceEventArgs : EventArgs
{
    public int DeviceIndex;
    public MidiDeviceInfo Info;

    public MidiDeviceEventArgs(int deviceIndex, MidiDeviceInfo info)
    {
        DeviceIndex = deviceIndex;
        Info = info;
    }
}
using NAudio.Midi;
using SynthesizerUI.Model;
using SynthesizerUI.Services.Interface;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace SynthesizerUI.Services;

public class MIDIDeviceService : IMIDIDeviceService
{
    public event EventHandler<MidiDeviceEventArgs>? DeviceConnected;
    public event EventHandler<MidiDeviceEventArgs>? DeviceRemoved;

    private readonly BackgroundWorker _worker;
    private List<MidiDeviceInfo> _previousDevices = new();
    private List<MidiDeviceInfo> _availableDevices = new();

    public MIDIDeviceService()
    {
        _worker = new BackgroundWorker();
        _worker.DoWork += Worker_DoWork;
        _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        _worker.RunWorkerAsync();

    }
    public void StartDevice(int deviceIndex)
    {
        throw new NotImplementedException();
    }

    public void StopDevice(int deviceIndex)
    {
        throw new NotImplementedException();
    }

    protected virtual void OnDeviceConnected(MidiDeviceEventArgs e)
    {
        DeviceConnected?.Invoke(this, e);
    }

    protected virtual void OnDeviceRemoved(MidiDeviceEventArgs e)
    {
        DeviceRemoved?.Invoke(this, e);
    }

    private List<MidiDeviceInfo> UpdateAvailableDevices()
    {
        var currentDevices = new List<MidiDeviceInfo>();
        for (var i = 0; i < MidiIn.NumberOfDevices; i++)
        {
            var info = new MidiDeviceInfo { Id = i, Name = MidiIn.DeviceInfo(i).ProductName };
            currentDevices.Add(info);
        }
        return currentDevices;
    }

    private void Worker_DoWork(object? sender, DoWorkEventArgs e)
    {
        while (true)
        {
            Thread.Sleep(500); // Adjust polling interval as needed
            var currentDevices = UpdateAvailableDevices();

            // Custom comparer
            var comparer = new MidiDeviceInfoComparer();

            // Compare lists
            var addedDevices = currentDevices.Except(_previousDevices, comparer).ToList();
            var removedDevices = _previousDevices.Except(currentDevices, comparer).ToList();

            if (addedDevices.Any() || removedDevices.Any())
            {
                e.Result = new { Added = addedDevices, Removed = removedDevices }; // Signal a change and data
                break;
            }

            _previousDevices = currentDevices;
        }
    }

    private void Worker_RunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
    {
        if (e.Result != null)
        {
            var changes = (dynamic)e.Result; // Cast to access properties

            if (changes.Removed.Count != 0)
            {
                // Remove devices (by index)
                for (var i = _previousDevices.Count - 1; i >= 0; i--) // iterate backwards for removals
                {
                    if (!changes.Removed.Contains(_previousDevices[i])) continue;

                    OnDeviceRemoved(new MidiDeviceEventArgs(i, _previousDevices[i]));
                    _availableDevices.RemoveAt(i);
                    //_synchronizationContext?.Post(state =>
                    //{
                    //    if (state == null) return;

                    //    var index = (int)state;
                    //    _availableDevices.RemoveAt(index);

                    //}, i);

                    //if (_previousDevices[i].Name != SelectedDevice?.Name) continue;

                    //// Selected device removed
                    //MessageBox.Show("The selected MIDI device has been removed.", "Device Removed", MessageBoxButton.OK);
                    //SelectedDevice = null; // Update UI
                }
            }

            if (changes.Added.Count != 0)
            {
                // Add new devices
                foreach (var device in changes.Added)
                {
                    var deviceCapture = device as MidiDeviceInfo;

                    if (deviceCapture == null) continue;

                    OnDeviceConnected(new MidiDeviceEventArgs(deviceCapture.Id, deviceCapture));
                    _availableDevices.Add(device);
                    //_synchronizationContext?.Post(state =>
                    //{
                    //    if (state is not MidiDeviceInfo deviceToAdd) return;
                    //    AvailableDevices.Add(deviceToAdd);
                    //}, deviceCapture);
                }
            }
            _previousDevices = UpdateAvailableDevices(); // Update previous for next iteration
        }

        _worker.RunWorkerAsync(); // Restart polling
    }
}
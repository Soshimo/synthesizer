using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NAudio.CoreAudioApi;
using NAudio.Midi;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    private readonly BackgroundWorker _worker;

    private readonly SynchronizationContext? _synchronizationContext = SynchronizationContext.Current;

    private float _modFrequency = 0;
    private string _selectedModShape = "saw";
    private float _osc1Tremolo = 0;
    private float _osc2Tremolo = 0;

    private string _osc1Waveform = "sine";
    private string _osc1Octave = "16'";

    private string _osc2Waveform = "sine";
    private string _osc2Octave = "8'";

    private float _osc1Detune = 0;
    private float _osc2Detune = 0;
    private float _osc1Mix = 0;
    private float _osc2Mix = 0;

    private float _volumeEnvelopeAttack = 0;
    private float _volumeEnvelopeDecay = 0;
    private float _volumeEnvelopeSustain = 0;
    private float _volumeEnvelopeRelease = 0;

    private float _filterCutoff = 0;
    private float _filterResonance = 0;
    private float _filterMod = 0;
    private float _filterEnvelope = 0;

    private float _masterDrive;
    private float _masterVolume;
    private float _masterReverb;

    public string[] OscillatorShapes { get; } = new[] { "sine", "square", "saw", "triangle" };

    private string _keyboardOctave = "Normal";
   
    private int _baseOctave = 2;

    private readonly ISynthesizerService _synthesizerService;

    private readonly ObservableCollection<MidiDeviceInfo> _availableDevices;

    private MidiDeviceInfo? _selectedDevice;
    private List<MidiDeviceInfo> _previousDevices;
    private MidiIn? _midiIn;

    private readonly IDialogService _dialogService;

    public MainWindowViewModel(ISynthesizerService synthesizerService, IDialogService dialogService, ILogger<MainWindowViewModel> logger)
    {
        _synthesizerService = synthesizerService;
        _dialogService = dialogService;

        AvailableDevices = new ObservableCollection<MidiDeviceInfo>();
        AvailableDevices = new ObservableCollection<MidiDeviceInfo>();

        _worker = new BackgroundWorker();
        _worker.DoWork += Worker_DoWork;
        _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        _worker.RunWorkerAsync();

        _previousDevices = new List<MidiDeviceInfo>(AvailableDevices);

        MidiDeviceChangedCommand = new RelayCommand(() =>
        {
            if (SelectedDevice == null)
            {
                try
                {
                    _midiIn?.Stop();

                }
                catch (Exception ex)
                {

                }

                if (_midiIn != null)
                {
                    _midiIn.MessageReceived -= MidiIn_MessageReceived;
                    _midiIn.ErrorReceived -= MidiIn_ErrorReceived;
                }

                _midiIn?.Dispose();
                _midiIn = null;
                return;
            }

            _midiIn = new MidiIn(SelectedDevice.Id);
            _midiIn.MessageReceived += MidiIn_MessageReceived;
            _midiIn.ErrorReceived += MidiIn_ErrorReceived;
            _midiIn.Start();

            //_dialogService.ShowDialog<DialogTemplates.Notification>(result =>
            //{
            //    if (!bool.TryParse(result, out var dialogResult)) return;
            //    if (!dialogResult)
            //    {
            //        SelectedDevice = null;
            //        return;
            //    }

            //    _midiIn = new MidiIn(SelectedDevice.Id);
            //    _midiIn.MessageReceived += MidiIn_MessageReceived;
            //    _midiIn.ErrorReceived += MidiIn_ErrorReceived;
            //    _midiIn.Start();
            //});
        });
    }

    public ObservableCollection<MidiDeviceInfo> AvailableDevices
    {
        get => _availableDevices;
        private init => SetProperty(ref _availableDevices, value);
    }
    public float FilterCutoff
    {
        get => _filterCutoff;
        set => SetProperty(ref _filterCutoff, value);
    }
    public float FilterResonance
    {
        get => _filterResonance;
        set => SetProperty(ref _filterResonance, value);
    }
    public float FilterMod
    {
        get => _filterMod;
        set => SetProperty(ref _filterMod, value);
    }
    public float FilterEnvelopeMix
    {
        get => _filterEnvelope;
        set => SetProperty(ref _filterEnvelope, value);
    }
    public float VolumeEnvelopeRelease
    {
        get => _volumeEnvelopeRelease; 
        set => SetProperty(ref _volumeEnvelopeRelease, value);
    }
    public float VolumeEnvelopeSustain
    {
        get => _volumeEnvelopeSustain;
        set => SetProperty(ref _volumeEnvelopeSustain, value);
    }
    public float VolumeEnvelopeDecay
    {
        get => _volumeEnvelopeDecay;
        set => SetProperty(ref _volumeEnvelopeDecay, value);
    }

    public float VolumeEnvelopeAttack
    {
        get => _volumeEnvelopeAttack;
        set => SetProperty(ref _volumeEnvelopeAttack, value);
    }
    public MidiDeviceInfo? SelectedDevice
    {
        get => _selectedDevice;
        set => SetProperty(ref _selectedDevice, value);
    }
    public string KeyboardOctave
    {
        get => _keyboardOctave;
        set => SetProperty(ref _keyboardOctave, value);
    }
    public float Osc1Detune
    {
        get => _osc1Detune;
        set => SetProperty(ref _osc1Detune, value);
    }

    public float Osc2Detune
    {
        get => _osc2Detune;
        set => SetProperty(ref _osc2Detune, value);
    }

    public float Osc1Mix
    {
        get => (float)_osc1Mix;
        set => SetProperty(ref _osc1Mix, value);
    }
    public float Osc2Mix
    {
        get => (float)_osc2Mix;
        set => SetProperty(ref _osc2Mix, value);
    }

    public float Osc1Tremolo
    {
        get => _osc1Tremolo;
        set => SetProperty(ref _osc1Tremolo, value);
    }

    public float Osc2Tremolo
    {
        get => _osc2Tremolo;
        set => SetProperty(ref _osc2Tremolo, value);
    }

    public float ModFrequency
    {
        get => _modFrequency;
        set => SetProperty(ref _modFrequency, value);
    }

    public int BaseOctave
    {
        get => _baseOctave;
        set => SetProperty(ref _baseOctave, value);
    }
    public string[] Shapes => OscillatorShapes;
    public string[] KeyboardOctaves { get; } = new[] { "+3", "+2", "+1", "Normal", "-1", "-2", "-3" };

    public string SelectedShape
    {
        get => _selectedModShape;
        set => SetProperty(ref _selectedModShape, value);
    }

    public string Osc1Waveform
    {
        get => _osc1Waveform;
        set => SetProperty(ref _osc1Waveform, value);
    }


    public string[] Osc1Octaves { get; } = new[] { "32'", "16'", "8'" };


    public string Osc1Octave
    {
        get => _osc1Octave;
        set => SetProperty(ref _osc1Octave, value);
    }
    public string Osc2Waveform
    {
        get => _osc2Waveform;
        set => SetProperty(ref _osc2Waveform, value);
    }

    public float MasterDrive
    {
        get => _masterDrive;
        set => SetProperty(ref _masterDrive, value);
    }

    public float MasterVolume
    {
        get => _masterVolume;
        set => SetProperty(ref _masterVolume, value);
    }

    public float MasterReverb
    {
        get => _masterReverb;
        set => SetProperty(ref _masterReverb, value);
    }


    public string[] Osc2Octaves { get; } = new[] { "16'", "8'", "4'" };

    public string Osc2Octave
    {
        get => _osc2Octave;
        set => SetProperty(ref _osc2Octave, value);
    }

    public ICommand MidiDeviceChangedCommand { get; }

    public void ReleaseKey(string note, int keyboardOctave)
    {
        var key = $"{note}{keyboardOctave + BaseOctave}";
        var (index, octave) = NoteHelper.ParseNoteString(key);

        _synthesizerService.NoteOff(note);
    }

    public void PressKey(string note, int keyboardOctave)
    {
        var key = $"{note}{keyboardOctave + BaseOctave}";
        var (index, octave) = NoteHelper.ParseNoteString(key);

        var frequency = (float)NoteHelper.NoteToFrequency(index, octave);
        var data = GetVoiceData(frequency);

        _synthesizerService.NoteOn(note, data);
    }

    private void MidiIn_ErrorReceived(object? sender, MidiInMessageEventArgs e)
    {
    }

    private void MidiIn_MessageReceived(object? sender, MidiInMessageEventArgs e)
    {
        if (e.MidiEvent.CommandCode == MidiCommandCode.NoteOn && e.MidiEvent.Channel == 1)
        {
            if (e.MidiEvent is not NoteOnEvent note) return;

            var key = note.NoteName;

            var (index, octave) = NoteHelper.ParseNoteString(note.NoteName);
            var frequency = NoteHelper.NoteToFrequency(note.NoteNumber);

            var data = GetVoiceData((float)frequency);

            _synthesizerService.NoteOn(key, data);
        }
        else if(e.MidiEvent.CommandCode == MidiCommandCode.NoteOff && e.MidiEvent.Channel == 1)
        {
            if (e.MidiEvent is not NoteEvent note) return;

            var key = note.NoteName;
            _synthesizerService.NoteOff(key);
        }
    }

    private List<MidiDeviceInfo> UpdateAvailableDevices()
    {
        var currentDevices = new List<MidiDeviceInfo>();
        for (int i = 0; i < MidiIn.NumberOfDevices; i++)
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
            if (changes.Added.Count != 0)
            {
                // Add new devices
                foreach (var device in changes.Added)
                {
                    var deviceCapture = device as MidiDeviceInfo;

                    _synchronizationContext?.Post(state =>
                    {
                        if (state is not MidiDeviceInfo deviceToAdd) return;
                        AvailableDevices.Add(deviceToAdd);
                    }, deviceCapture);
                }
            }

            if (changes.Removed.Count != 0)
            {
                // Remove devices (by index)
                for (var i = _previousDevices.Count - 1; i >= 0; i--) // iterate backwards for removals
                {
                    if (!changes.Removed.Contains(_previousDevices[i])) continue;

                    _synchronizationContext?.Post(state =>
                    {
                        if (state == null) return;

                        var index = (int)state;
                        AvailableDevices.RemoveAt(index);

                    }, i);

                    if (_previousDevices[i].Name != SelectedDevice?.Name) continue;

                    // Selected device removed
                    MessageBox.Show("The selected MIDI device has been removed.", "Device Removed", MessageBoxButton.OK);
                    SelectedDevice = null; // Update UI
                }
            }

            _previousDevices = UpdateAvailableDevices(); // Update previous for next iteration
        }

        _worker.RunWorkerAsync(); // Restart polling
    }


    private VoiceData GetVoiceData(float frequency)
    {
        return new VoiceData(frequency, .01f, .1f, .7f, .1f);
    }


}
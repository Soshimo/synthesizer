using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NAudio.Midi;
using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI.ViewModel;

// ReSharper disable once ClassNeverInstantiated.Global
public class SynthesizerPageViewModel : ViewModelBase
{
    private readonly SynchronizationContext? _synchronizationContext = SynchronizationContext.Current;

    private const int BaseOctave = 4;

    private float _modFrequency = 2.1f;
    private OscillatorShape _selectedModShape;
    private float _osc1Tremolo = 15;
    private float _osc2Tremolo = 17;

    private OscillatorShape _osc1Waveform;
    private OctaveSetting _osc1Octave;

    private OscillatorShape _osc2Waveform;
    private OctaveSetting _osc2Octave;

    private float _osc1Detune;
    private float _osc2Detune = -25;
    private float _osc1Mix = 50;
    private float _osc2Mix = 50;

    private float _volumeEnvelopeAttack = .02f;
    private float _volumeEnvelopeDecay = .15f;
    private float _volumeEnvelopeSustain = .68f;
    private float _volumeEnvelopeRelease = .05f;

    private float _filterCutoff = 256;
    private float _filterResonance = 7;
    private float _filterMod = 21;
    private float _filterEnvelope = 56;

    private float _masterDrive = 38;
    private float _masterVolume = 75;
    private float _masterReverb = 32;

    private KeyboardOctave? _keyboardOctave;

    private readonly ISynthesizerService _synthesizerService;

    private MidiDeviceInfo? _selectedDevice;
    //private List<MidiDeviceInfo> _previousDevices;
    private MidiIn? _midiIn;


    // ReSharper disable once NotAccessedField.Local
    private readonly IDialogService _dialogService;
    private readonly ILogger<SynthesizerPageViewModel> _logger;

    private string? _currentNote;

    public SynthesizerPageViewModel(ISynthesizerService synthesizerService, IDialogService dialogService, ILogger<SynthesizerPageViewModel> logger, IMIDIDeviceService midiDeviceService)
    {
        _synthesizerService = synthesizerService;
        _dialogService = dialogService;

        _selectedModShape = OscillatorShapes[0];
        _osc1Waveform = OscillatorShapes[0];
        _osc2Waveform = OscillatorShapes[0];
           
        _osc1Octave = Osc1Octaves[0];
        _osc2Octave = Osc2Octaves[0];
        _logger = logger;

        AvailableDevices = new ObservableCollection<MidiDeviceInfo>();

        KeyboardOctaves = new ObservableCollection<KeyboardOctave?>
        {
            new() { Display = "-3", Offset = -3 },
            new() { Display = "-2", Offset = -2 },
            new() { Display = "-1", Offset = -1 },
            new() { Display = "Normal", Offset = 0 },
            new() { Display = "+1", Offset = 1 },
            new() { Display = "+2", Offset = 2 },
            new() { Display = "+3", Offset = 3 }
        };

        KeyboardOctave = KeyboardOctaves.First(k => k.Display == "Normal");

        midiDeviceService.DeviceConnected += (sender, args) =>
        {
            _synchronizationContext?.Post(state =>
            {
                if (state is not MidiDeviceInfo info) return;
                AvailableDevices.Add(info);
            }, args.Info);
        };

        midiDeviceService.DeviceRemoved += (sender, args) =>
        {
            var device = AvailableDevices.FirstOrDefault(d => d.Name == args.Info.Name);
            if (device == null) return;
            _synchronizationContext?.Post(state =>
            {
                if (state is not MidiDeviceInfo info) return;

                if (SelectedDevice?.Name == info.Name)
                {
                    MessageBox.Show($"{info.Name} has been disconnected.");
                }

                AvailableDevices.Remove(info);
                SelectedDevice = null;
            
            }, device);
        };

        _synthesizerService.SetDrive(_masterDrive);

        MidiDeviceChangedCommand = new RelayCommand(MidiDeviceChanged);

        KnobChangedCommand = new RelayCommand<string>((st) =>
        {
            // TODO: convert to an Enum
            if (st == "MasterDrive")
            {
                _synthesizerService.SetDrive(_masterDrive);
            }
        });
    }

    private void MidiDeviceChanged()
    {
        if (SelectedDevice == null)
        {
            try
            {
                _midiIn?.Stop();

            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to stop the Midi device. {0}", ex);
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
    }

    public ObservableCollection<MidiDeviceInfo> AvailableDevices { get; }
    public ObservableCollection<KeyboardOctave?> KeyboardOctaves { get; }

    public OscillatorShape[] OscillatorShapes { get; } = { new() {Display = "sine", Value = Waveform.Sine}, new () { Display = "square", Value = Waveform.Square}, new() { Display = "saw", Value = Waveform.Sine }, new() { Display = "triangle", Value = Waveform.Sine } };

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
    public KeyboardOctave? KeyboardOctave
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
        get => _osc1Mix;
        set => SetProperty(ref _osc1Mix, value);
    }
    public float Osc2Mix
    {
        get => _osc2Mix;
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
    public OscillatorShape SelectedShape
    {
        get => _selectedModShape;
        set => SetProperty(ref _selectedModShape, value);
    }

    public OscillatorShape Osc1Waveform
    {
        get => _osc1Waveform;
        set => SetProperty(ref _osc1Waveform, value);
    }


    public OctaveSetting[] Osc1Octaves { get; } = new OctaveSetting[]
    {
        new() { Display = "32'", Index = 0 }, new() { Display = "16'", Index = 1 }, new() { Display = "8'", Index = 2 }
    };


    public OctaveSetting Osc1Octave
    {
        get => _osc1Octave;
        set => SetProperty(ref _osc1Octave, value);
    }
    public OscillatorShape Osc2Waveform
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

    public OctaveSetting[] Osc2Octaves { get; } = new OctaveSetting[]
        { new() { Display = "16'", Index = 0}, new() { Display = "8'" , Index = 1}, new() {Display = "4'", Index = 2} };

    public OctaveSetting Osc2Octave
    {
        get => _osc2Octave;
        set => SetProperty(ref _osc2Octave, value);
    }

    public ICommand MidiDeviceChangedCommand { get; }
    public string? CurrentNote { get => _currentNote;
        set => SetProperty(ref _currentNote, value);
    }

    public ICommand KnobChangedCommand { get; }

    public void ReleaseKey(string note, int noteIndex)
    {
        var actualNoteIndex = noteIndex + ((BaseOctave + KeyboardOctave.Offset) * 12);
        NoteOff(actualNoteIndex);
    }

    public void PressKey(string note, int noteIndex)
    {
        var actualNoteIndex = noteIndex + ((BaseOctave + KeyboardOctave.Offset) * 12);
        NoteOn(actualNoteIndex);
    }

    private void NoteOn(int noteIndex)
    {
        //var key = $"{note}{noteIndex}";

        var frequency = (float)NoteHelper.NoteToFrequency(noteIndex);

        var data = GetVoiceData(frequency);
        _synthesizerService.NoteOn(noteIndex, data);
    }

    private void NoteOff(int noteIndex)
    {
        //var key = $"{note}{noteIndex}";
        _synthesizerService.NoteOff(noteIndex);
    }

    private void MidiIn_ErrorReceived(object? sender, MidiInMessageEventArgs e)
    {
    }

    private void MidiIn_MessageReceived(object? sender, MidiInMessageEventArgs e)
    {
        switch (e.MidiEvent.CommandCode)
        {
            case MidiCommandCode.NoteOn when e.MidiEvent.Channel == 1:
            {
                if (e.MidiEvent is not NoteOnEvent note) return;

                CurrentNote = note.NoteName;
                NoteOn(note.NoteNumber);
                break;
            }
            case MidiCommandCode.NoteOff when e.MidiEvent.Channel == 1:
            {
                if (e.MidiEvent is not NoteEvent note) return;

                CurrentNote = "";
                NoteOff(note.NoteNumber);
                break;
            }
            case MidiCommandCode.KeyAfterTouch:
                break;
            case MidiCommandCode.ControlChange:
                break;
            case MidiCommandCode.PatchChange:
                break;
            case MidiCommandCode.ChannelAfterTouch:
                break;
            case MidiCommandCode.PitchWheelChange:
                break;
            case MidiCommandCode.Sysex:
                break;
            case MidiCommandCode.Eox:
                break;
            case MidiCommandCode.TimingClock:
                break;
            case MidiCommandCode.StartSequence:
                break;
            case MidiCommandCode.ContinueSequence:
                break;
            case MidiCommandCode.StopSequence:
                break;
            case MidiCommandCode.AutoSensing:
                break;
            case MidiCommandCode.MetaEvent:
                break;
        }
    }

    private VoiceData GetVoiceData(float rootFrequency)
    {
        var frequency1 = (float)(rootFrequency * Math.Pow(2, _osc1Octave.Index - 2));
        var frequency2 = (float)(rootFrequency * Math.Pow(2, _osc2Octave.Index - 1));

        return new VoiceData()
        {
            RootFrequency = rootFrequency,
            Oscillator1Frequency = frequency1,
            Oscillator2Frequency = frequency2,
            Oscillator1Detune = Osc1Detune,
            Oscillator2Detune = Osc1Detune,
            FilterCutoff = FilterCutoff,
            FilterResonance = FilterResonance,
            VolumeEnvelopeAttack = VolumeEnvelopeAttack,
            VolumeEnvelopeDecay = VolumeEnvelopeDecay,
            VolumeEnvelopeRelease = VolumeEnvelopeRelease,
            VolumeEnvelopeSustain = VolumeEnvelopeSustain,
            ModFrequency = ModFrequency,
            ModOscillator1 = Osc1Tremolo,
            ModOscillator2 = Osc2Tremolo,
            ModWaveShape = SelectedShape.Value
        };
    }


}
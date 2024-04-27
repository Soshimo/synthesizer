using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public class MainWindowViewModel : ObservableObject
{

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

    public string[] OscillatorShapes { get; } = new[] { "sine", "square", "saw", "triangle" };

    public ObservableCollection<string> MidiDevices { get; }

    private string _keyboardOctave = "0";
    private string _selectedDevice = "";
    public ObservableCollection<PianoKeyViewModel> PianoKeys { get; } = new();
    
    private int _value = 25;

    private int _baseOctave = 2;

    private readonly ISynthesizerService _synthesizerService;

    //private readonly List<Key> _assignedKeyList =
    //[
    //    Key.A,
    //    Key.W,
    //    Key.S,
    //    Key.E,
    //    Key.D,
    //    Key.F,
    //    Key.T,
    //    Key.G,
    //    Key.Y,
    //    Key.H,
    //    Key.U,
    //    Key.J,
    //    Key.K,
    //    Key.I,
    //    Key.L,
    //    Key.O,
    //    Key.OemSemicolon,
    //    Key.OemQuotes,
    //    Key.P,
    //    Key.Enter,
    //    Key.OemOpenBrackets,
    //    Key.B,
    //    Key.N,
    //    Key.M
    //];

    //private readonly List<bool> _isBlackList =
    //[
    //    false, true, false, true, false, false, true, false, true, false, true, false, false, true, false, true,
    //    false, false, true, false, true, false, true, false
    //];

    //private readonly List<string> _noteList =
    //[
    //    "C", "C#", "D", "D#", "E", "F",
    //    "F#", "G", "G#", "A", "A#", "B",
    //    "C", "C#", "D", "D#", "E", "F",
    //    "F#", "G", "G#", "A", "A#", "B"
    //];

    //public ICommand KeyPressCommand { get; }
    //public ICommand KeyReleaseCommand { get; }

    public MainWindowViewModel(ISynthesizerService synthesizerService)
    {
        _synthesizerService = synthesizerService;

        MidiDevices = new ObservableCollection<string>();

        //InitializeKeyboard();

        //KeyReleaseCommand = new RelayCommand<PianoKeyViewModel>(model =>
        //{
        //    var note = $"{model?.Note}{model?.Octave + BaseOctave}";
        //    _synthesizerService.NoteOff(note);
        //});

        //KeyPressCommand = new RelayCommand<PianoKeyPressedEventArgs>(args =>
        //{
        //    var model = args?.DataContext;

        //    var note = $"{model?.Note}{model?.Octave + BaseOctave}";
        //    var (index, octave) = NoteHelper.ParseNoteString(note);
        //    var frequency = (float)NoteHelper.NoteToFrequency(index, octave);

        //    var data = GetVoiceData(frequency);

        //    _synthesizerService.NoteOn(note, data);
        //});
    }


    private VoiceData GetVoiceData(float frequency)
    {
        return new VoiceData(frequency, .01f, .1f, .7f, .1f);
    }


    //private void InitializeKeyboard()
    //{
    //    const int numKeys = 24;

    //    var octave = 0;
    //    for (var i = 0; i < numKeys; i++)
    //    {
    //        if (_noteList[i] == "A") octave++;

    //        var key = new PianoKeyViewModel()
    //        {
    //            AssignedKey = _assignedKeyList[i],
    //            IsBlack = _isBlackList[i],
    //            Note = _noteList[i],
    //            Octave = octave,
    //            Index = i
    //        };

    //        key.KeyPressed += Piano_KeyPressed;
    //        key.KeyReleased += Piano_KeyReleased;

    //        PianoKeys.Add(key);
    //    }
    //}

    //private void Piano_KeyReleased(object? sender, KeyPressedEventArgs e)
    //{
    //    var note = $"{e.Note}{e.Octave + BaseOctave}";
    //    _synthesizerService.NoteOff(note);
    //}

    //private void Piano_KeyPressed(object? sender, KeyPressedEventArgs e)
    //{
    //    var note = $"{e.Note}{e.Octave + BaseOctave}";

    //    var (index, octave) = NoteHelper.ParseNoteString(note);
    //    var frequency = (float)NoteHelper.NoteToFrequency(index, octave);

    //    _synthesizerService.NoteOn(note, new VoiceData(frequency, .01f, .1f, .7f, .1f));
    //}


    public string SelectedDevice
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
    public int Value
    {
        get => _value;
        set => SetProperty(ref _value, value);
    }
    public string[] Shapes => OscillatorShapes;
    public string[] KeyboardOctaves { get; } = new[] { "+3", "+2", "+1", "0", "-1", "-2", "-3" };

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


    public string[] Osc2Octaves { get; } = new[] { "16'", "8'", "4'" };


    public string Osc2Octave
    {
        get => _osc2Octave;
        set => SetProperty(ref _osc2Octave, value);
    }

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

}


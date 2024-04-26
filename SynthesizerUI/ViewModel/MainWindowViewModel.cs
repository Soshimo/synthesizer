using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public class MainWindowViewModel : ObservableObject
{

    public ObservableCollection<PianoKeyViewModel> PianoKeys { get; } = new();
    
    private int _value = 25;

    private int _baseOctave = 2;

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

    private readonly ISynthesizerService _synthesizerService;
    public MainWindowViewModel(ISynthesizerService synthesizerService)
    {
        _synthesizerService = synthesizerService;

        var assignedKeyList = new List<Key>
        {
            Key.A,
            Key.W,
            Key.S,
            Key.E,
            Key.D,
            Key.F,
            Key.T,
            Key.G,
            Key.Y,
            Key.H,
            Key.U,
            Key.J,
            Key.K,
            Key.I,
            Key.L,
            Key.O,
            Key.OemSemicolon,
            Key.OemQuotes,
            Key.P,
            Key.Enter,
            Key.OemOpenBrackets,
            Key.B,
            Key.N,
            Key.M
        };

        var isBlackList = new List<bool>
        {
            false, true, false, true, false, false, true, false, true, false, true, false, false, true, false, true,
            false, false, true, false, true, false, true, false
        };

        var noteList = new List<string>
        {
            "C", "C#", "D", "D#", "E", "F", 
            "F#", "G", "G#", "A", "A#", "B", 
            "C", "C#", "D", "D#", "E", "F", 
            "F#", "G", "G#", "A", "A#", "B"
        };

        for (var i = 0; i < 24; i++)
        {
            var key = new PianoKeyViewModel()
            {
                AssignedKey = assignedKeyList[i],
                IsBlack = isBlackList[i],
                Note = noteList[i],
                UpperRegister = i >= 12
            };

            key.KeyPressed += Piano_KeyPressed;
            key.KeyReleased += Piano_KeyReleased;

            PianoKeys.Add(key);
        }
    }

    private void Piano_KeyReleased(object? sender, KeyPressedEventArgs e)
    {
        var note = $"{e.Note}{(e.UpperRegister ? BaseOctave + 1 : BaseOctave)}";
        _synthesizerService.NoteOff(note);
    }

    private void Piano_KeyPressed(object? sender, KeyPressedEventArgs e)
    {
        var note = $"{e.Note}{(e.UpperRegister ? BaseOctave + 1 : BaseOctave)}";

        var (index, octave) = NoteHelper.ParseNoteString(note);
        var frequency = (float)NoteHelper.NoteToFrequency(index, octave);

        _synthesizerService.NoteOn(note, new VoiceData(frequency, .01f, .1f, .7f, .1f));
    }
}


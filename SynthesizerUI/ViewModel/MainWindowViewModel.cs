using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public class MainWindowViewModel : ObservableObject
{

    public ObservableCollection<PianoKeyViewModel> PianoKeys { get; } = new();
    
    private int _value = 25;

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
                Octave = i < 12 ? 0 : 1
            };

            key.KeyPressed += Key_KeyPressed;
            key.KeyReleased += Key_KeyReleased;

            PianoKeys.Add(key);
        }
   
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.A, IsBlack = false, Note = "C", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.W, IsBlack = true, Note = "C#", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.S, IsBlack = false, Note = "D", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.E, IsBlack = true, Note = "D#", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.D, IsBlack = false, Note = "E", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.F, IsBlack = false, Note = "F", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.T, IsBlack = true, Note = "F#", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.G, IsBlack = false, Note = "G", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.Y, IsBlack = true, Note = "G#", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.H, IsBlack = false, Note = "A", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.U, IsBlack = true, Note = "A#", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.J, IsBlack = false, Note = "B", Octave = 0 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.K, IsBlack = false, Note = "C", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.I, IsBlack = true, Note = "C#", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.L, IsBlack = false, Note = "D", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.O, IsBlack = true, Note = "D#", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.OemSemicolon, IsBlack = false, Note = "E", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.OemQuotes, IsBlack = false, Note = "F", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.P, IsBlack = true, Note = "F#", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.Enter, IsBlack = false, Note = "G", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.OemOpenBrackets, IsBlack = true, Note = "G#", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.B, IsBlack = false, Note = "A", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.N, IsBlack = true, Note = "A#", Octave = 1 });
        //PianoKeys.Add(new PianoKeyViewModel { AssignedKey = Key.M, IsBlack = false, Note = "B", Octave = 1 });
    }

    private void Key_KeyReleased(object? sender, KeyEventArgs e)
    {
    }

    private void Key_KeyPressed(object? sender, KeyEventArgs e)
    {
    }
}


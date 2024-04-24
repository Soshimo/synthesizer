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
    public double TotalKeysWidth => 560;

    private readonly ISynthesizerService _synthesizerService;
    public MainWindowViewModel(ISynthesizerService synthesizerService)
    {
        _synthesizerService = synthesizerService;

        PianoKeys.Add(new PianoKeyViewModel(synthesizerService) { AssignedKey = Key.A, IsBlack = false, Note = "C", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel(synthesizerService) { AssignedKey = Key.W, IsBlack = true, Note = "C#", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel(synthesizerService) { AssignedKey = Key.S, IsBlack = false, Note = "D", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.E, IsBlack = true, Note = "D#", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.D, IsBlack = false, Note = "E", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.F, IsBlack = false, Note = "F", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.T, IsBlack = true, Note = "F#", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.G, IsBlack = false, Note = "G", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.Y, IsBlack = true, Note = "G#", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.H, IsBlack = false, Note = "A", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.U, IsBlack = true, Note = "A#", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.J, IsBlack = false, Note = "B", Octave = 0 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.K, IsBlack = false, Note = "C", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.I, IsBlack = true, Note = "C#", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.L, IsBlack = false, Note = "D", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.O, IsBlack = true, Note = "D#", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) 
            { AssignedKey = Key.OemSemicolon, IsBlack = false, Note = "E", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.OemQuotes, IsBlack = false, Note = "F", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.P, IsBlack = true, Note = "F#", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.Enter, IsBlack = false, Note = "G", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) 
            { AssignedKey = Key.OemOpenBrackets, IsBlack = true, Note = "G#", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.B, IsBlack = false, Note = "A", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.N, IsBlack = true, Note = "A#", Octave = 1 });
        PianoKeys.Add(new PianoKeyViewModel (synthesizerService) { AssignedKey = Key.M, IsBlack = false, Note = "B", Octave = 1 });
    }
}


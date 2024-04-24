using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public class PianoKeyViewModel : ObservableObject
{
    private string _note;
    private bool _isBlack;
    private int _octave;

    private readonly ISynthesizerService _synthesizerService;

    public int Octave
    {
        get => _octave;
        set => SetProperty(ref _octave, value);
    }

    public string Note
    {
        get => _note; 
        set => SetProperty(ref _note, value);
    }
    public bool IsBlack { get => _isBlack; set => SetProperty(ref _isBlack, value); }
    public Key AssignedKey { get; set; }  // System.Windows.Input.Key enum value

    public ICommand KeyPressCommand { get; }
    public ICommand KeyReleaseCommand { get; }

    private bool _isPressed;

    public bool IsPressed
    {
        get => _isPressed;
        set => SetProperty(ref _isPressed, value);
    }

    public PianoKeyViewModel(ISynthesizerService synthesizerService)
    {
        _note = "";

        _synthesizerService = synthesizerService;

        KeyReleaseCommand = new RelayCommand(ExecuteKeyRelease);
        KeyPressCommand = new RelayCommand(ExecuteKeyPress);

    }

    private void ExecuteKeyPress()
    {
        IsPressed = true;

        // play sound
        _synthesizerService.NoteOn(Note,  Octave);
    }

    private void ExecuteKeyRelease()
    {
        IsPressed = false;

        // Stop sound
        _synthesizerService.NoteOff(Note, Octave);
    }
}
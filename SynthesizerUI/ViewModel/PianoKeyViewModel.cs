using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public class PianoKeyViewModel : ObservableObject
{
    private string _note;
    private bool _isBlack;
    private int _octave;
    private bool _isPressed;

    //private readonly ISynthesizerService _synthesizerService;

    // Define the events
    public event EventHandler<KeyEventArgs> KeyPressed;
    public event EventHandler<KeyEventArgs> KeyReleased;

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

    public bool IsPressed
    {
        get => _isPressed;
        set => SetProperty(ref _isPressed, value);
    }

    public PianoKeyViewModel()
    {
        _note = "";

        //_synthesizerService = synthesizerService;
        KeyReleaseCommand = new RelayCommand(ExecuteKeyRelease);
        KeyPressCommand = new RelayCommand(ExecuteKeyPress);
    }

    private void ExecuteKeyPress()
    {
        IsPressed = true;

        //var noteString = $"{Note}{Octave + 2}"; // i.e. C0, or C1
        //var (index, actualOctave) = NoteHelper.ParseNoteString(noteString);
        //var frequency = NoteHelper.NoteToFrequency(index, actualOctave);
        // play sound
        //_synthesizerService.NoteOn(new VoiceData(noteString, (float)frequency, .1f, .1f, .4f, .4f));

        OnKeyPressed(new KeyEventArgs(Note, Octave));

    }

    private void ExecuteKeyRelease()
    {
        IsPressed = false;

        //var noteString = $"{Note}{Octave + 2}"; // i.e. C0, or C1
        // Stop sound
        //_synthesizerService.NoteOff(noteString);

        OnKeyReleased(new KeyEventArgs(Note, Octave));
    }

    protected virtual void OnKeyPressed(KeyEventArgs e)
    {
        KeyPressed?.Invoke(this, e);
    }

    protected virtual void OnKeyReleased(KeyEventArgs e)
    {
        KeyReleased?.Invoke(this, e);
    }
}
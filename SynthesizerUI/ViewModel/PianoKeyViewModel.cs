using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services;

namespace SynthesizerUI.ViewModel;

public sealed class PianoKeyViewModel : ObservableObject
{
    private string _note;
    private bool _isBlack;
    private bool _upperRegister;
    private bool _isPressed;

    //private readonly ISynthesizerService _synthesizerService;

    // Define the events
    public event EventHandler<KeyPressedEventArgs>? KeyPressed;
    public event EventHandler<KeyPressedEventArgs>? KeyReleased;

    public bool UpperRegister
    {
        get => _upperRegister;
        set => SetProperty(ref _upperRegister, value);
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

        KeyReleaseCommand = new RelayCommand(ExecuteKeyRelease);
        KeyPressCommand = new RelayCommand(ExecuteKeyPress);
    }

    private void ExecuteKeyPress()
    {
        IsPressed = true;
        OnKeyPressed(new KeyPressedEventArgs(Note, UpperRegister));
    }

    private void ExecuteKeyRelease()
    {
        IsPressed = false;
        OnKeyReleased(new KeyPressedEventArgs(Note, UpperRegister));
    }

    private void OnKeyPressed(KeyPressedEventArgs e)
    {
        KeyPressed?.Invoke(this, e);
    }

    private void OnKeyReleased(KeyPressedEventArgs e)
    {
        KeyReleased?.Invoke(this, e);
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SynthesizerUI.ViewModel;

public class PianoKeyViewModel : ObservableObject
{
    public ICommand KeyPressCommand { get; }
    public ICommand KeyReleaseCommand { get; }

    private bool _isPressed;

    public bool IsPressed
    {
        get => _isPressed;
        set => SetProperty(ref _isPressed, value);
    }

    public PianoKeyViewModel()
    {
        KeyReleaseCommand = new RelayCommand(ExecuteKeyRelease);
        KeyPressCommand = new RelayCommand(ExecuteKeyPress);

    }

    private void ExecuteKeyPress()
    {
        IsPressed = true;

        // play sound
    }

    private void ExecuteKeyRelease()
    {
        IsPressed = false;

        // Stop sound
    }
}
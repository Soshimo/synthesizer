using System.Windows;
using SynthesizerUI.ViewModel;

namespace SynthesizerUI;

public class PianoKeyPressedEventArgs : RoutedEventArgs
{
    public PianoKeyPressedEventArgs(RoutedEvent routedEvent, PianoKeyViewModel buttonDataContext) : base(routedEvent)
    {
        DataContext = buttonDataContext;
    }

    public PianoKeyViewModel DataContext { get; private set; }
}
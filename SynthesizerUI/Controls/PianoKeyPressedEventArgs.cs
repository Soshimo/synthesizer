using System.Windows;
using SynthesizerUI.ViewModel;

namespace SynthesizerUI;

public class PianoKeyPressedEventArgs(RoutedEvent routedEvent, PianoKeyViewModel buttonDataContext) : RoutedEventArgs(routedEvent)
{
    public PianoKeyViewModel DataContext { get; private set; } = buttonDataContext;
}
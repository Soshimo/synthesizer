using System.Windows;
using SynthesizerUI.Controls;

namespace SynthesizerUI.DependencyObjects;

//public static class PianoKeyboardAttachedProperties
//{
//    public static readonly DependencyProperty CurrentNoteProperty =
//        DependencyProperty.RegisterAttached(
//            "CurrentNote",
//            typeof(string),
//            typeof(PianoKeyboardControl),
//            new FrameworkPropertyMetadata(
//                null,
//                FrameworkPropertyChangeCallback_CurrentNoteChanged));

//    private static void FrameworkPropertyChangeCallback_CurrentNoteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is PianoKeyboardControl control)
//        {
//            var newNote = (string)e.NewValue;
//            foreach (var keyViewModel in control.PianoKeys)
//            {
//                keyViewModel.IsPressed = keyViewModel.Note == newNote;
//            }
//        }
//    }

//    public static void SetCurrentNote(UIElement element, string value)
//    {
//        element.SetValue(CurrentNoteProperty, value);
//    }

//    public static string GetCurrentNote(UIElement element)
//    {
//        return (string)element.GetValue(CurrentNoteProperty);
//    }
//}
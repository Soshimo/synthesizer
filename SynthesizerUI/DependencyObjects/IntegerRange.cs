using System.Windows;

namespace SynthesizerUI.DependencyObjects;

public class IntegerRange : DependencyObject
{
    public static readonly DependencyProperty StartProperty =
        DependencyProperty.Register("Start", typeof(int), typeof(IntegerRange), new PropertyMetadata(0));

    public static readonly DependencyProperty EndProperty =
        DependencyProperty.Register("End", typeof(int), typeof(IntegerRange), new PropertyMetadata(0));

    public int Start
    {
        get => (int)GetValue(StartProperty);
        set => SetValue(StartProperty, value);
    }

    public int End
    {
        get => (int)GetValue(EndProperty);
        set => SetValue(EndProperty, value);
    }
}
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace SynthesizerUI.Windows;

public class SynthesizerMainWindow : ModernWindow
{
    public static readonly DependencyProperty IconSourceProperty = DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(ModernWindow), new PropertyMetadata(null));

    public ImageSource IconSource
    {
        get => (ImageSource)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);

    }

}
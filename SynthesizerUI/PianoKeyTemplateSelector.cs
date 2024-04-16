using SynthesizerUI.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace SynthesizerUI;

public class PianoKeyTemplateSelector : DataTemplateSelector
{
    public DataTemplate? WhiteKeyTemplate { get; set; }
    public DataTemplate? BlackKeyTemplate { get; set; }

    public override DataTemplate? SelectTemplate(object? item, DependencyObject container)
    {
        if (item is PianoKeyViewModel viewModel)
        {
            return viewModel.IsBlack ? BlackKeyTemplate : WhiteKeyTemplate;
        }
        return base.SelectTemplate(item, container);
    }
}
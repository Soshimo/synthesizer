using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI.Services;

public class DialogService : IDialogService
{
    //private static Dictionary<Type, Type> _mappings = new Dictionary<Type, Type>();

    public void ShowDialog(string templateName, Action<string> callback)
    {
        var templateType = Type.GetType(templateName);
        if (templateType == null) throw new ArgumentException("Invalid template type", nameof(templateName));

        ShowDialogInternal(templateType, callback);
    }

    public void ShowDialog<TView>(Action<string> callback)
    {
        ShowDialogInternal(typeof(TView), callback);
    }

    private void ShowDialogInternal(Type templateType, Action<string> callback)
    {
        var dialog = new DialogWindow();

        EventHandler closeEventHandler = null;

        closeEventHandler = (s, e) =>
        {
            callback(dialog.DialogResult.ToString() ?? string.Empty);
            dialog.Closed -= closeEventHandler;
        };


        dialog.Closed += closeEventHandler;


        var content = Ioc.Default.GetService(templateType);

        dialog.Content = content;
        dialog.ShowDialog();

    }
}
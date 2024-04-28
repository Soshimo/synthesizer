namespace SynthesizerUI.Services.Interface;

public interface IDialogService
{
    void ShowDialog(string templateName, Action<string> callback);
    void ShowDialog<TView>(Action<string> callback);
}
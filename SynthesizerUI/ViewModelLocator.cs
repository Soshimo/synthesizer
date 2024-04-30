using CommunityToolkit.Mvvm.DependencyInjection;
using SynthesizerUI.ViewModel;

namespace SynthesizerUI;

public class ViewModelLocator
{
    public SynthesizerPageViewModel? SynthesizerPageViewModel => Ioc.Default.GetService<SynthesizerPageViewModel>();
    public NotificationViewModel? NotificationViewModel => Ioc.Default.GetService<NotificationViewModel>();
    public MainViewModel? MainWindowViewModel => Ioc.Default.GetService<MainViewModel>();
}
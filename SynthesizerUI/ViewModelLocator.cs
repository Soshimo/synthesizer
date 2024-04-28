using CommunityToolkit.Mvvm.DependencyInjection;
using SynthesizerUI.ViewModel;

namespace SynthesizerUI;

public class ViewModelLocator
{
    public MainWindowViewModel? MainWindowViewModel => Ioc.Default.GetService<MainWindowViewModel>();
    public NotificationViewModel? NotificationViewModel => Ioc.Default.GetService<NotificationViewModel>();
}
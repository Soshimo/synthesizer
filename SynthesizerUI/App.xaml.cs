using CommunityToolkit.Mvvm.DependencyInjection;
using SynthesizerUI.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;

namespace SynthesizerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<ISynthesizerService, SynthesizerService>()
                    .BuildServiceProvider()
            );
        }

    }

}

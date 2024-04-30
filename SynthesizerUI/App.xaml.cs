using CommunityToolkit.Mvvm.DependencyInjection;
using SynthesizerUI.ViewModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetEscapades.Extensions.Logging.RollingFile;
using SynthesizerUI.Extensions;
using SynthesizerUI.Services;
using Microsoft.Extensions.Configuration;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false, true)
#if DEBUG
                .AddJsonFile("appsettings.Development.json", true, true)
#else
                .AddJsonFile("appsettings.Production.json", true, true)
#endif
                .Build();


            // configure logging
            var userDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var logDirectory = Path.Combine(userDirectory, "SynthesizerUI", "Logs");
            var configDirectory = Path.Combine(userDirectory, "SynthesizerUI", "Configuration");

            Directory.CreateDirectory(configDirectory);
            Directory.CreateDirectory(logDirectory);

            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                    .AddSingleton<ISynthesizerService, SynthesizerService>()
                    .AddSingleton<IDialogService, DialogService>()
                    .AddSingleton<IMIDIDeviceService, MIDIDeviceService>()
                    .AddTransient<DialogTemplates.Notification>()
                    .AddViewModels<ViewModelBase>()
                    .AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                        loggingBuilder.AddFile(options =>
                        {
                            options.FileSizeLimit = 1024 * 1024 * 250; // 250MB max
                            options.Periodicity = PeriodicityOptions.Daily;
                            options.RetainedFileCountLimit = null;
                            options.LogDirectory = logDirectory;
                        });
                        loggingBuilder.AddConsole();
                    })
                    .BuildServiceProvider()
            );
        }

    }

}

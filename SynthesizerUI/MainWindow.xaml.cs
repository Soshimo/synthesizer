using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.DependencyInjection;
using SynthesizerUI.Services;
using SynthesizerUI.ViewModel;

namespace SynthesizerUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            var service = Ioc.Default.GetService<ISynthesizerService>();

            if(service != null)
                DataContext = new MainWindowViewModel(service);

            InitializeComponent();

        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void MainWindow_OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }
    }
} 
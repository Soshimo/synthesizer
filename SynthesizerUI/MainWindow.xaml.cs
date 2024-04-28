using System.Windows;
using SynthesizerUI.Controls;
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

        private void PianoKeyboardControl_OnKeyPressed(object? sender, KeyPressedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel?.PressKey(e.Note, e.Octave);
        }

        private void PianoKeyboardControl_OnKeyReleased(object? sender, KeyPressedEventArgs e)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel?.ReleaseKey(e.Note, e.Octave);
        }
    }
} 
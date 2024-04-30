using SynthesizerUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SynthesizerUI.Controls
{
    /// <summary>
    /// Interaction logic for SynthesizerPage.xaml
    /// </summary>
    public partial class SynthesizerPage : Page
    {
        public SynthesizerPage()
        {
            InitializeComponent();
        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MainWindow_OnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void MainWindow_OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
        }

        private void PianoKeyboardControl_OnKeyPressed(object? sender, KeyPressedEventArgs e)
        {
            var viewModel = DataContext as SynthesizerPageViewModel;
            viewModel?.PressKey(e.Note, e.Index);
        }

        private void PianoKeyboardControl_OnKeyReleased(object? sender, KeyPressedEventArgs e)
        {
            var viewModel = DataContext as SynthesizerPageViewModel;
            viewModel?.ReleaseKey(e.Note, e.Index);
        }
    }
}

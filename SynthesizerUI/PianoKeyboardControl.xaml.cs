using SynthesizerUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SynthesizerUI
{
    /// <summary>
    /// Interaction logic for PianoKeyboardControl.xaml
    /// </summary>
    public partial class PianoKeyboardControl : UserControl
    {
        // Dependency Property for Piano Keys
        public static readonly DependencyProperty PianoKeysProperty = DependencyProperty.Register(
            nameof(PianoKeys),
            typeof(ObservableCollection<PianoKeyViewModel>),
            typeof(PianoKeyboardControl),
            new PropertyMetadata(null));

        public ObservableCollection<PianoKeyViewModel> PianoKeys
        {
            get => (ObservableCollection<PianoKeyViewModel>)GetValue(PianoKeysProperty);
            set => SetValue(PianoKeysProperty, value);
        }

        public double TotalKeysWidth => 560;

        public PianoKeyboardControl()
        {
            InitializeComponent();
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button button) return;

            var viewModel = button.DataContext as PianoKeyViewModel;
            viewModel?.KeyPressCommand.Execute(null);
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button button) return;

            var viewModel = button.DataContext as PianoKeyViewModel;
            viewModel?.KeyReleaseCommand.Execute(null);
        }
    }
}

﻿using SynthesizerUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
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


    public partial class PianoKeyboardControl : UserControl
    {
        public event EventHandler<KeyPressedEventArgs>? KeyPressed;
        public event EventHandler<KeyPressedEventArgs>? KeyReleased;

        private readonly List<Key> _assignedKeyList =
        [
            Key.A,
            Key.W,
            Key.S,
            Key.E,
            Key.D,
            Key.F,
            Key.T,
            Key.G,
            Key.Y,
            Key.H,
            Key.U,
            Key.J,
            Key.K,
            Key.I,
            Key.L,
            Key.O,
            Key.OemSemicolon,
            Key.OemQuotes,
            Key.P,
            Key.Enter,
            Key.OemOpenBrackets,
            Key.B,
            Key.N,
            Key.M
        ];

        private readonly List<bool> _isBlackList =
        [
            false, true, false, true, false, false, true, false, true, false, true, false, false, true, false, true,
            false, false, true, false, true, false, true, false
        ];

        private readonly List<string> _noteList =
        [
            "C", "C#", "D", "D#", "E", "F",
            "F#", "G", "G#", "A", "A#", "B",
            "C", "C#", "D", "D#", "E", "F",
            "F#", "G", "G#", "A", "A#", "B"
        ];

        public ObservableCollection<PianoKeyViewModel> PianoKeys { get; } = new ObservableCollection<PianoKeyViewModel>();

        public double TotalKeysWidth => 560;

        public PianoKeyboardControl()
        {
            InitializeComponent();
            InitializeKeyboard();
        }

        private void InitializeKeyboard()
        {
            const int numKeys = 24;

            var octave = 0;
            for (var i = 0; i < numKeys; i++)
            {
                if (_noteList[i] == "A") octave++;

                var key = new PianoKeyViewModel()
                {
                    AssignedKey = _assignedKeyList[i],
                    IsBlack = _isBlackList[i],
                    Note = _noteList[i],
                    Octave = octave,
                    Index = i
                };

                PianoKeys.Add(key);
            }

        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button button) return;
            if (button.DataContext is not PianoKeyViewModel viewModel) return;

            viewModel.IsPressed = true;

            OnKeyPressed(new KeyPressedEventArgs(viewModel.Note, viewModel.Octave));
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is not Button button) return;

            if(button.DataContext is not PianoKeyViewModel viewModel) return;

            viewModel.IsPressed = false;

            OnKeyReleased(new KeyPressedEventArgs(viewModel.Note, viewModel.Octave));
        }

        protected virtual void OnKeyReleased(KeyPressedEventArgs e)
        {
            KeyReleased?.Invoke(this, e);
        }

        protected virtual void OnKeyPressed(KeyPressedEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }
    }
}

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
using SynthesizerUI.DependencyObjects;

namespace SynthesizerUI
{
    /// <summary>
    /// Interaction logic for KnobControl.xaml
    /// </summary>
    public partial class KnobControl : UserControl
    {
        private bool _isDragging;
        private Point _lastMousePosition;

        public event EventHandler<KnobChangedEventArgs> KnobChanged;

        public KnobControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty LogarithmicProperty =
            DependencyProperty.Register("Logarithmic", typeof(bool), typeof(KnobControl), new PropertyMetadata(false));

        public bool Logarithmic
        {
            get => (bool)GetValue(LogarithmicProperty);
            set => SetValue(LogarithmicProperty, value);
        }

        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register("Minimum", typeof(double), typeof(KnobControl),
                new PropertyMetadata(0.0, OnMinimumOrMaximumChanged));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register("Maximum", typeof(double), typeof(KnobControl),
                new PropertyMetadata(100.0, OnMinimumOrMaximumChanged));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        private static void OnMinimumOrMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (KnobControl)d;
            control.UpdateImage();
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(double), typeof(KnobControl), new PropertyMetadata(0.0, OnValueChanged));

        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (KnobControl)d;
            control.UpdateImage();

            control.OnKnobChanged(new KnobChangedEventArgs() { OldValue = (double)e.OldValue, NewValue = (double)e.NewValue});
        }


        public static readonly DependencyProperty SpriteSheetProperty = DependencyProperty.Register(
            "SpriteSheet", typeof(SpriteSheet), typeof(KnobControl), new PropertyMetadata(null, OnSpriteSheetChanged));

        public SpriteSheet? SpriteSheet
        {
            get => (SpriteSheet)GetValue(SpriteSheetProperty);
            set => SetValue(SpriteSheetProperty, value);
        }

        private static void OnSpriteSheetChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (KnobControl)d;
            control.UpdateImage();
        }

        public static readonly DependencyProperty WrapRotationProperty =
            DependencyProperty.Register("WrapRotation", typeof(bool), typeof(KnobControl), new PropertyMetadata(false));

        public bool WrapRotation
        {
            get => (bool)GetValue(WrapRotationProperty);
            set => SetValue(WrapRotationProperty, value);
        }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step",
            typeof(float),
            typeof(KnobControl),
            new FrameworkPropertyMetadata(1.0f, FrameworkPropertyMetadataOptions.AffectsRender),
            ValidateStepValue);

        private static bool ValidateStepValue(object value)
        {
            return (float)value > 0;
        }

        public float Step
        {
            get => (float)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }


        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateImage();
        }


        private void UpdateImage()
        {
            if (SpriteSheet?.CroppedImages.Count == 0) return;

            var maximum = Logarithmic ? Math.Log2(Maximum) : Maximum;
            var minimum = Logarithmic ? Math.Log2(Minimum) : Minimum;
            var value = Logarithmic ? Math.Log2(Value) : Value;

            var mappedValue = (value - minimum) / (maximum - minimum);
            var index = (int)Math.Floor(mappedValue * SpriteSheet?.CroppedImages.Count ?? 0);
            index = Math.Max(0, Math.Min((SpriteSheet?.CroppedImages.Count ?? 0) - 1, index));

            var imageNullable = Template.FindName("PART_Image", this);

            var image = (Image?)Template.FindName("PART_Image", this);
            if (image != null)
            {
                image.Source = SpriteSheet?.CroppedImages[index];
            }
        }

        private void KnobControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            ReleaseMouseCapture(); // Release the mouse capture when dragging stops
            e.Handled = true;
        }

        private void KnobControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            _lastMousePosition = e.GetPosition(this);
            CaptureMouse(); // Capture the mouse to ensure the control receives all mouse events during dragging
            e.Handled = true;
        }

        private bool? _lastShift;
        private Point _lastPosition;
        private double _startPosX;
        private double _startPosY;
        private double _startVal;

        private void KnobControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging) return;

            var currentPosition = e.GetPosition(this);

            var currentPageX = currentPosition.X;
            var currentPageY = currentPosition.Y;

            var currentShiftKey = (Keyboard.Modifiers == ModifierKeys.Shift);

            if (_lastShift != currentShiftKey)
            {
                _lastShift = currentShiftKey;
                _startPosX = currentPageX;
                _startPosY = currentPageY;
                _startVal = Value;
            }

            var offsetX = currentPageX - _lastPosition.X;
            var offsetY = currentPageY - _lastPosition.Y;
            var offset = (_startPosY - currentPageY - _startPosX + currentPageX) * 2;

            var min = Logarithmic ? Math.Log2(Minimum) : Minimum;
            var max = Logarithmic ? Math.Log2(Maximum) : Maximum;
            var ctlStep = Logarithmic ? .001 : 0.5;
            var startVal = Logarithmic ? Math.Log2(_startVal) : _startVal;

            var newValue = min + (((startVal + (max - min) * offset / ((currentShiftKey ? 4 : 1) * 128)) - min) / ctlStep) * ctlStep;
            newValue = Math.Round(newValue / ctlStep) * ctlStep;

            newValue = Math.Min(Math.Max(min, newValue), max);
            Value = Logarithmic ? Math.Pow(2, newValue) : newValue;

        }


        private void KnobControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var minimum = Logarithmic ? Math.Log2(Minimum) : Minimum;
            var maximum = Logarithmic ? Math.Log2(Maximum) : Maximum;
            var value = Logarithmic ? Math.Log2(Value) : Value;
            var range = (maximum - minimum);
            var delta = 1.0d;

            if ((Keyboard.Modifiers == ModifierKeys.Shift))
            {
                delta *= 0.2;
            }

            delta *= (range * 0.1);

            var step = Logarithmic ? 1 : (maximum - minimum) / 100;
            if (Math.Abs(delta) < step) delta = step;
            delta *= Math.Sign(e.Delta);

            value += delta;

            //value = value > maximum ? maximum : value < minimum ? minimum : value;
            value = Math.Min(Math.Max(minimum, value), maximum);
            Value = Logarithmic ? Math.Pow(2, value) : value;
        }

        protected virtual void OnKnobChanged(KnobChangedEventArgs e)
        {
            KnobChanged?.Invoke(this, e);
        }
    }

    public class KnobChangedEventArgs : EventArgs
    {
        public double OldValue { get; set; }
        public double NewValue { get; set; }
    }
}

using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core;


public sealed class Automation
{
    private readonly IChannel? _inputChannel;
    private double _value;

    public Automation(IAudioNode node, int? inputIndex = null, double value = 0)
    {
        _inputChannel = inputIndex.HasValue ? node.Inputs[inputIndex.Value] : null;
        _value = value;
    }

    private bool IsDynamic()
    {
        return (_inputChannel?.Samples.Count > 0);
    }

    public void SetValue(double value)
    {
        _value = value;
    }

    public double GetValue()
    {
        return IsDynamic() ? _inputChannel?.Samples[0] ?? 0 : _value;
    }

    public static implicit operator double(Automation automation)
    {
        return automation.GetValue();
    }

}

//public class ValueChangedEventArgs : EventArgs
//{
//    public double OldValue;
//    public double NewValue;

//    public ValueChangedEventArgs(double oldValue, double newValue)
//    {
//        OldValue = oldValue;
//        NewValue = newValue;
//    }
//}
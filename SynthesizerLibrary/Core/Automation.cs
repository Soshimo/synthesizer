using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core;


public class Automation
{
    private readonly IChannel? _inputChannel;
    private double _value;

    public event EventHandler<ValueChangedEventArgs> ValueChanged;
    public Automation(IAudioNode node, int? inputIndex = null, double value = 0)
    {
        _inputChannel = inputIndex.HasValue ? node.Inputs[inputIndex.Value] : null;
        _value = value;
    }

    public bool IsStatic()
    {
        return (_inputChannel?.Samples.Count == 0);
    }

    public bool IsDynamic()
    {
        return (_inputChannel?.Samples.Count > 0);
    }

    public void SetValue(double value)
    {
        OnValueChanged(new ValueChangedEventArgs(_value, value));
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

    protected virtual void OnValueChanged(ValueChangedEventArgs e)
    {
        ValueChanged?.Invoke(this, e);
    }
}

public class ValueChangedEventArgs : EventArgs
{
    public double OldValue;
    public double NewValue;

    public ValueChangedEventArgs(double oldValue, double newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
}
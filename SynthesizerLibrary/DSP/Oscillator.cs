using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;

namespace SynthesizerLibrary.DSP;

public class Oscillator : AudioNode
{
    private readonly Automation _frequency;
    private readonly Automation _pulseWidth;

    private double _phase;
    private double _p;

    private WaveShape _waveShape;

    public Oscillator(IAudioProvider provider, float frequency = 440f, WaveShape waveShape = WaveShape.Sine, float pulseWidth = 0.5f) : base(provider, 2, 1, "Oscillator")
    {
        _frequency = new Automation(this, 0, frequency);
        _pulseWidth = new Automation(this, 1, pulseWidth);

        _waveShape = waveShape;

        _phase = 0.0;
        _p = 0.0;

    }

    public void SetFrequency(double frequency)
    {
        _frequency.SetValue(frequency);
    }

    public void SetWaveShape(WaveShape waveShape)
    {
        _waveShape = waveShape;
    }

    protected override void GenerateMix()
    {
        var output = Outputs[0];

        double pulseWidth = _pulseWidth;
        double frequency = _frequency;

        output.Samples[0] = GetMix();

        _phase = (_phase + frequency / AudioProvider.SampleRate / 2) % 1;

        var p = (_phase) % 1;
        _p = p < pulseWidth ? p / pulseWidth : (p - pulseWidth) / (1 - pulseWidth);
    }

    private double GetMix()
    {
        return _waveShape switch
        {
            WaveShape.Sine => Sine(),
            WaveShape.Triangle => Triangle(),
            WaveShape.Square => Square(),
            WaveShape.Sawtooth => Sawtooth(),
            WaveShape.InvSawtooth => InvSawtooth(),
            WaveShape.Pulse => Pulse(),
            _ => throw new ArgumentOutOfRangeException(nameof(_waveShape))
        };
    }


    private double Sine()
    {
        return Math.Sin(_p * Math.PI * 2);
    }

    double Triangle()
    {
        return _p < 0.5 ? 4 * _p - 1 : 3 - 4 * _p;
    }

    double Square()
    {
        return _p < 0.5 ? -1 : 1;
    }

    double Sawtooth()
    {
        return 1 - _p * 2;
    }

    double InvSawtooth()
    {
        return _p * 2 - 1;
    }

    double Pulse()
    {
        return _p < 0.5 ? (_p < 0.25 ? _p * 8 - 1 : 1 - (_p - 0.25) * 8) : -1;
    }


}
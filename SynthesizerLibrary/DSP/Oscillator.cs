using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class Oscillator : AudioNode
{
    private readonly Automation _frequency;
    private readonly Automation _pulseWidth;
    private readonly Automation _detune;

    private double _phase;
    private double _p;

    private Waveform _waveShape;

    public Oscillator(IAudioProvider provider, float frequency = 440f, Waveform waveShape = Waveform.Sine, float detune = 0.0f, float pulseWidth = 0.5f) : base(provider, 3, 1)
    {
        _frequency = new Automation(this, 0, frequency);
        _detune = new Automation(this, 1, detune);
        _pulseWidth = new Automation(this, 2, pulseWidth);

        _waveShape = waveShape;

        _phase = 0.0;
        _p = 0.0;

    }

    public void SetFrequency(double frequency)
    {
        _frequency.SetValue(frequency);
    }

    public void SetDetune(double detune)
    {
        _detune.SetValue(detune);
    }

    public void SetWaveShape(Waveform waveShape)
    {
        _waveShape = waveShape;
    }

    protected override void GenerateMix()
    {
        var output = Outputs[0];

        double pulseWidth = _pulseWidth;
        double detuneFactor = Math.Pow(2, _detune / 1200);
        double frequency = _frequency * detuneFactor;


        output.Samples[0] = GetMix();

        _phase = (_phase + frequency / AudioProvider.SampleRate / 2) % 1;

        var p = (_phase) % 1;
        _p = p < pulseWidth ? p / pulseWidth : (p - pulseWidth) / (1 - pulseWidth);
    }

    private double GetMix()
    {
        return _waveShape switch
        {
            Waveform.Sine => Sine(),
            Waveform.Triangle => Triangle(),
            Waveform.Square => Square(),
            Waveform.Sawtooth => Sawtooth(),
            Waveform.InvSawtooth => InvSawtooth(),
            Waveform.Pulse => Pulse(),
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
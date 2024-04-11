using System.Drawing;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public sealed class Noise : AudioNode
{
    private readonly Random _random;
    
    private NoiseColor _color;


    private double _b0, _b1, _b2, _b3, _b4, _b5;
    private double _c1, _c2, _c3, _c4;
    private int _q;
    private double _q0, _q1;
    private double _brownQ;

    public NoiseColor Color
    {
        get => _color;
        set
        {
            _color = value;
            Reset();
        }
    }

    public Noise(IAudioProvider provider, NoiseColor color = NoiseColor.White) : base(provider, 0, 1)
    {
        _random = new Random();
        Color = color;
    }

    protected override void GenerateMix()
    {
        Outputs[0].Samples[0] = (float)GetValue();
    }

    private void Reset()
    {
        _q = 15;
        _c1 = (1 << _q) - 1;
        _c2 = (int)(_c1 / 3) + 1;
        _c3 = 1 / _c1;
        _c1 = _c2 * 6;
        _c4 = 3 * (_c2 - 1);
        _q0 = Math.Exp(-200 * Math.PI / AudioProvider.SampleRate);
        _q1 = 1 - _q0;
    }

    private double GetValue()
    {
        return _color switch
        {
            NoiseColor.Brown => Brown(),
            NoiseColor.Pink => Pink(),
            NoiseColor.White => White(),
            _ => 0
        };
    }

    private double White()
    {
        var r = _random.NextDouble();
        return (r * _c1 - _c4) * _c3;
    }

    private double Pink()
    {
        var w = White();
        _b0 = 0.997 * _b0 + 0.029591 * w;
        _b1 = 0.985 * _b1 + 0.032534 * w;
        _b2 = 0.950 * _b2 + 0.048056 * w;
        _b3 = 0.850 * _b3 + 0.090579 * w;
        _b4 = 0.620 * _b4 + 0.108990 * w;
        _b5 = 0.250 * _b5 + 0.255784 * w;
        return 0.55 * (_b0 + _b1 + _b2 + _b3 + _b4 + _b5);
    }

    private double Brown()
    {
        var w = White();
        _brownQ = _q1 * w + _q0 * _brownQ;
        return 6.2 * _brownQ;
    }
}
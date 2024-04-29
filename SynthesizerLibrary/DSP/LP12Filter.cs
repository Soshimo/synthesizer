using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class LP12Filter : AudioNode
{
    private readonly Automation _cutoff;
    private readonly Automation _resonance;

    private double _previousCutoff;
    private double _previousResonance;
    // coefficients
    private double _w, _q, _r, _c;

    private double _vibraSpeed;
    private double _vibraPos;

    public LP12Filter(IAudioProvider provider, double cutoff = 20000, double resonance = 1) : base(provider, 3, 1)
    {

        _cutoff = new Automation(this, 1, cutoff);
        _resonance = new Automation(this, 2, resonance); ;

        CalcCoefficients();

        _previousCutoff = cutoff;
        _previousResonance = resonance;
    }

    protected override void GenerateMix()
    {
        var input = Inputs[0];
        var output = Outputs[0];

        if (_previousCutoff.CompareTo(_cutoff.GetValue()) != 0 || _previousResonance.CompareTo(_resonance.GetValue()) != 0)
        {
            CalcCoefficients();
            _previousCutoff = _cutoff.GetValue();
            _previousResonance = _resonance.GetValue();
        }

        var numberOfChannels = input.Samples.Count;
        for (var i = 0; i < numberOfChannels; i++)
        {

            var sample = input.Samples[i];
            _vibraSpeed += (sample - _vibraPos) * _c;
            _vibraPos += _vibraSpeed;
            _vibraSpeed *= _r;

            output.Samples[i] = _vibraPos;
        }
    }


    private void CalcCoefficients()
    {
        _w = Math.PI * 2 * _cutoff.GetValue() / AudioProvider.SampleRate;
        _q = 1.0 - _w / (2 * (_resonance.GetValue() + 0.5 / (1.0 + _w)) + _w - 2);
        _r = _q * _q;
        _c = _r + 1 - 2 * Math.Cos(_w) * _q;
    }

}
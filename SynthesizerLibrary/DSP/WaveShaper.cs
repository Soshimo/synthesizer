using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class WaveShaper : AudioNode
{
    private double[] _curve;
    public WaveShaper(IAudioProvider provider, double[]? curve = null) : base(provider, 1, 1)
    {
        _curve = curve ?? new double[4096];
    }

    public double[] Curve
    {
        get => _curve;
        set => _curve = value ?? throw new ArgumentNullException(nameof(value));
    }

    protected override void GenerateMix()
    {
        var inputSamples = Inputs[0].Samples;
        var outputSamples = Outputs[0].Samples;

        var curveLength = _curve.Length;

        for (var i = 0; i < inputSamples.Count; i++)
        {
            var sample = inputSamples[i];
            var index = (int)((sample + 1) * 0.5 * (curveLength - 1));  // Map the sample to a curve index
            index = Math.Clamp(index, 0, curveLength - 1);
            outputSamples[i] = _curve[index];
        }
    }
}
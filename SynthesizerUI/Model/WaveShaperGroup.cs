using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;
using SynthesizerLibrary.DSP;

namespace SynthesizerUI.Model;

public class WaveShaperGroup : GroupNode
{
    private readonly Gain _preGain;
    private readonly Gain _postGain;
    private readonly WaveShaper _waveShaper;

    public WaveShaperGroup(IAudioProvider provider) : base(provider, 1, 1)
    {
        _preGain = new Gain(provider);
        _postGain = new Gain(provider);
        _waveShaper = new WaveShaper(provider);

        _preGain.Connect(_waveShaper);
        _waveShaper.Connect(_postGain);

        InputPassThroughNodes[0].Connect(_preGain);
        _postGain.Connect(OutputPassThroughNodes[0]);

        var curve = new double[65536]; // FIXME: share across instances
        GenerateColortouchCurve(curve);
        _waveShaper.Curve = curve;
    }

    public void SetDrive(double value)
    {
        _preGain.Multiplier.SetValue(value);

        var postDrive = Math.Pow(1 / value, 0.6);
        _postGain.Multiplier.SetValue(postDrive);

    }

    static double[] GenerateColortouchCurve(double[] curve)
    {
        var n = 65536;
        var n2 = n / 2;

        for (var i = 0; i < n2; ++i)
        {
            var x = i / (double)n2;
            x = GenerateShape(x);  // Shape is a placeholder for your shaping function

            curve[n2 + i] = x;
            curve[n2 - i - 1] = -x;
        }

        return curve;
    }


    static double[] GenerateMirrorCurve(double[] curve)
    {
        var n = 65536;
        var n2 = n / 2;

        for (var i = 0; i < n2; ++i)
        {
            var x = i / (double)n2;
            x = GenerateShape(x);  // Shape is a placeholder for your shaping function

            curve[n2 + i] = x;
            curve[n2 - i - 1] = x;
        }

        return curve;
    }

    static double DecibelToLinear(double db)
    {
        return Math.Pow(10.0, 0.05 * db);
    }


    static double E4(double x, double k)
    {
        return 1.0 - Math.Exp(-k * x);
    }

    static double GenerateShape(double x)
    {
        var threshold = -27D; // dB
        var headroom = 21D; // dB

        var linearThreshold = DecibelToLinear(threshold);
        var linearHeadroom = DecibelToLinear(headroom);

        var maximum = 1.05 * linearHeadroom * linearThreshold;
        var kk = maximum - linearThreshold;

        double sign = x < 0 ? -1 : 1;
        var absx = Math.Abs(x);

        var shapedInput = absx < linearThreshold ? absx : linearThreshold + kk * E4(absx - linearThreshold, 1.0 / kk);
        shapedInput *= sign;

        return shapedInput;
    }

}
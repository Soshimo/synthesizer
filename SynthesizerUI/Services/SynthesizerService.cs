using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SynthesizerUI.Services.Interface;

namespace SynthesizerUI.Services;

public class SynthesizerService : ISynthesizerService
{
    private readonly WasapiAudioProvider _audioProvider = new();
    private readonly Gain _masterGain;
    //private readonly WaveShaper _waveShaper;
    private readonly WaveShaperGroup _waveShaperGroup;

    private readonly Voice?[] _voices = new Voice?[256];
        
    public SynthesizerService()
    {
        _masterGain = new Gain(_audioProvider);

        //var curve = Enumerable.Range(0, 4096).Select(i => (double)i / 4096 * 2 - 1).ToArray();
        //_waveShaper = new WaveShaper(_audioProvider, curve);

        //_waveShaper.Connect(_masterGain);

        _waveShaperGroup = new WaveShaperGroup(_audioProvider);

        // let's go ahead and set the drive to 50
        _waveShaperGroup.SetDrive(50);

        _waveShaperGroup.Connect(_masterGain);

        _audioProvider.ConnectToOutput(_masterGain);
        _audioProvider.Play();
    }

    public void SetDrive(double value)
    {
        _waveShaperGroup.SetDrive(0.01 + (value * value / 500.0));
    }
    private double[] CreateShaperCurve()
    {
        var driveShaper = new double[4096];

        // Fill the "bottom" half of the response curve (linear)
        for (var i = 0; i < 1024; i++)
        {
            driveShaper[2048 + i] = driveShaper[2047 - i] = (double)i / 2048;
        }

        // Fill the "top" half of the response curve (logarithmic)
        for (var i = 0; i < 1024; i++)
        {
            driveShaper[3072 + i] = driveShaper[1023 - i] = Math.Sqrt((i + 1024) / 1024.0) / 2;
        }

        return driveShaper;
    }
    public void NoteOn(int noteIndex, VoiceData voiceData)
    {
        if (_voices[noteIndex] != null) return;

        var voiceObject = new Voice(_audioProvider, voiceData);

        _voices[noteIndex] = voiceObject;
        voiceObject.Connect(_waveShaperGroup);

        voiceObject.NoteOn();
    }

    public void NoteOff(int noteIndex)
    {
        var voiceObject = _voices[noteIndex];
        if(voiceObject == null ) return;

        voiceObject.NoteOff();
        voiceObject.VoiceComplete += (sender, args) =>
        {
            _voices[noteIndex]?.Disconnect(_waveShaperGroup);
            _voices[noteIndex] = null;
        };
    }
}
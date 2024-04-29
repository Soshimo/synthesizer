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
    private readonly WaveShaper _waveShaper;

    //private readonly ObjectPool<Voice, VoiceFactory> _voiceObjectPool;
    //private readonly Dictionary<string, VoiceWrapper> _voiceDictionary = new() { };

    private Voice?[] _voices = new Voice?[256];
    private VoiceWrapper?[] _wrappers = new VoiceWrapper?[256];
        
    public SynthesizerService()
    {
        var factory = new VoiceFactory(_audioProvider, new VoiceData());
        //_voiceObjectPool = new ObjectPool<Voice, VoiceFactory>(factory, 16);

        _masterGain = new Gain(_audioProvider);

        var curve = Enumerable.Range(0, 4096).Select(i => (double)i / 4096 * 2 - 1).ToArray();
        _waveShaper = new WaveShaper(_audioProvider, CreateShaperCurve());

        _waveShaper.Connect(_masterGain);
        _audioProvider.ConnectToOutput(_masterGain);
        _audioProvider.Play();
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
        var wrapper = new VoiceWrapper(_audioProvider, voiceObject);

        _voices[noteIndex] = voiceObject;
        _wrappers[noteIndex] = wrapper;

        //wrapper.Oscillator1Frequency.Value.SetValue(voiceData.Oscillator1Frequency);
        //wrapper.Oscillator2Frequency.Value.SetValue(voiceData.Oscillator2Frequency);

        voiceObject.InitializeVoice(voiceData);
        voiceObject.SetVolumeEnvelopeAttack(voiceData.Attack);
        voiceObject.SetVolumeEnvelopeDecay(voiceData.Decay);
        voiceObject.SetVolumeEnvelopeRelease(voiceData.Release);
        voiceObject.SetVolumeEnvelopeSustain(voiceData.Sustain);
        voiceObject.Connect(_waveShaper);

        voiceObject.NoteOn();
    }

    public void NoteOff(int noteIndex)
    {
        var voiceObject = _voices[noteIndex];
        if(voiceObject == null ) return;

        voiceObject.NoteOff();
        voiceObject.VoiceComplete += (sender, args) =>
        {
            _voices[noteIndex]?.Disconnect(_waveShaper);
            _wrappers[noteIndex]?.Dispose();

            _voices[noteIndex] = null;
            _wrappers[noteIndex] = null;
        };
    }
}
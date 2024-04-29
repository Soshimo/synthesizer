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

    private readonly ObjectPool<Voice, VoiceFactory> _voiceObjectPool;
    private readonly Dictionary<string, VoiceWrapper> _voiceDictionary = new() { };

    private const int BaseOctave = 2;

    public SynthesizerService()
    {
        var factory = new VoiceFactory(_audioProvider, new VoiceData(0, .1f, .1f, .7f, .1f));
        _voiceObjectPool = new ObjectPool<Voice, VoiceFactory>(factory, 16);

        _masterGain = new Gain(_audioProvider);

        var curve = Enumerable.Range(0, 256).Select(i => (double)i / 255 * 2 - 1).ToArray();
        _waveShaper = new WaveShaper(_audioProvider, CreateShaperCurve());

        _waveShaper.Connect(_masterGain);
        _audioProvider.ConnectToOutput(_masterGain);
        _audioProvider.Play();
    }

    private double[] CreateShaperCurve()
    {
        double[] driveShaper = new double[4096];

        // Fill the "bottom" half of the response curve (linear)
        for (int i = 0; i < 1024; i++)
        {
            driveShaper[2048 + i] = driveShaper[2047 - i] = (double)i / 2048;
        }

        // Fill the "top" half of the response curve (logarithmic)
        for (int i = 0; i < 1024; i++)
        {
            driveShaper[3072 + i] = driveShaper[1023 - i] = Math.Sqrt((i + 1024) / 1024.0) / 2;
        }

        return driveShaper;
    }
    public void NoteOn(string key, VoiceData voiceData)
    {
        if (_voiceDictionary.ContainsKey(key)) return;
            
        var voiceObject = _voiceObjectPool.GetObject();
        if (voiceObject == null) return;

        var wrapper = new VoiceWrapper(_audioProvider, voiceObject);
        _voiceDictionary[key] = wrapper;

        wrapper.FrequencyAutomationNode.Value.SetValue(voiceData.Frequency);
        wrapper.Voice.SetAttack(voiceData.Attack);

        //voiceObject.Connect(_masterGain);
        voiceObject.Connect(_waveShaper);
        voiceObject.NoteOn();
    }

    public void NoteOff(string noteKey)
    {
        if (!_voiceDictionary.Remove(noteKey, out var value)) return;

        var voiceObject = value.Voice;

        voiceObject.NoteOff();
        voiceObject.VoiceComplete += (sender, args) =>
        {
            value.Dispose();
            //voiceObject.Disconnect(_masterGain);
            voiceObject.Disconnect(_waveShaper);
            _voiceObjectPool.ReturnObject(voiceObject);
        };
    }
}
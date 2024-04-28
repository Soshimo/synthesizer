using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SynthesizerUI.Model;
using SythesizerLibrary.Core.Audio;

namespace SynthesizerUI.Services;

public class SynthesizerService : ISynthesizerService
{
    private readonly WasapiAudioProvider _audioProvider = new();
    private readonly Gain _masterGain;

    private readonly ObjectPool<Voice, VoiceFactory> _voiceObjectPool;
    private readonly Dictionary<string, VoiceWrapper> _voiceDictionary = new() { };

    private const int BaseOctave = 2;

    public SynthesizerService()
    {
        var factory = new VoiceFactory(_audioProvider, new VoiceData(0, .1f, .1f, .7f, .1f));
        _voiceObjectPool = new ObjectPool<Voice, VoiceFactory>(factory, 16);

        _masterGain = new Gain(_audioProvider);

        _audioProvider.ConnectToOutput(_masterGain);
        _audioProvider.Play();
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
        
        voiceObject.Connect(_masterGain);
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
            voiceObject.Disconnect(_masterGain);
            _voiceObjectPool.ReturnObject(voiceObject);
        };
    }
}
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

    private readonly Dictionary<string, VoiceWrapper> _voiceDictionary = [];

    private const int BaseOctave = 2;

    public SynthesizerService()
    {
        var factory = new VoiceFactory(_audioProvider, new VoiceData(0, .7f, 1.0f, .5f, 1.5f));
        _voiceObjectPool = new ObjectPool<Voice, VoiceFactory>(factory, 10);

        _masterGain = new Gain(_audioProvider);

        _audioProvider.ConnectToOutput(_masterGain);
        _audioProvider.Play();
    }

    public void NoteOn(string note, int octave)
    {
        var noteString = $"{note}{octave + BaseOctave}"; // i.e. C0, or C1
        if (_voiceDictionary.ContainsKey(noteString)) return;

        var voiceObject = _voiceObjectPool.GetObject();
        if(voiceObject == null) return;

        var (index, actualOctave) = NoteHelper.ParseNoteString(noteString);
        var frequency = NoteHelper.NoteToFrequency(index, actualOctave);

        var wrapper = new VoiceWrapper(_audioProvider, voiceObject);
        _voiceDictionary[noteString] = wrapper;

        wrapper.FrequencyAutomationNode.Value.SetValue(frequency);
        
        voiceObject.Connect(_masterGain);
        voiceObject.NoteOn();
    }

    public void NoteOff(string note, int octave)
    {
        var noteString = $"{note}{octave + BaseOctave}"; // i.e. C0, or C1
        if (!_voiceDictionary.ContainsKey(noteString)) return;

        var voiceObjectWrapper = _voiceDictionary[noteString];
        _voiceDictionary.Remove(noteString);

        var voiceObject = voiceObjectWrapper.Voice;

        voiceObject.NoteOff();
        voiceObject.VoiceComplete += (sender, args) =>
        {
            voiceObjectWrapper.Dispose();
            //voiceObjectWrapper = null;

            voiceObject.Disconnect(_masterGain);
            _voiceObjectPool.ReturnObject(voiceObject);
        };
    }
}
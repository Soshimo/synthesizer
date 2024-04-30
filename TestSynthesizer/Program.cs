// See https://aka.ms/new-console-template for more information

using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;
using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Operators;
using SynthesizerLibrary.Scale;
using SynthesizerLibrary.Tuning;
using SynthesizerLibrary.Util;


var audioProvider = new WasapiAudioProvider();

var minorKeyOfBFlat = new MinorScale("Bb", new WesternTuning());  // 2^(n/12)

var foo = (float)minorKeyOfBFlat.GetFrequency(0, 3);
var bar = (float)minorKeyOfBFlat.GetFrequency(4, 3);

//var osc1 = new Oscillator(audioProvider, foo, WaveShape.Sawtooth);
//var oscGain = new Gain(audioProvider, 1);
//osc1.Connect(oscGain);

////var osc2 = new Oscillator(audioProvider, bar, WaveShape.Triangle);
////var osc2Gain = new Gain(audioProvider, .5);
////osc2.Connect(osc2Gain);

////var noise = new Noise(audioProvider, NoiseColor.Pink);
////var noiseGain = new Gain(audioProvider, .1);
////noise.Connect(noiseGain);

////var mixer = new Mixer(audioProvider, 3);

////oscGain.Connect(mixer, 0);
////osc2Gain.Connect(mixer, 1);
////noiseGain.Connect(mixer, 2);

//var env = new ADSREnvelope(audioProvider, 0, .7, 1, .75, 1.5);

////osc1.Connect(env);

////   /\___
////  /     \
//// /      \

////env.Connect(oscGain, 1);

//var osc2 = new Oscillator(audioProvider, 440f);
//var osc3 = new Oscillator(audioProvider, 4f);

//osc3.Connect(osc2, 1);

//var lfo = new Oscillator(audioProvider, 2f);
//var mulAdd = new MulAdd(audioProvider, 100, foo);
//lfo.Connect(mulAdd);


//var mulAdd2 = new MulAdd(audioProvider, 100, bar);
//lfo.Connect(mulAdd2);

var oscGain = new Gain(audioProvider);
var voice = new SynthVoice(audioProvider, new NoteData(foo, .7f, 1f, .5f, 1.5f));
var lfo = new Oscillator(audioProvider, 2f);
var mulAdd = new MulAdd(audioProvider, 100, foo);

lfo.Connect(mulAdd);
mulAdd.Connect(voice);

//var frequencyAutomationNode = new AutomationNode(audioProvider, foo);
//frequencyAutomationNode.Connect(voice);

voice.Connect(oscGain);
audioProvider.ConnectToOutput(oscGain);
audioProvider.Play();

//voice.NoteOn();
//Thread.Sleep(350);
//voice.NoteOff();

var exit = false;
while (!exit)
{
    if (Console.KeyAvailable)
    {
        var key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.Escape:
                exit = true;
                break;

            case ConsoleKey.A:
            {
                if (!voice.IsComplete) break;
                var (index, octave) = NoteHelper.ParseNoteString("A4");
                var frequency = NoteHelper.NoteToFrequency(index, octave);
                mulAdd.Add.SetValue(frequency);

                //frequencyAutomationNode.Value.SetValue(NoteHelper.NoteToFrequency(index, octave));
                voice.NoteOn();
                Thread.Sleep(250);
                voice.NoteOff();
            } 
                break;

            case ConsoleKey.B:
            {
                if (!voice.IsComplete) break;

                var (index, octave) = NoteHelper.ParseNoteString("D#4");

                var frequency = NoteHelper.NoteToFrequency(index, octave);
                mulAdd.Add.SetValue(frequency);

                //frequencyAutomationNode.Value.SetValue(NoteHelper.NoteToFrequency(index, octave));
                voice.NoteOn();
                Thread.Sleep(250);
                voice.NoteOff();
            }
                break;
        }
    }
}



record NoteData(float Frequency, float Attack, float Decay, float Sustain, float Release);

class SynthVoice : GroupNode
{
    private readonly ADSREnvelope _envelope;

    public bool IsComplete { get; private set; }
    public SynthVoice(IAudioProvider provider, NoteData noteData) : base(provider, 5, 5)
    {
        IsComplete = true;

        var osc1 = new Oscillator(provider, noteData.Frequency, Waveform.Sine);

        // TODO: get values from voice constructor
        _envelope = new ADSREnvelope(provider, 0, noteData.Attack, noteData.Decay, noteData.Sustain, noteData.Release);

        _envelope.Complete += (sender, args) =>
        {
            IsComplete = true;
        };

        var osc1Gain = new Gain(provider, 0.5);
        var masterGain = new Gain(provider);

        InputPassThroughNodes[0].Connect(osc1);

        osc1.Connect(osc1Gain);

        _envelope.Connect(osc1Gain, 1);

        osc1Gain.Connect(masterGain);

        masterGain.Connect(OutputPassThroughNodes[0]);
    }

    public void NoteOn()
    {
        if (!IsComplete) return;
            
        IsComplete = false;
        _envelope.Gate.SetValue(1);
    }

    public void NoteOff()
    {
        _envelope.Gate.SetValue(0);
    }

}


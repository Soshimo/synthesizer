// See https://aka.ms/new-console-template for more information

using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SythesizerLibrary.Core;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;
using SythesizerLibrary.Operators;
using SythesizerLibrary.Scale;
using SythesizerLibrary.Tuning;
using static System.Runtime.InteropServices.JavaScript.JSType;


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
var voice = new SynthVoice(audioProvider, foo, bar);
//var lfo = new Oscillator(audioProvider, 2f);
//var mulAdd = new MulAdd(audioProvider, 100, foo);

//lfo.Connect(mulAdd);
//mulAdd.Connect(voice);

var frequencyAutomationNode = new AutomationNode(audioProvider, foo);
frequencyAutomationNode.Connect(voice);

voice.Connect(oscGain);
audioProvider.ConnectToOutput(oscGain);
audioProvider.Play();

voice.NoteOn();
Thread.Sleep(250);
voice.NoteOff();

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
                frequencyAutomationNode.Value.SetValue(440.0);
                voice.NoteOn();
                Thread.Sleep(250);
                voice.NoteOff();
                break;

            case ConsoleKey.B:
                var (index, octave) = NoteHelper.ParseNoteString("Bb4");
                frequencyAutomationNode.Value.SetValue(NoteHelper.NoteToFrequency(index, octave, 27.5));
                voice.NoteOn();
                Thread.Sleep(250);
                voice.NoteOff();
                break;
        }
    }
}


class SynthVoice : GroupNode
{
    private ADSREnvelope _envelope;

    public SynthVoice(IAudioProvider provider, float frequency1, float frequency2) : base(provider, 5, 1, "SynthVoice")
    {
        var osc1 = new Oscillator(provider, frequency1, WaveShape.Sine);
        //var osc2 = new Oscillator(provider, frequency2, WaveShape.Sawtooth);

        var mixer = new Mixer(provider, 2);

        //osc1.Connect(mixer);
        //osc2.Connect(mixer, 1);

        // TODO: get values from voice constructor
        _envelope = new ADSREnvelope(provider, 0, .7, 1, .75, 1.5);

        var osc1Gain = new Gain(provider, 0.5);
        var osc2Gain = new Gain(provider, 0.5);
        var masterGain = new Gain(provider);

        InputPassThroughNodes[0].Connect(osc1);
        //InputPassThroughNodes[1].Connect(osc2);

        osc1.Connect(osc1Gain);
        //osc2.Connect(osc2Gain);

        _envelope.Connect(osc1Gain, 1);
        _envelope.Connect(osc2Gain, 1);

        osc1Gain.Connect(mixer, 0);
        osc2Gain.Connect(mixer, 1);

        mixer.Connect(masterGain);

        //var lfo = new Oscillator(provider, 2f);

        //var mulAdd = new MulAdd(provider, 100, frequency1);
        //lfo.Connect(mulAdd);

        //var mulAdd2 = new MulAdd(provider, 100, frequency2);
        //lfo.Connect(mulAdd2);

        //mulAdd.Connect(osc1);
        //mulAdd2.Connect(osc2);

        // input drives => automationpassthorugh => automation
        // automationpassthrough will have automation in constructor
        // automationpassthrough will set the automation value in the getmix


        masterGain.Connect(OutputPassThroughNodes[0]);
    }

    public void NoteOn()
    {
        _envelope.Gate.SetValue(1);
    }

    public void NoteOff()
    {
        _envelope.Gate.SetValue(0);
    }

    protected override void GenerateMix()
    {
        // TODO create a passthrough automation node to link one input to another input in the group
        // for now we will do it manually
        //_automationNode.Value.SetValue(Frequency);

        base.GenerateMix();
    }
}


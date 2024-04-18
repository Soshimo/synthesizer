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

//var lfo = new Oscillator(audioProvider, 2f);

//var mulAdd = new MulAdd(audioProvider, 100, foo);
//lfo.Connect(mulAdd);

//var mulAdd2 = new MulAdd(audioProvider, 100, bar);
//lfo.Connect(mulAdd2);




var oscGain = new Gain(audioProvider);
var voice = new SynthVoice(audioProvider, foo, bar);
voice.Connect(oscGain);

//mulAdd.Connect(voice);
//mulAdd2.Connect(voice, 1);

//mulAdd.Connect(voice);

//var gain2 = new Gain(audioProvider);
//osc2.Connect(gain2);

audioProvider.ConnectToOutput(oscGain);

audioProvider.Play();


//env.Gate.SetValue(1);
//Thread.Sleep(250);
//env.Gate.SetValue(0);

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
        }
    }
}


class SynthVoice : GroupNode
{
    public SynthVoice(IAudioProvider provider, float frequency1, float frequency2) : base(provider, 2, 1)
    {
        var osc1 = new Oscillator(provider, frequency1, WaveShape.Sine);
        var osc2 = new Oscillator(provider, frequency2, WaveShape.Sawtooth);

        _ = new Automation(this, 0, frequency1);
        _ = new Automation(this, 1, frequency2);

        var mixer = new Mixer(provider, 2);

        osc1.Connect(mixer);
        osc2.Connect(mixer, 1);

        var gain = new Gain(provider);
        mixer.Connect(gain);

        //var lfo = new Oscillator(provider, 2f);

        //var mulAdd = new MulAdd(provider, 100, frequency1);
        //lfo.Connect(mulAdd);

        //var mulAdd2 = new MulAdd(provider, 100, frequency2);
        //lfo.Connect(mulAdd2);

        //mulAdd.Connect(osc1);
        //mulAdd2.Connect(osc2);


        gain.Connect(OutputPassThroughNodes[0]);
    }

}


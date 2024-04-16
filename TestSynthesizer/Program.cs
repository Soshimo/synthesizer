// See https://aka.ms/new-console-template for more information

using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.DSP;
using SythesizerLibrary.Operators;
using SythesizerLibrary.Scale;
using SythesizerLibrary.Tuning;


var audioProvider = new WasapiAudioProvider();

var minorKeyOfBFlat = new MinorScale("Bb", new WesternTuning());  // 2^(n/12)

var foo = (float)minorKeyOfBFlat.GetFrequency(0, 3);
var bar = (float)minorKeyOfBFlat.GetFrequency(4, 3);

var osc1 = new Oscillator(audioProvider, foo, WaveShape.Sawtooth);
var oscGain = new Gain(audioProvider, .5);
osc1.Connect(oscGain);

//var osc2 = new Oscillator(audioProvider, bar, WaveShape.Triangle);
//var osc2Gain = new Gain(audioProvider, .5);
//osc2.Connect(osc2Gain);

//var noise = new Noise(audioProvider, NoiseColor.Pink);
//var noiseGain = new Gain(audioProvider, .1);
//noise.Connect(noiseGain);

//var mixer = new Mixer(audioProvider, 3);

//oscGain.Connect(mixer, 0);
//osc2Gain.Connect(mixer, 1);
//noiseGain.Connect(mixer, 2);

var env = new ADSREnvelope(audioProvider, 0, .7, 1, .75, 1.5);

//osc1.Connect(env);

//   /\___
//  /     \
// /      \

env.Connect(oscGain, 1);

audioProvider.ConnectToOutput(oscGain);

audioProvider.Play();


env.Gate.SetValue(1);
Thread.Sleep(250);
env.Gate.SetValue(0);

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



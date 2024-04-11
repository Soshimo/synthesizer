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

var foo = (float)minorKeyOfBFlat.GetFrequency(0, 2);
var bar = (float)minorKeyOfBFlat.GetFrequency(3, 2);

var osc1 = new Oscillator(audioProvider, foo);

var noise = new Noise(audioProvider, NoiseColor.Brown);
var noiseGain = new Gain(audioProvider, .05);

noise.Connect(noiseGain);

var gain = new Gain(audioProvider, 1);

noiseGain.Connect(gain);
osc1.Connect(gain);

//var lfo = new Oscillator(audioProvider, 2, WaveShape.Sawtooth);

//lfo.Connect(gain, 1);

//var osc2 = new Oscillator(audioProvider, bar);

//var filter = new LP12Filter(audioProvider, 1000);

//osc1.Connect(filter);
//osc2.Connect(filter);

//var filterLfo = new Oscillator(audioProvider,.5f);
//var filterMod = new MulAdd(audioProvider, 150, foo);

//filterLfo.Connect(filterMod);
//filterMod.Connect(filter, 1);  // connect lfo to cutoff frequency

audioProvider.ConnectToOutput(gain);

audioProvider.Play();

while (true)
{
    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
    {
        break;
    }
}



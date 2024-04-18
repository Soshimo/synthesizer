// See https://aka.ms/new-console-template for more information

using SynthesizerLibrary.DSP;
using SynthesizerLibrary.Util;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;
using SythesizerLibrary.DSP;
using SythesizerLibrary.Operators;
using SythesizerLibrary.Scale;
using SythesizerLibrary.Tuning;


var audioProvider = new WasapiAudioProvider();

//var minorKeyOfBFlat = new MinorScale("Bb", new WesternTuning());  // 2^(n/12)

//var foo = (float)minorKeyOfBFlat.GetFrequency(0, 3);
//var bar = (float)minorKeyOfBFlat.GetFrequency(4, 3);

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

var oscGain = new Gain(audioProvider);
var voice = new Voice(audioProvider);
voice.Connect(oscGain);

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




public class Voice : AudioNode
{
    private Gain oscGain;
    private Oscillator lfo;
    
    public Voice(IAudioProvider provider) : base(provider, 1, 1, true)
    {
        var osc = new Oscillator(provider);
        oscGain = new Gain(provider, .5);


        osc.Connect(oscGain);
        oscGain.Connect(OutputPassThroughNodes[0]);

        lfo = new Oscillator(provider, 2f);

        var multiAdd = new MulAdd(provider, .2, .5);
        lfo.Connect(multiAdd);

        multiAdd.Connect(osc, 1);
        //lfo.Connect(osc, 1);
        
        //osc.Connect(oscGain);

        //lfo.Connect(oscGain, 1);

        oscGain.Connect(OutputPassThroughNodes[0]);
    }

}


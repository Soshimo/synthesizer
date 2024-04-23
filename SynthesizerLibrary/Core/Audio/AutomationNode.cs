using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core.Audio;

public class AutomationNode : AudioNode
{
    public Automation Value { get; }
    public AutomationNode(IAudioProvider provider, double value = 0) 
        : base(provider, 1, 1, "AutomationNode")
    {
        Value = new Automation(this, 0, value);
    }

    protected override void GenerateMix()
    {
        Outputs[0].Samples[0] = Value;
        base.GenerateMix();
    }
}
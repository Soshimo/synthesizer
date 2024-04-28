namespace SynthesizerUI.Services;

public interface IPoolObject
{
    bool IsComplete { get; }
    void Reset();
}
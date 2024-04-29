using SynthesizerLibrary.Core.Audio;
using SynthesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.Core;

public class Scheduler : PassThroughNode
{
    private double _bpm;
    private readonly PriorityQueue<AudioEvent?, double> _queue;  // .NET 6+ supports priority queue
    private double _time;
    private double _lastBeatTime;
    private double _beatLength;
    private readonly int _beatsPerBar;
    private int _beat;
    private int _bar;
    private int _beatInBar;
    private double _seconds;

    public Scheduler(IAudioProvider provider, double bpm = 120) 
        : base(provider, 1, 1)
    {
        _bpm = bpm;
        _queue = new PriorityQueue<AudioEvent?, double>();
        _time = 0;
        _beat = 0;
        _beatInBar = 0;
        _bar = 0;
        _seconds = 0;
        _beatsPerBar = 4; // Default value, can be parameterized

        _lastBeatTime = 0;
        _beatLength = 60 / _bpm * AudioProvider.SampleRate;
    }

    public void SetTempo(double bpm)
    {
        _bpm = bpm;
        _beatLength = 60 / _bpm * AudioProvider.SampleRate;
    }

    public AudioEvent? AddRelative(double beats, Action callback)
    {
        var eventTime = _time + beats * _beatLength;
        var newEvent = new AudioEvent(callback, eventTime);
        _queue.Enqueue(newEvent, eventTime);
        return newEvent;
    }

    public AudioEvent? AddAbsolute(double beat, Action callback)
    {
        if (beat < _beat ||
            (Math.Abs(beat - _beat) < double.Epsilon && _time > _lastBeatTime))
        {
            return null;  // Event time has already passed
        }
        var eventTime = _lastBeatTime + (beat - _beat) * _beatLength;
        var newEvent = new AudioEvent(callback, eventTime);
        _queue.Enqueue(newEvent, eventTime);
        return newEvent;
    }

    public override void Tick()
    {
        base.Tick();  // Continue processing as a PassThroughNode
        TickClock();

        while (_queue.TryPeek(out var nextEvent, out var eventTime) &&
               eventTime <= _time)
        {
            _queue.Dequeue();
            ProcessEvent(nextEvent);
        }
    }

    private void TickClock()
    {
        _time += 1;
        _seconds = _time / AudioProvider.SampleRate;
        if (_time >= _lastBeatTime + _beatLength)
        {
            _beat++;
            _beatInBar++;
            if (_beatInBar >= _beatsPerBar)
            {
                _bar++;
                _beatInBar = 0;
            }
            _lastBeatTime += _beatLength;
        }
    }

    private void ProcessEvent(AudioEvent? evt) {
        evt.Callback();
        // Re-schedule if repeating events are needed (not shown here for simplicity)
    }


    public record AudioEvent(Action Callback, double Time);
}
﻿using System.Runtime.CompilerServices;
using SynthesizerLibrary.Core;
using SynthesizerLibrary.Core.Audio;
using SythesizerLibrary.Core;
using SythesizerLibrary.Core.Audio;
using SythesizerLibrary.Core.Audio.Interface;

namespace SynthesizerLibrary.DSP;

public class Envelope : AudioNode
{
    public Automation Gate { get; }

    private readonly IList<double> _levels;
    private readonly IList<double> _times;

    private readonly int? _releaseStage;

    private int? _stage;
    private int? _time;
    private int? _changeTime;

    private double _level;
    private double _delta;
    private bool _gateOn;

    public event EventHandler<EventArgs>? Complete; 
    protected Envelope(IAudioProvider provider, IList<double> levels, IList<double> times, double? gate, int? releaseStage = null ) : base(provider, 1, 1)
    {
        Gate = new Automation(this, 0, gate ?? 1);

        _levels = levels.ToList();
        _times = times.ToList();
        _releaseStage = releaseStage;

        _stage = null;
        _time = null;
        _changeTime = null;

        _level = _levels[0];
        _delta = 0;
        _gateOn = false;

    }

    protected override void GenerateMix()
    {
        var gate = Gate.GetValue();
        var stageChanged = false;

        if (gate > 0 && !_gateOn)
        {
            _gateOn = true;
            _stage = 0;
            _time = 0;
            _delta = 0;
            _level = _levels[0];
            if (_stage != _releaseStage)
            {
                stageChanged = true;
            }
        }

        if (_gateOn && gate <= 0)
        {
            _gateOn = false;
            if (_releaseStage.HasValue)
            {
                _stage = _releaseStage;
                stageChanged = true;
            }
        }

        if (_changeTime.HasValue)
        {
            _time += 1;
            if (_time >= _changeTime)
            {
                _stage += 1;
                if (_stage != _releaseStage)
                {
                    stageChanged = true;
                }
                else
                {
                    _changeTime = null;
                    _delta = 0;
                }
            }
        }

        if (stageChanged)
        {
            if (_stage != _times.Count)
            {
                if (_stage != null)
                {
                    _delta = CalculateDelta(_stage.Value, _level);
                    if (_time != null) _changeTime = CalculateChangeTime(_stage.Value, _time.Value);
                }
            }
            else
            {
                OnComplete();
                _stage = null;
                _time = null;
                _changeTime = null;

                _delta = 0;
            }
        }

        _level += _delta;
        Outputs[0].Samples[0] = _level;
    }


    private double CalculateDelta(int stage, double level)
    {
        if(stage >= _levels.Count) return 0;

        var delta = _levels[stage + 1] - level;
        var time = _times[stage] * AudioProvider.SampleRate;
        return (delta / time);
    }

    private int CalculateChangeTime(int stage, int time)
    {
        var stageTime = _times[stage] * AudioProvider.SampleRate;
        return (int)(time + stageTime);
    }

    protected virtual void OnComplete()
    {
        Complete?.Invoke(this, EventArgs.Empty);
    }
}
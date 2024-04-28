﻿using SynthesizerUI.Model;

namespace SynthesizerUI.Services.Interface;

public interface ISynthesizerService
{
    void NoteOn(string key, VoiceData data);
    void NoteOff(string noteKey);
}
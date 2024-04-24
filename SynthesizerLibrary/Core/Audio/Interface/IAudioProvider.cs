using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SythesizerLibrary.Core.Audio.Interface;

public interface IAudioProvider
{
    bool NeedTraverse { get; set; }
    int Channels { get; }
    int SampleRate { get; }
    int TotalWriteTime { get; }
    void ConnectToOutput(IAudioNode? node);
}
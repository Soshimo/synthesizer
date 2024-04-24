using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SythesizerLibrary.Core.Audio.Interface;

public interface IChannel
{
    IAudioNode? Node { get; }
    int Index { get; }
    List<IChannel> Connected { get;  }
    int Channels { set; get; }
    void Connect(IChannel from);
    void Disconnect(IChannel from);
    List<double> Samples { get; set; }
}
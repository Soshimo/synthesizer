using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SythesizerLibrary.Core.Audio.Interface;

public interface IAudioNode
{
    IList<IChannel> Inputs { get; }
    IList<IChannel> Outputs { get; }

    void Connect(IAudioNode node, int inputIndex, int outputIndex);
    void Disconnect(IAudioNode node, int output, int input);
    void Tick();
    List<IAudioNode> Traverse(List<IAudioNode> nodes);

    List<IAudioNode> InputPassThroughNodes { get; }
    List<IAudioNode> OutputPassThroughNodes { get; set; }

    void Remove();
    bool IsAggregate { get; }
    bool NeedsTraverse { get; set; }

    int GetWriteTime();
}
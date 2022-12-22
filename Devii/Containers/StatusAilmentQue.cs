using System.Collections.Concurrent;
using Devii.Creatures;
using Devii.StatusAilments;

namespace Devii.Containers;

public sealed class StatusAilmentQue
{
    public IStatusAilment StatusAilment { get;  } 
    public Creature Self { get; }
    public Creature Target { get; }

    public StatusAilmentQue(IStatusAilment statusAilment, Creature self, Creature target)
    {
        StatusAilment = statusAilment;
        Self = self;
        Target = target;
    }
}
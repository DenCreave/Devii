using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;
using Devii.Exceptions;

namespace Devii.StatusAilments;

public sealed class InsultDebuff : IStatusAilment
{
    public string Name => "Exposed to insults";
    public int DurationMillisec => 6000;
    public int? MaxTicks => 3;
    public int CurrentTicks { get; set; }
    public bool IsHarmful => true;
    public bool IsDisplayed => true;
    
    public Creature Self { get; set; }
    public Creature Target { get; set; }
    public StatusAilmentTypes[] Types => new[] { StatusAilmentTypes.Debuff,StatusAilmentTypes.DamageOverTime,StatusAilmentTypes.OverTimeEffect };
    public DateTime TimeOfAcquisition { get; init; }

    public InsultDebuff()
    {
        CurrentTicks = 1;
        TimeOfAcquisition = new DateTime();
        TimeOfAcquisition = DateTime.Now;
    }
    
    public void RequestAction(Creature self, Creature target)
    {
        Self = self;
        Target = target;
        StatusAilmentQue ailmentQue = new StatusAilmentQue(this, self, target);
        self.StatusAilmentQues.Enqueue(ailmentQue);
    }

    public void Activate(Creature self, Creature target)
    {
        target.StatusAilments.AddOrUpdate(Name,this, (key,value) =>
        {
            return this;
        });
    }

    public void TakeAction(Creature self, Creature target)
    {
        Target.TakeDmg(Self.DealDmg("Insult"));
        ++CurrentTicks;
    }

    public void TakeAction(Creature self, Creature target,ref double? value)
    {
        throw new BuffTakeActionError(Name);
    }

    public void TakeAction(Creature self, Creature target, string value)
    {
        throw new BuffTakeActionError(Name);
    }

    public void Deactivate(Creature self, Creature target)
    {
        self.StatusAilments.TryRemove(Name, out _);
    }
}
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;
using Devii.Exceptions;

namespace Devii.StatusAilments;

public sealed class GruppenBuff : IStatusAilment
{
    public string Name => "Gruppen's Ecstasy";
    public int DurationMillisec => 9000;
    public int? MaxTicks => 3;
    public int CurrentTicks { get; set; }
    public bool IsHarmful => false;
    public bool IsDisplayed => true;
    public StatusAilmentTypes[] Types => new[] { StatusAilmentTypes.Buff, StatusAilmentTypes.HealingOverTime, StatusAilmentTypes.OverTimeEffect };
    public DateTime TimeOfAcquisition { get; init; }

    public GruppenBuff()
    {
        CurrentTicks = 1;
        TimeOfAcquisition = new DateTime();
        TimeOfAcquisition = DateTime.Now;
    }
    public void RequestAction(Creature self, Creature target)
    {
        StatusAilmentQue ailmentQue = new StatusAilmentQue(this, self, target);
        self.StatusAilmentQues.Enqueue(ailmentQue);
    }

    public void Activate(Creature self, Creature target)
    {
        self.StatusAilments.AddOrUpdate(Name,this, (key,value) =>
        {
            return this;
        });
    }

    public void TakeAction(Creature self, Creature target)
    {
        self.TakeHealing(self.DealHealing("Gruppen"));
        ++CurrentTicks;
    }

    public void TakeAction(Creature self, Creature target, ref double? value)
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
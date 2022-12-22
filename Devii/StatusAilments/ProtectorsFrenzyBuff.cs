using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;
using Devii.Exceptions;

namespace Devii.StatusAilments;

public sealed class ProtectorsFrenzyBuff : IStatusAilment
{
    public string Name => "Protector's Frenzy";
    public int DurationMillisec => 4200;
    public double Value => 3.2;

    public double HasteValue => -0.5;
    public int? MaxTicks => null;
    public int CurrentTicks { get; set; }
    public bool IsHarmful => false;
    public bool IsDisplayed => true;

    public StatusAilmentTypes[] Types => new[] { StatusAilmentTypes.Buff, StatusAilmentTypes.DamageIncrease };
    public DateTime TimeOfAcquisition { get; init; }

    public ProtectorsFrenzyBuff()
    {
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
        bool alreadyExists = false;
        self.StatusAilments.AddOrUpdate(Name, this, (key, value) =>
        {
            alreadyExists = true;
            return this;
        });
        if (!alreadyExists)
        {
            self.SetHaste(HasteValue);
            self.SetDamageDoneMultiplier(Value);
        }
    }

    public void TakeAction(Creature self, Creature target)
    {
        throw new BuffTakeActionError(Name);
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
        if (self.StatusAilments.TryRemove(Name, out _))
        {
            self.SetDamageDoneMultiplier(-Value);
            self.SetHaste(-HasteValue);
        }
    }
}
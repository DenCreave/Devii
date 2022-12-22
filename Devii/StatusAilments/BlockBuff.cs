using System.Collections.Concurrent;
using System.Data;
using System.Xml.Schema;
using Devii.Containers;
using Devii.Creatures;
using Devii.Enums;
using Devii.Exceptions;

namespace Devii.StatusAilments;

public sealed class BlockBuff : IStatusAilment
{
    public string Name => "Block";
    public int DurationMillisec => 200;
    public int? MaxTicks => 1;
    public int CurrentTicks { get; set; }
    public bool IsHarmful => false;
    public bool IsDisplayed => true;
    
    public StatusAilmentTypes[] Types => new[] { StatusAilmentTypes.WhenStruck, StatusAilmentTypes.ActionIntegerValueRequired };

    public DateTime TimeOfAcquisition { get; init; }

    public BlockBuff()
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

    /// <summary>
    /// since this buffs takeaction is called in take dmg with a parameter, but will also be called by the buff watcher
    /// takeaction here will call the deactivate method
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <exception cref="BuffTakeActionError"></exception>
    public void TakeAction(Creature self, Creature target)
    {
        Deactivate(self,target);
    }

    /// <summary>
    /// so the was as is follows: buffs and skills will be operated through a designated class
    /// that class will also ask the creature self to roll a damage number, and the creature target to take the damage
    /// so all the damage taken will be made by the creature -> this way this function can be invoked via the creature
    /// inside a take damage function where it checks for buffs and various flags
    ///
    /// summary:
    /// step 1 :requested by creatured
    /// step 2 :initiated by designated serverlike class -> Skill.CastMe(self,target)
    /// step 3 : in case creature takes dmg or heals or gets buffed or something like that -> those are taken inside its method Creature.takedmg Creature.TakeHealing
    /// Creature.DealDmg(string spellNameCast){} will also have flags like OnSkillUse
    /// Creature.DealDmg(string spellnamecast){ Where Skilllists.key=spellnamcast. cast it
    ///
    /// i should just go to sleep
    ///
    /// 
    /// </summary>
    /// <param name="self"></param>
    /// <param name="target"></param>
    /// <param name="value"></param>
    public void TakeAction(Creature self, Creature target, ref double? value) 
    {
        if (CurrentTicks == 1)
        {
            //CounterBuff buffUp = new CounterBuff(value is not null ? (int)value : 0);
            //buffUp.RequestAction(self,target);
            ProtectorsFrenzyBuff frenzyBuff = new ProtectorsFrenzyBuff();
            frenzyBuff.RequestAction(self,target);
            self.PersonalCombatLog.LogAction(String.Empty, 0,false,false,$"BLOCKED ~( {value} )~");
            target.TakeDmg(value*3);
            value = 0;
            ++CurrentTicks;
        }

        //Deactivate(self,target);
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
using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.StatusAilments;

namespace Devii.Skills;

public sealed class GruppenSkill :ISkill
{
    public string Title => "Gruppen";
    public bool AffectedByGCD => true;
    public int? Value => 3;
    public double EffectiveRate => 0.69;
    public bool IsHarmful => false;
    public string Description => "Causes the player to heal via a quickie gruppensex. If this takes action while under Protectors Frenzy, it will leaving the player" +
                                 " in a lustful ecstasy and will be healed 3 times under 9 seconds.";
    
    
    public void RequestAction( Creature self, Creature target)
    {
        SkillQue queMe = new SkillQue(this, self, target);
        self.SkillQues.Enqueue(queMe);
    }

    public void CastMe(Creature self, Creature target)
    {
        //it would be fun to play sound effects based on skills like this isnt it?
        self.TakeHealing(self.DealHealing(Title));
        if (self.StatusAilments.ContainsKey("Protector's Frenzy"))
        {
            GruppenBuff gruppenBuff = new GruppenBuff();
            gruppenBuff.RequestAction(self, target);
        }
        self.SetLastGCDTrigger();
    }
}
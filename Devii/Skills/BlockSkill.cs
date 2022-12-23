using System.Collections.Concurrent;
using Devii.Containers;
using Devii.Creatures;
using Devii.StatusAilments;

namespace Devii.Skills;

public class BlockSkill : ISkill
{
    public string Title => "Block";
    public bool AffectedByGCD => true;
    public bool IsHarmful => false;

    public int? Value => null; 
    public double EffectiveRate => 0;

    public string Description =>
        "I'm a fan of Devil May Cry, so I tried to make a skill like royal guard, but unlike in dmc3," +
        " you will have double the time (0.2 sec) to perform a perfect block and theres no penalty for miss timing," +
        " your block \"buff\" will just simply expire.  On a successful counter it will increase your haste (reduce global cooldown by 50%)" +
        " for 4.2 seconds and will enter a trance like state where damage done is increases by 400% and it will return" +
        " the amount of damage blocked threefold.";
    
    public void RequestAction(Creature self, Creature target)
    {
        SkillQue queMe = new SkillQue(this, self, target);
        self.SkillQues.Enqueue(queMe);
    }
    
    public void CastMe(Creature self, Creature target)
    {
        BlockBuff buffUp = new BlockBuff();
        buffUp.RequestAction(self,target);
        self.SetLastGCDTrigger();
    }
}
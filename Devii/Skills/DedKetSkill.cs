using Devii.Containers;
using Devii.Creatures;

namespace Devii.Skills;

public sealed class DedKetSkill : ISkill
{
    public string Title => "DedKet";
    public string Description => "Eh, you'll see";
    public int? Value => null;
    public double EffectiveRate => 1;
    public bool AffectedByGCD => true;
    public bool IsHarmful => true;
    public void RequestAction(Creature self, Creature target)
    {
        SkillQue queMe = new SkillQue(this, self, target);
        self.SkillQues.Enqueue(queMe);
    }

    public void CastMe(Creature self, Creature target)
    {
        target.TakeDmg(target.GetMaxHealth());
        self.SetLastGCDTrigger();
    }
}
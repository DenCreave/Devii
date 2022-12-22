using System.ComponentModel.Design.Serialization;
using Devii.Containers;
using Devii.Creatures;

namespace Devii.Skills;

public sealed class CatClawSkill : ISkill
{
    public string Title => "CatClaw";

    public string Description =>
        "Nyanyame nyanyajyuunyanya-do no nyarabi de nyakunyaku inyanyaku nyanyahan nyanyadai nyanynaku nyarabete nyaganyagame.";

    public int? Value => 4;
    public double EffectiveRate => 0.69;
    public bool AffectedByGCD => true;
    public bool IsHarmful => true;
    public void RequestAction(Creature self, Creature target)
    {
        SkillQue queMe = new SkillQue(this, self, target);
        self.SkillQues.Enqueue(queMe);
    }

    public void CastMe(Creature self, Creature target)
    {
        target.TakeDmg(self.DealDmg(Title));
        self.SetLastGCDTrigger();
    }
}
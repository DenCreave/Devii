using Devii.Creatures;
using Devii.Skills;

namespace Devii.Containers;

public sealed class SkillQue
{
    public ISkill Skill { get; }
    public Creature Self { get; }
    public Creature Target { get; }

    public SkillQue(ISkill skill, Creature self, Creature target)
    {
        Skill = skill;
        Self = self;
        Target = target;
    }
}
using System.Collections.Concurrent;
using Devii.Containers;
using Devii.StatusAilments;
using Devii.Creatures;

namespace Devii.Skills;

public interface ISkill
{
    string Title { get; }
    string Description { get; }
    
    int? Value { get; }
    
    double EffectiveRate { get; } //an easy way to nerf stuff, like from 90% it goes down to 60% and hit 33% less

    ///so dmg done goes like:
    /// give the skills value to SELF.DEALDMG
    /// magic happens there with numbers like dmg multiplier and such
    /// then effectiveness reduces that number and gives it to the TARGET.TAKEDMG
    /// so it can get damage

    bool AffectedByGCD { get; }
    bool IsHarmful { get; }

    void RequestAction(Creature self, Creature target);
    void CastMe(Creature self, Creature target);
}
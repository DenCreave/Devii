using System;
using System.Collections;
using System.Threading.Tasks;
using Devii.Creatures;
using Devii.Enums;

namespace Devii.AutoTasks;

public sealed class BuffWatcher
{
    private Creature Player { get; }
    private Creature Npc { get; }

    public BuffWatcher(Creature player, Creature npc)
    {
        Player = player;
        Npc = npc;
    }

    private void BuffWatch(Creature creature)
    {
        //i thought about putting these two to a different function, but as i see i skip a foreach with it, so i keep it longer as in code
        if (!creature.StatusAilments.IsEmpty)
        {
            foreach (var key in creature.StatusAilments.Keys)
            {
                if (creature.StatusAilments[key].MaxTicks is not null)
                {
                    if (creature.StatusAilments[key].Types.Contains(StatusAilmentTypes.OverTimeEffect))
                    {
                        if ((creature.StatusAilments[key].DurationMillisec / creature.StatusAilments[key].MaxTicks) *
                            creature.StatusAilments[key].CurrentTicks
                            < DateTime.Now.Subtract(creature.StatusAilments[key].TimeOfAcquisition).TotalMilliseconds)
                        {
                            if (creature.StatusAilments[key].CurrentTicks <= creature.StatusAilments[key].MaxTicks)
                            {
                                creature.StatusAilments[key].TakeAction(creature, creature.Target);
                            }
                        }
                    }
                }

                if (creature.StatusAilments.ContainsKey(key))
                {
                    if ((creature.StatusAilments[key].DurationMillisec < DateTime.Now
                            .Subtract(creature.StatusAilments[key].TimeOfAcquisition).TotalMilliseconds))
                    {
                        creature.StatusAilments[key].Deactivate(creature, creature.Target);
                    }
                    else if (creature.StatusAilments[key].MaxTicks is not null)
                    {
                        if (creature.StatusAilments[key].MaxTicks < creature.StatusAilments[key].CurrentTicks)
                        {
                            creature.StatusAilments[key].Deactivate(creature, creature.Target);
                        }
                    }
                }
            }
        }
    }


    public async Task<Creature> BuffTakeAction()
    {
        await Task.Run(async () =>
        {
            while (Player.GetHealth() > 0 && Npc.GetHealth() > 0)
            {
                BuffWatch(Player);
                BuffWatch(Npc);
                await Task.Delay(5);
            }
            
        });
        return (Npc.GetHealth() == 0 ? Player : Npc );
    }
}
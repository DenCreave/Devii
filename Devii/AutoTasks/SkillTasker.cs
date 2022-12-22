using Devii.Containers;
using Devii.Creatures;

namespace Devii.AutoTasks;

public sealed class SkillTasker
{
    public Creature Player { get; }
    public Creature Npc { get; }

    public SkillTasker(Creature player, Creature npc)
    {
        Player = player;
        Npc = npc;
    }

    
    /// <summary>
    /// returns the winning creature
    /// </summary>
    /// <returns></returns>
    public async Task<Creature> SkillExecuter()
    {
        SkillQue swapper = new SkillQue(null,null,null);
        await Task.Run(async () =>
        {
            while (Player.GetHealth() > 0 && Npc.GetHealth() > 0)
            {
                if (Player.SkillQues.TryDequeue(out swapper))
                {
                    swapper.Skill.CastMe(swapper.Self, swapper.Target);
                }

                if (Npc.SkillQues.TryDequeue(out swapper))
                {
                    swapper.Skill.CastMe(swapper.Self, swapper.Target);
                }

                await Task.Delay(5);
            }
        });
        return (Npc.GetHealth() == 0 ? Player : Npc );
    }
}
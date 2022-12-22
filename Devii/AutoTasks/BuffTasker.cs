using System.Threading.Tasks;
using Devii.Containers;
using Devii.Creatures;

namespace Devii.AutoTasks;

public sealed class BuffTasker
{
    private Creature Player { get; }
    private Creature Npc { get; }

    public BuffTasker(Creature player, Creature npc)
    {
        Player = player;
        Npc = npc;
    }
    public async Task<Creature> BuffExecuter()
    {
        StatusAilmentQue swapper = new StatusAilmentQue(null,null,null);
        await Task.Run(async () =>
        {
            while (Player.GetHealth() > 0 && Npc.GetHealth() > 0)
            {
                if (Player.StatusAilmentQues.TryDequeue(out swapper))
                {
                    swapper.StatusAilment.Activate(swapper.Self, swapper.Target);
                }

                if (Npc.StatusAilmentQues.TryDequeue(out swapper))
                {
                    swapper.StatusAilment.Activate(swapper.Self, swapper.Target);
                }

                await Task.Delay(5);
            }
        });
        return (Npc.GetHealth() == 0 ? Player : Npc );
    }
    
}
using System.Security.Principal;
using System.Text;
using Devii.Creatures;
using Devii.Handlers;
using Spectre.Console;

namespace Devii.AutoTasks;

public sealed class UIUpdater
{
    private Creature Player { get; set; }
    private Creature Npc { get; set; }
    string CatColor => "[deeppink3_1]";
    string PlayerColor => "[orangered1]";
    string EndTag => "[/]";

    private int TableRows => 13;
    public UIUpdater(Creature player, Creature npc)
    {
        Player = player;
        Npc = npc;
    }

    private Panel CreaturePanel(Creature creature)
    {
        //this is the log
        //this isnt thread safe i guess, but im just reading so  it shouldnt cause problems
        return new Panel(UIOperator.IntoALineWithNewLine(creature.PersonalCombatLog.PlaceHolders));
    }
    private BarChart[] CreatureBarChartMaker(Creature creature)
    {
        List<BarChart> barCharts = new List<BarChart>();
        barCharts.Add(new BarChart().AddItem("Health", creature.GetHealth(),Color.Green).WithMaxValue(creature.GetMaxHealth()));
        foreach (var item in creature.StatusAilments.ToArray())
        {
            if (item.Value.IsDisplayed)
            {
                barCharts.Add(new BarChart().AddItem(item.Key,
                    Math.Round((item.Value.DurationMillisec - (DateTime.Now.Subtract(item.Value.TimeOfAcquisition).TotalMilliseconds)) * 0.01,0),
                    Color.Aquamarine1).WithMaxValue(Math.Round((item.Value.DurationMillisec*0.01),0)));
            }
        }

        return barCharts.ToArray();
    }

    private BarChart CooldownBarChart(Creature creature)
    {
        BarChart retme = new BarChart();
        
        if (DateTime.Now.Subtract(creature.GetLastGCDTrigger()).TotalMilliseconds < creature.BaseGlobalCoolDownMs*creature.GetHaste())
        {
            retme.AddItem("[green]CD[/]",
                Math.Round(DateTime.Now.Subtract(creature.GetLastGCDTrigger()).TotalMilliseconds * 0.01, 1),Color.Green).WithMaxValue(creature.BaseGlobalCoolDownMs * creature.GetHaste() * 0.01);
        }
        else
        {
            retme.AddItem("[green]CD[/]", 0,Color.Green);
        }

        return retme;
    }

    private Grid GridUserInterface()
    {
        Grid retme = new Grid();
        retme.AddColumn(); //one for  pic one for text box
        retme.AddColumn();
        retme.Alignment(Justify.Center);

        retme.AddRow(new Markup[]
        {
            new Markup(UIOperator.ColoredStringBuilder(CatColor,Npc.Name,EndTag)),
            new Markup(UIOperator.ColoredStringBuilder(CatColor,Npc.GetSpeechBox(),EndTag))
        });

        retme.AddRow(new Markup[]
        {
            new Markup(" "),//empty row 
            new Markup(" ")
        });
        
        retme.AddRow(new Markup[]
        {
            new Markup(UIOperator.ColoredStringBuilder(PlayerColor,Player.Name)),
            new Markup(UIOperator.ColoredStringBuilder(PlayerColor,Player.GetSpeechBox()))
            
        });
        return retme;
    }

    private Table CreatureTableMaker(Creature creature)
    {
        Table creatureTable = new Table();
        creatureTable.Border=TableBorder.Square;
        creatureTable.BorderColor(Color.Green);
        creatureTable.AddColumn(new TableColumn(new Markup(UIOperator.ColoredStringBuilder("[darkmagenta]",creature.Name,"[/]"))));
        for (int i = 0; i < TableRows; i++)
        {
            creatureTable.AddEmptyRow();
        }
        BarChart[] creatureBarChart = CreatureBarChartMaker(creature);
        for (int i = 0; i < creatureBarChart.Length && i < TableRows; i++)
        {
            creatureTable.UpdateCell(i, 0, creatureBarChart[i]);
        }
        Panel creaturePanel = CreaturePanel(creature);
        creaturePanel.BorderColor(Color.Green);
        creatureTable.AddRow(creaturePanel); // this row is the last one, so can use it as @tableRows
        return creatureTable;
    }

    private string ListPlayerSkills()
    {
        string space8 = "        "; //8 times 
        int counter = 1;
        StringBuilder sb = new StringBuilder();
        foreach (var skill in Player.SkillKeysOrdered)
        {
            sb.Append(skill);
            sb.Append($" ~ {counter}");
            sb.Append(space8);
            ++counter;
        }

        sb.Length -= 8;
        string color = "[lightsalmon3_1]";
        string endTag = "[/]";
        return UIOperator.ColoredStringBuilder(color, sb.ToString(), endTag);
    }
    
    
    public async Task<Creature> FightUI()
    {
        await Task.Run(() =>
        {

            //player tables, right side

            Table playerTable = CreatureTableMaker(Player);
            

            //npc tables, right side
            Table npcTable = CreatureTableMaker(Npc);


            //GUI
            Grid GUI = GridUserInterface();

            Table wrapperRoot = new Table();
            wrapperRoot.BorderColor(Color.Green);
            wrapperRoot.Border=TableBorder.Heavy;
            //wrapperRoot.Alignment == Justify.Center
            wrapperRoot.AddColumns("if you see this header rendered", "then consider it an easter egg",
                "cos it means i fucked up bad").Centered();
            wrapperRoot.HideHeaders();

            wrapperRoot.AddEmptyRow();
            wrapperRoot.AddEmptyRow();

            wrapperRoot.UpdateCell(0, 0, playerTable);
            wrapperRoot.UpdateCell(0, 1, GUI).Alignment(Justify.Center);
            wrapperRoot.UpdateCell(0, 2, npcTable);

            wrapperRoot.UpdateCell(1, 0, CooldownBarChart(Player));
            wrapperRoot.UpdateCell(1, 1, new Markup(ListPlayerSkills())).Centered();
            wrapperRoot.UpdateCell(1, 2, CooldownBarChart(Npc));

            
            AnsiConsole.Live(wrapperRoot).StartAsync(async ui =>
            {
                ui.Refresh();
                while (Npc.GetHealth() > 0 && Player.GetHealth() > 0)
                {

                    wrapperRoot.UpdateCell(0, 0, CreatureTableMaker(Player));
                    wrapperRoot.UpdateCell(0, 1, GridUserInterface());
                    wrapperRoot.UpdateCell(0, 2, CreatureTableMaker(Npc));

                    wrapperRoot.UpdateCell(1, 0, CooldownBarChart(Player));
                    //wrapperRoot.UpdateCell(1, 1, new Markup(ListPlayerSkills()));
                    wrapperRoot.UpdateCell(1, 2, CooldownBarChart(Npc));
                    ui.Refresh();
                    Thread.Sleep(14); //to give it near 60 fps
                }
                
                wrapperRoot.UpdateCell(0, 0, CreatureTableMaker(Player));
                wrapperRoot.UpdateCell(0, 1, GridUserInterface());
                wrapperRoot.UpdateCell(0, 2, CreatureTableMaker(Npc));

                wrapperRoot.UpdateCell(1, 0, CooldownBarChart(Player));
                //wrapperRoot.UpdateCell(1, 1, new Markup(ListPlayerSkills()));
                wrapperRoot.UpdateCell(1, 2, CooldownBarChart(Npc));
                wrapperRoot.Caption(UIOperator.ColoredStringBuilder("[lightsalmon3_1]", "the fight is over\nPress any key to continue"));
                ui.Refresh();
                await Task.Delay(14);
                //Thread.Sleep(14); //to give it near 60 fps
            });


        });
        return (Npc.GetHealth() == 0 ? Player : Npc );
    }
}
using System.Collections.Concurrent;
using System.Text;
using Devii.AutoTasks;
using Devii.Creatures;
using Devii.Enums;
using Devii.Globals;
using Devii.Handlers;
using Devii.ObjektumReferendum;
using Devii.Skills;
using Spectre.Console;

namespace Devii.LevelLibraries;

public sealed class LevelTwo
{

    public static void Level_2_Win()
    {
        string Offsetter(string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string[] split = input.Split('\n');
            
            foreach (var item in split)
            {
                stringBuilder.Append($" {item}\n");
            }
            
            return stringBuilder.ToString();
        }
        Console.Clear();
        IWontModel nope = new IWontModel();
        SzimbaWTF1Model wtf1 = new SzimbaWTF1Model();
        SzimbaWTF2Model wtf2 = new SzimbaWTF2Model();
        NolaPukeModel bleh = new NolaPukeModel();
        string bleh2 = Offsetter(bleh.Text);
        Table root = new Table();
        Grid mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 100;
        mygrid.Alignment = Justify.Center;
        root.Border=TableBorder.None;
        root.AddColumn("header to hide");
        root.HideHeaders();
        root.AddRow("");
        Style style1 = new Style(Color.Grey30);
        Style style2 = new Style(Color.Grey78);
        Style style3 = new Style(Color.Red3_1);
        
        AnsiConsole.Live(root).Start(ui =>
        {
            for (int i = 0; i < 9; i++)
            {
                root.UpdateCell(0, 0, new Text( nope.Text, (i%2==0? style1 :style2)));
                ui.Refresh();
                Thread.Sleep(700);
            }

            root.UpdateCell(0, 0, new Markup(UIOperator.ColoredStringBuilder( "[darkorange3_1]", $"{wtf1.Text}")));
            ui.Refresh();
            Thread.Sleep(1200);

            root.UpdateCell(0, 0, new Markup(UIOperator.ColoredStringBuilder( "[darkorange3_1]", $"{wtf2.Text}")));
            ui.Refresh();
            Thread.Sleep(1200);


            for (int i = 0; i < 9; i++)
            {
                root.UpdateCell(0, 0, new Markup(UIOperator.ColoredStringBuilder("[darkorange3]", (i%2==0? bleh.Text  :  bleh2))));
                ui.Refresh();
                Thread.Sleep(700);
            }


            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[darkorange3_1]", "You are disgusting...")));
            root.UpdateCell(0, 0, mygrid);
            ui.Refresh();
            Thread.Sleep(2800);
            
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[darkorange3_1]", "just...")));
            root.UpdateCell(0, 0, mygrid);
            ui.Refresh();
            Thread.Sleep(1200);
            
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[darkorange3_1]", "how could you?")));
            root.UpdateCell(0, 0, mygrid);
            ui.Refresh();
            Thread.Sleep(4500);

        });
        
        Console.Clear();
        
        mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 100;

        AnsiConsole.Live(mygrid).Start(ui =>
        {
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[red3_1]", "Silence you mongrel!")));
            ui.Refresh();
            Thread.Sleep(1800);
            
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[red3_1]", "You bratty narrator should learn your place!")));
            ui.Refresh();
            Thread.Sleep(2400);
            
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[red3_1]", "On your knees and lick my boots, and when you're done, don't forget to say woof!")));
            ui.Refresh();
            Thread.Sleep(4500);
            
            
            
        });

        FemmeFataleModel mistress = new FemmeFataleModel();
        string[] oneByOne = mistress.Text.Split('\n');
        Console.Clear();
        foreach (var line in oneByOne)
        {
            
            AnsiConsole.Write(new Text( $"{line}\n", style3));
            Thread.Sleep(100);
        }
        Thread.Sleep(6000);
        UIOperator.DestroyBuffer();
        var answer = AnsiConsole.Prompt(new TextPrompt<string>("[red3_1]And now be a good boy and say woof[/]")
            .ChoicesStyle(new Style().Foreground(Color.Red3_1))
            .PromptStyle("deeppink1_1")
            .AddChoices(new[]
            {
                "woof"
            })
            .InvalidChoiceMessage("[red3_1]I don't need a dog that says no. Prove yourself as a good dog and lick my boots.[/]"));


            //.ValidationErrorMessage<string>("[red3_1]I don't need a dog that says no. Prove yourself as a good dog and lick my boots."));
        
        
        var answer2 = AnsiConsole.Prompt(new TextPrompt<string>("[red3_1]And now lick my boots.[/]")
            .ChoicesStyle(new Style().Foreground(Color.Red3_1))
            .PromptStyle("deeppink1_1")
            .AddChoices(new[]
            {
                "*slurp*"
            })
            .InvalidChoiceMessage("[red3_1]C'mon Fido, I believe in you![/]"));
        
        AnsiConsole.Write(new Markup(UIOperator.ColoredStringBuilder("[red3_1]", "Good boy! See? Now I'm willing to keep you")));
        Thread.Sleep(4000);
        SaveFile saveFile = new SaveFile();
        saveFile.UnlockedLevels = LevelUnlock.Level3;
        
        AnsiConsole.Write("\n\n\n\n\n");
        
        if (saveFile.Save())
        {
            AnsiConsole.Write(new Markup(UIOperator.ColoredStringBuilder("[salmon1]", "You have unlocked Level 3 in the chapter select! Or well rather completed the game so you didnt unlock it cos there is no level 3. Maybe later ill come back and make more. See ya")));
        }
        UIOperator.DestroyBuffer();
        Console.ReadKey(true);
        MyGlobals.MySwitch = LevelAdjustingSwitch.MainMenu;
    }
    
    public static void Level_2_Lose()
    {
        AnsiConsole.Status()
            .Start("...", ctx => 
            {
                Thread.Sleep(3000);
                ctx.Status ("well that was sad...");
                Thread.Sleep(3000);

            });
        
        
        var answer = AnsiConsole.Prompt(new TextPrompt<string>("Do you want to quit or restart this level?")
            .ChoicesStyle(new Style().Foreground(Color.Orange1))
            .AddChoices(new[]
            {
                "quit","try", "the fuck am i supposed to do with a fucking lion??"
            }));

        switch (answer)
        {
            case "quit":
            {
                MyGlobals.MySwitch = LevelAdjustingSwitch.Exit;
                break;
            }
            case "try":
            {
                MyGlobals.MySwitch = LevelAdjustingSwitch.LevelTwo;
                break;
            }
            case "the fuck am i supposed to do with a fucking lion??":
            {
                MyGlobals.MySwitch = LevelAdjustingSwitch.MainMenu;
                break;
            }
        }
    }
    
    public static void Level_2_Cinematic_1()
    {
        UIOperator.DestroyBuffer();
        Console.Clear();
        Table root = new Table();
        root.Border=TableBorder.None;
        root.AddColumn("header to hide");
        root.AddColumn("header to hide");
        root.HideHeaders();
        root.AddRow(new Markup("helo"));
        SzimbaModel szimbaModel = new SzimbaModel();
        SzimbanolaModel szimbanolaModel = new SzimbanolaModel();
        string basicColor = "[orangered1]";
        Style style1 = new Style(Color.OrangeRed1);

        AnsiConsole.Live(root).Start(ui =>
        {
            
            
            root.UpdateCell(0,0, new Text( $"Le big Szimba: \n\n\n{szimbaModel.Text}",style1));
            //root.UpdateCell(0, 0, new Markup(UIOperator.ColoredStringBuilder(basicColor, $"Le big Szimba:\n\n{szimbaModel.Text}")));
            ui.Refresh();
            Thread.Sleep(2500);
            
            root.UpdateCell(0, 0, new Markup(UIOperator.ColoredStringBuilder(basicColor, $"{szimbanolaModel.Text}\n\n\n    Found anything sweetheart?")));
            ui.Refresh();
            Thread.Sleep(2250);
            
            root.UpdateCell(0, 0, new Text($"{szimbaModel.Text}\n\n\n     Not yet honey",style1));
            ui.Refresh();
            Thread.Sleep(2200);
            
            root.AddRow( new Markup(UIOperator.ColoredStringBuilder("[red1]", "    -There they are!!")));
            ui.Refresh();
            Thread.Sleep(1800);
        });
    }
    
    
    public static bool Level_2_Fight()
    {
        Console.Clear();
        UIOperator.DestroyBuffer();


        ConcurrentDictionary<string, ISkill> playerSkills = MyLoader.CreateSpellbook(new string[] {"Block","Gruppen","Insult","Magic", "DedKet" });
        ConcurrentDictionary<string, ISkill> enemySkills = MyLoader.CreateSpellbook(new string[] { "CatClaw" });

        PlayerModel playerModel = new PlayerModel();
        Creature player = new Creature("Vaento", 1000, false, 300, 4, 1,
            playerSkills,playerModel.Text);

        LionWTF1Model lionWtf1Model = new LionWTF1Model();
        Creature enemy = new Creature("Simba und Nola", 1000, true, 666666, 42, 1.2, enemySkills,
            lionWtf1Model.Text);

        player.Target = enemy;
        enemy.Target = player;
        
        BuffTasker buffTasker = new BuffTasker(player, enemy);
        BuffWatcher buffWatcher = new BuffWatcher(player, enemy);
        SkillTasker skillTasker = new SkillTasker(player, enemy);
        UIUpdater uiUpdater = new UIUpdater(player,enemy);

        var playerSkillTask = player.ManualAttack();
        var enemySkillTask = enemy.AutoAttack();
        var buffTaskerTask = buffTasker.BuffExecuter();
        var buffWatcherTask = buffWatcher.BuffTakeAction();
        var skillTaskerTask = skillTasker.SkillExecuter();
        var uiUpdaterTask = uiUpdater.FightUI();
        

        Task.WaitAll(playerSkillTask, enemySkillTask, buffTaskerTask, buffWatcherTask, skillTaskerTask, uiUpdaterTask);

        return enemy.GetHealth() == 0;
    }
}
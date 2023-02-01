using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
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

public sealed class LevelOne
{
    public static bool Level_1_Fight()
    {
        Console.Clear();
        UIOperator.DestroyBuffer();


        ConcurrentDictionary<string, ISkill> playerSkills = MyLoader.CreateSpellbook(new string[] {"Block","Gruppen","Insult","Magic" });
        ConcurrentDictionary<string, ISkill> enemySkills = MyLoader.CreateSpellbook(new string[] { "Magic", "CatClaw" });

        PlayerModel playerModel = new PlayerModel();
        Creature player = new Creature("Vaento", 1000, false, 300, 4, 1,
            playerSkills,playerModel.Text);

        CattoModel cattomodel = new CattoModel();
        Creature enemy = new Creature("Fluffy Hanekawa", 1000, true, 500, 4, 1.2, enemySkills,
            cattomodel.Text);

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

    public static void Level_1_Cinematic_1()
    {
        UIOperator.DestroyBuffer();
        Console.Clear();
        var mygrid = new Grid();
        mygrid.Width = 180; 
        CatModel catModel=new CatModel();
        PlayerModel player=new PlayerModel();

        mygrid.AddColumn();
        mygrid.AddColumn();
        mygrid.AddColumn();
        AnsiConsole.Live(mygrid).Start(UI =>
        {
            UI.Refresh();
            mygrid.AddRow(new Markup(catModel.Text).LeftAligned(),new Markup(" ").Centered(),new Markup(player.Text).RightAligned());
            UI.Refresh();
            Thread.Sleep(2500);
            mygrid.AddRow(new Text(""), new Markup("[red]This da evilest da fluffiest catgirl whose hp is 500![/]").Centered(), new Text(""));
            UI.Refresh();
            Thread.Sleep(2300);
            mygrid.AddRow(new Text(""), new Markup("[red]But me and my gruppies will take it on![/]").Centered(), new Text(""));
            UI.Refresh();
            Thread.Sleep(2280);
            mygrid.AddRow(new Text(""), new Markup("[red]Guard yourself you cute monster![/]").Centered(), new Text(""));
            UI.Refresh();
            Thread.Sleep(2100);
            mygrid.AddRow(new Text(""), new Markup("[red]CHAAAAAAAAAAAAAAAAAAAAAARGE!!![/]").Centered(), new Text(""));
            UI.Refresh();
            Thread.Sleep(1800);
        });
        
        
    }


    public static void Level_1_Win()
    {
        Console.Clear();
        Grid mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 100;
        DedketModel dedket = new DedketModel();
        string color = "[darkorange3_1]";
        byte[] crypt = {0x79,0x6F,0x75,0x27,0x76,0x65,0x20,0x6A,0x75,0x73,0x74,0x20,0x61,0x63,0x71,0x75,0x69,0x72,0x65,0x64,0x20,0x61,0x20,0x6E,0x65,0x77,0x20,0x73,0x65,0x78,0x20,0x6F,0x62,0x6A,0x65,0x63,0x74,0x21 };
        AnsiConsole.Live(mygrid).Start(ui =>
        {
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color,"Congratulations!")));
            ui.Refresh();
            Thread.Sleep(2000);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color,"you killed a cat...")));
            ui.Refresh();
            Thread.Sleep(4000);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color,"oh, by the way")));
            ui.Refresh();
            Thread.Sleep(1000);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color,Encoding.ASCII.GetString(crypt))));
            ui.Refresh();
            Thread.Sleep(3000);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color,"the dead cat!")));
            ui.Refresh();
            Thread.Sleep(400);
        });
        AnsiConsole.Write(new Markup(UIOperator.ColoredStringBuilder(color,dedket.Text)));
        UIOperator.DestroyBuffer();
        Console.ReadKey(true);
    }

    public static void Level_1_Lose()
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
                "quit","try", "bruh a cat killed me"
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
                MyGlobals.MySwitch = LevelAdjustingSwitch.LevelOne;
                break;
            }
            case "bruh a cat killed me":
            {
                MyGlobals.MySwitch = LevelAdjustingSwitch.MainMenu;
                break;
            }
        }

    }

    public static void Level_1_Cinematic_2()
    {
        string colorMiss= "[red1]";
        string color0 = "[indianred1]";
        string color1 = "[deeppink2]";
        string color2 = "[deeppink1]";
        string color3 = "[deeppink1_1]";
        string color4 = "[magenta2_1]";
        string color5 = "[magenta1]";
        string color6 = "[hotpink]";
        string color7 = "[indianred1_1]";

        Grid mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 100;
        mygrid.Centered();
        Console.Clear();
        string space65 = "                                                                 0;";
        AnsiConsole.Live(mygrid).Start(ui =>
        {
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss,"We succeded! Now we shall face a greateest threat of their family!")));
            ui.Refresh();
            Thread.Sleep(3000);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, $" .")));
            ui.Refresh();
            Thread.Sleep(700);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, " .")));
            ui.Refresh();
            Thread.Sleep(700);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, " .")));
            ui.Refresh();
            Thread.Sleep(700);
            mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss,"Lions!")));
            ui.Refresh();
            Thread.Sleep(3000);
        });
        
        Console.Clear();

        LandWaterfallModel waterfall = new LandWaterfallModel();

        mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 100;
        mygrid.LeftAligned();
        string waterfallColor = "[turquoise4]";
        byte[] text0 = {0x2D,0x43,0x61,0x6C,0x6C,0x20,0x6D,0x65,0x20,0x4D,0x69,0x73,0x74,0x72,0x65,0x73,0x73,0x20,0x79,0x6F,0x75,0x20,0x6C,0x69,0x74,0x74,0x6C,0x65,0x20,0x62,0x69,0x74,0x63,0x68,0x21 };
        byte[] text1 = { 0x2D,0x59,0x65,0x73,0x20,0x6D,0x69,0x73,0x74,0x72,0x65,0x73,0x73,0x2E};
        byte[] text2 = { 0x2D,0x53,0x68,0x65,0x20,0x64,0x6F,0x65,0x73,0x21,0x20,0x53,0x68,0x65,0x20,0x6E,0x65,0x65,0x64,0x73,0x20,0x70,0x75,0x6E,0x69,0x73,0x68,0x6D,0x65,0x6E,0x74,0x21};
        byte[] text3 =  { 0x2D,0x50,0x75,0x6E,0x69,0x73,0x68,0x20,0x68,0x65,0x72,0x21};
        byte[] text4 =  { 0x2D,0x53,0x70,0x61,0x6E,0x6B,0x20,0x68,0x65,0x72,0x21};
        byte[] text5 =  { 0x2D,0x49,0x27,0x6C,0x6C,0x20,0x62,0x72,0x69,0x6E,0x67,0x20,0x74,0x68,0x65,0x20,0x76,0x69,0x62,0x72,0x6F,0x21};
        byte[] text6 =  { 0x2D,0x4E,0x6F,0x6E,0x6F,0x6E,0x6F,0x20,0x74,0x69,0x65,0x20,0x68,0x65,0x72,0x2C,0x20,0x61,0x6E,0x64,0x20,0x73,0x68,0x65,0x20,0x68,0x61,0x73,0x20,0x74,0x6F,0x20,0x77,0x61,0x74,0x63,0x68,0x21};
        byte[] text7 =  { 0x2D,0x59,0x65,0x73,0x2C,0x20,0x79,0x65,0x73,0x2C,0x20,0x79,0x65,0x73,0x21,0x20,0x41,0x6E,0x64,0x20,0x73,0x68,0x65,0x20,0x63,0x61,0x6E,0x27,0x74,0x20,0x63,0x75,0x6D,0x21};
        byte[] text8 =  { 0x2D,0x53,0x68,0x65,0x20,0x68,0x61,0x73,0x20,0x73,0x75,0x63,0x68,0x20,0x61,0x20,0x62,0x69,0x67,0x20,0x6D,0x6F,0x75,0x74,0x68,0x2C,0x20,0x67,0x61,0x67,0x20,0x69,0x74,0x21};
        byte[] text9 =  { 0x2D,0x54,0x68,0x65,0x6E,0x20,0x69,0x74,0x73,0x20,0x64,0x65,0x63,0x69,0x64,0x65,0x64,0x20,0x2D,0x73,0x61,0x69,0x64,0x20,0x74,0x68,0x65,0x20,0x6D,0x69,0x73,0x74,0x72,0x65,0x73,0x73,0x2D,0x21};
        byte[] text10 = { 0x54,0x68,0x65,0x6E,0x20,0x74,0x68,0x65,0x79,0x20,0x68,0x61,0x64,0x20,0x61,0x20,0x68,0x65,0x61,0x6C,0x74,0x68,0x79,0x20,0x67,0x72,0x75,0x70,0x70,0x65,0x6E,0x73,0x65,0x78,0x20,0x61,0x6E,0x64,0x20,0x68,0x65,0x61,0x6C,0x65,0x64,0x20,0x74,0x68,0x65,0x69,0x72,0x20,0x77,0x6F,0x75,0x6E,0x64,0x73,0x20,0x75,0x70};
        
        

        AnsiConsole.Live(mygrid).Start(ui =>
                {
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(waterfallColor,$"{waterfall.Text}")));
                    ui.Refresh();
                    Thread.Sleep(3000);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color0,"-Boss,")));
                    ui.Refresh();
                    Thread.Sleep(2800);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, Encoding.ASCII.GetString(text0))));
                    ui.Refresh();
                    Thread.Sleep(2300);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color0, Encoding.ASCII.GetString(text1))));
                    ui.Refresh();
                    Thread.Sleep(1550);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, "-So what is it? Tell me.")));
                    ui.Refresh();
                    Thread.Sleep(2100);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color0,"-I've been thinking.")));
                    ui.Refresh();
                    Thread.Sleep(1700);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color0,"-Is it a good idea to fight with lions?")));
                    ui.Refresh();
                    Thread.Sleep(2350);
                    mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder(color0,"-I mean we could barely deal with a cat...")));
                    ui.Refresh();
                    Thread.Sleep(4000);
                    
                });
        
        Console.Clear();

        MistressModel mistress=new MistressModel();
        
        

        StringBuilder sb = new StringBuilder();
        sb.Append("\n");
        sb.Append("\n");
        sb.Append("\n");
        sb.Append(UIOperator.ColoredStringBuilder(colorMiss, "-You dare deny me?"));
        sb.Append("\n");
        Table endCinematic = new Table();
        endCinematic.AddColumn(" ");
        endCinematic.AddColumn(" ");
        endCinematic.HideHeaders();
        endCinematic.Border(TableBorder.None);
        endCinematic.AddRow(new Markup(UIOperator.ColoredStringBuilder(colorMiss, mistress.Text))
            , new Markup(UIOperator.ColoredStringBuilder(colorMiss, sb.ToString())));
        AnsiConsole.Live(endCinematic).Start(ui =>
        {
            ui.Refresh();
            Thread.Sleep(3000);
            sb.Append(UIOperator.ColoredStringBuilder(color1, Encoding.ASCII.GetString(text2)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(300);

            sb.Append(UIOperator.ColoredStringBuilder(color2, Encoding.ASCII.GetString(text3)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(200);

            sb.Append(UIOperator.ColoredStringBuilder(color3, Encoding.ASCII.GetString(text4)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(186);

            sb.Append(UIOperator.ColoredStringBuilder(color4, Encoding.ASCII.GetString(text5)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(187);

            sb.Append(UIOperator.ColoredStringBuilder(color5, Encoding.ASCII.GetString(text6)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(256);

            sb.Append(UIOperator.ColoredStringBuilder(color6, Encoding.ASCII.GetString(text7)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(314);

            sb.Append(UIOperator.ColoredStringBuilder(color7, Encoding.ASCII.GetString(text8)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(3000);

            sb.Append(UIOperator.ColoredStringBuilder(colorMiss, Encoding.ASCII.GetString(text9)));
            sb.Append("\n");
            endCinematic.UpdateCell(0, 1, new Markup(sb.ToString()));
            ui.Refresh();
            Thread.Sleep(3000);
        });

        mygrid = new Grid();
        mygrid.AddColumn();
        mygrid.Width = 120;
        mygrid.Centered();
        mygrid.AddRow(new Markup(UIOperator.ColoredStringBuilder("[salmon1]", Encoding.ASCII.GetString(text10))));
        AnsiConsole.Write(mygrid);
        Thread.Sleep(3000);
        UIOperator.DestroyBuffer();


        SaveFile saveFile = new SaveFile();
        saveFile.UnlockedLevels = LevelUnlock.Level2;
        
        if (saveFile.Save())
        {
            AnsiConsole.Write(new Markup(UIOperator.ColoredStringBuilder("[salmon1]", "You have unlocked Level 2 in the chapter select! Moving forward, when you beat a level, you unlock it aswell in chapter select too")));
        }
        
        
        Console.ReadKey();
        MyGlobals.MySwitch = LevelAdjustingSwitch.MainMenu;

    }

}
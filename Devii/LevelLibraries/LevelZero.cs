using System.Text;
using Devii.Globals;
using Devii.Containers;
using Devii.Enums;
using Devii.Handlers;
using Devii.Skills;
using Spectre.Console;


namespace Devii.LevelLibraries;

public sealed class LevelZero
{
    
    public static void Level_0_MainMenu(MainMenuOptionsContainer optionsContainer)
    {
        UIOperator.DestroyBuffer();
        Console.Clear();
        string choiceColor = "[mediumvioletred]";
        string titleColor = "[salmon1]";
        string endTag = "[/]";

        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(UIOperator.ColoredStringBuilder(titleColor,"Welcome to the game of Deviants!",endTag))
                .PageSize(10)
                .HighlightStyle(new Style().Foreground(Color.Orange1))
                .MoreChoicesText("[green](this line of code is not even shown)[/]") //this line of code doesnt even do anything
                .AddChoices(new[]
                {
                    UIOperator.ColoredStringBuilder(choiceColor,"Start New Game",endTag), UIOperator.ColoredStringBuilder(choiceColor,"Chapter Select",endTag),
                    UIOperator.ColoredStringBuilder(choiceColor,"Library",endTag), UIOperator.ColoredStringBuilder(choiceColor,"Exit",endTag)
                }));

        switch (UIOperator.ColoredStringDemolisher(choiceColor,selected,endTag))
        {
            case "Start New Game":
                MyGlobals.MySwitch = LevelAdjustingSwitch.LevelOne;
                optionsContainer.SelectedOption = MainMenuOptions.Level1;
                break;
            case "Chapter Select":
                optionsContainer.SelectedOption = MainMenuOptions.ChapterSelect;
                break;
            case "Library":
                optionsContainer.SelectedOption = MainMenuOptions.Library;
                break;
            case "Exit":
                MyGlobals.MySwitch = LevelAdjustingSwitch.Exit;
                optionsContainer.SelectedOption = MainMenuOptions.Exit;
                break;
            default:
                throw new Exception("Unexpected switch case in Level_0_MainMenu");
        }

        
    }

    public static void Level_0_library(MainMenuOptionsContainer optionsContainer)
    {
        Tree NewView(ISkill skill)
        {
            string rootColor = "[darkorange]";
            string childColor = "[orangered1]";
            Tree view = new Tree(UIOperator.ColoredStringBuilder(rootColor,skill.Title));
            
            var isHarmful = view.AddNode(new Markup(UIOperator.ColoredStringBuilder(rootColor, "Harmful")));
            var isHarmfulChild = isHarmful.AddNode(new Markup(UIOperator.ColoredStringBuilder(childColor, skill.IsHarmful.ToString())));

            var value = view.AddNode(new Markup(UIOperator.ColoredStringBuilder(rootColor, "Value")));
            //im putting a note here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/null-coalescing-operator , https://stackoverflow.com/questions/69730874/c-sharp-null-conditional-in-ternary-condition
            var valueChild= value.AddNode(new Markup(UIOperator.ColoredStringBuilder(childColor, skill.Value?.ToString() ?? "NULL")));
            
            var effectiveRate =view.AddNode(new Markup(UIOperator.ColoredStringBuilder(rootColor, "Effectiveness")));
            var effectiveRateChild = effectiveRate.AddNode(new Markup(UIOperator.ColoredStringBuilder(childColor, $"{skill.EffectiveRate * 100}%")));
            
            var description =view.AddNode(new Markup(UIOperator.ColoredStringBuilder(rootColor, "Description")));
            var descriptionChild = description.AddNode(new Markup(UIOperator.ColoredStringBuilder(childColor, skill.Description)));
            
            return view;
        }

        int KeyHandler(ConsoleKeyInfo input,ref int index, int arrayLength)
        {
            if (input.Key is ConsoleKey.RightArrow or ConsoleKey.D)
            {
                index = (index + 1) % arrayLength;
            }

            if (input.Key is ConsoleKey.A or ConsoleKey.LeftArrow)
            {
                if (index==0)
                {
                    index = arrayLength - 1;
                }
                else
                {
                    index--;
                }
            }

            return index;
        }

        int outerIndex = 0;
        SkillLoader skills = new SkillLoader();
        var skillArray = skills.Skills.Values.ToArray();
        int length = skillArray.Length;
        Console.Clear();
        Tree skillView = NewView(skillArray[outerIndex]);
        ConsoleKeyInfo keyInfo; 
        Table wrapper = new Table();
        wrapper.AddColumn(" ");
        wrapper.AddRow(skillView);
        wrapper.NoBorder();
        wrapper.NoSafeBorder();
        AnsiConsole.Live(wrapper).Start(ui =>
        {
            do
            {
                ui.UpdateTarget(wrapper);
                ui.Refresh();
                keyInfo = Console.ReadKey(true);
                if (keyInfo.Key is ConsoleKey.A or  ConsoleKey.D or ConsoleKey.RightArrow or ConsoleKey.LeftArrow)
                {
                    
                    wrapper.UpdateCell(0,0,skillView = NewView(skillArray[KeyHandler(keyInfo, ref outerIndex, length)]));
                }
            } while (keyInfo.Key != ConsoleKey.Backspace);
        });
        optionsContainer.SelectedOption = MainMenuOptions.MainMenu;
    }
    
    public static void Level_0_chapter_select(MainMenuOptionsContainer optionsContainer)
    {
        Console.Clear();
        string choiceColor = "[mediumvioletred]";
        string titleColor = "[salmon1]";
        string endTag = "[/]";

        

        string[] Builder()
        {
            SaveFile saveFile = new SaveFile();
            saveFile.Load();

            List<string> tmp = new List<string>();
            
            tmp.Add(UIOperator.ColoredStringBuilder(choiceColor, "back"));
            for (int i = 1; i <= (int)saveFile.UnlockedLevels; i++)
            {
                tmp.Add(UIOperator.ColoredStringBuilder(choiceColor, $"Level {i}", endTag));
            }
            tmp.Add(UIOperator.ColoredStringBuilder(choiceColor,"Exit"));
            return tmp.ToArray();
        }



        var selected = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title(UIOperator.ColoredStringBuilder(titleColor, "Welcome to the game of Deviants!", endTag))
                .PageSize(5)
                .HighlightStyle(new Style().Foreground(Color.Orange1))
                .MoreChoicesText(
                    "[green](Move up and down to reveal more fruits)[/]") 
                .AddChoices(Builder()));


        switch (UIOperator.ColoredStringDemolisher(choiceColor,selected,endTag))
        {
            case "back":
                optionsContainer.SelectedOption = MainMenuOptions.MainMenu;
                break;
            case "Level 1":
                optionsContainer.SelectedOption = MainMenuOptions.Level1;
                MyGlobals.MySwitch = LevelAdjustingSwitch.LevelOne;
                break;
            case "Level 2":
                optionsContainer.SelectedOption = MainMenuOptions.Level2;
                MyGlobals.MySwitch = LevelAdjustingSwitch.LevelTwo;
                break;
            case "Level 3":
                optionsContainer.SelectedOption = MainMenuOptions.MainMenu;
                break;
            case "Exit":
                MyGlobals.MySwitch = LevelAdjustingSwitch.Exit;
                optionsContainer.SelectedOption = MainMenuOptions.Exit;
                break;
            default:
                throw new Exception("Unexpected switch case at chapter select");
        }
    }
}
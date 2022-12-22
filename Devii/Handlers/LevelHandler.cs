using System.Runtime.CompilerServices;
using Devii.Containers;
using Devii.Enums;
using Devii.LevelLibraries;
using Spectre.Console;

namespace Devii.Handlers;

public sealed class LevelHandler
{
    public static void Level_1()
    {
        //cinematic of pre
        LevelOne.Level_1_Cinematic_1();
        //fight
        if (LevelOne.Level_1_Fight())
        {
            LevelOne.Level_1_Win();
            //post cinematic
            LevelOne.Level_1_Cinematic_2();
        }
        else
        {
            LevelOne.Level_1_Lose();
        }
        
    }
    
    
    public static void Level_0()
    {
        MainMenuOptionsContainer localSwitch = new MainMenuOptionsContainer();
        while (true)
        {
            switch ( localSwitch.SelectedOption)
            {
                case MainMenuOptions.MainMenu:
                {
                    LevelZero.Level_0_MainMenu(localSwitch);
                    break;
                }
                case MainMenuOptions.StartNewGame:
                {
                    return;
                }
                case MainMenuOptions.Library:
                {
                    LevelZero.Level_0_library(localSwitch);
                    break; 
                }
                case MainMenuOptions.Level1:
                {
                    return;
                }
                case MainMenuOptions.Level2:
                {
                    throw new NotImplementedException("level 2 is not yet implemented");
                    return;
                }
                case MainMenuOptions.ChapterSelect:
                {
                    LevelZero.Level_0_chapter_select(localSwitch);
                    break;
                }
                case MainMenuOptions.Exit:
                {
                    return; //end
                }
                default:
                {
                    throw new Exception($"Level handler has unexpected case : {localSwitch}");
                }
            }
        }
    }

    
    
    
}
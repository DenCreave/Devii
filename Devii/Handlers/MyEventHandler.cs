using Devii.Globals;
using Devii.Enums;

namespace Devii.Handlers;

public sealed class MyEventHandler
{
    public static void HandleMe() 
    {
        switch (MyGlobals.MySwitch)
        {
            case LevelAdjustingSwitch.MainMenu:
            {
                LevelHandler.Level_0();
                break;
            }
            case LevelAdjustingSwitch.LevelOne:
            {
                LevelHandler.Level_1();
                break;
            }
            /*case 2:
            {
                break;
            }*/
        }
    }
}
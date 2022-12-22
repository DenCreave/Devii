using System.Collections.Concurrent;
using Devii.Globals;
using Devii.Enums;
using Devii.Handlers;
using Spectre.Console;
using Spectre.Console.Rendering;


namespace Devii;

public sealed class Game
{
    public static async Task GameOn()
    {
        MyLoader.FirstLoad();

        await Task.Run(() =>
        {
            while (MyGlobals.MySwitch!= LevelAdjustingSwitch.Exit)
            {
                MyEventHandler.HandleMe();
            }
        });
        Console.WriteLine("goodbye, thank you for playing");
    }
}
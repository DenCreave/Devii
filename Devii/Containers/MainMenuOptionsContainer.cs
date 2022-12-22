using Devii.Enums;

namespace Devii.Containers;

public sealed class MainMenuOptionsContainer
{
    public MainMenuOptions SelectedOption { get; set; }

    public MainMenuOptionsContainer()
    {
        SelectedOption = MainMenuOptions.MainMenu;
    }
}
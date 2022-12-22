using System.Text;
using Devii.Exceptions;
using Devii.Handlers;
using Spectre.Console;

namespace Devii.Creatures;


/// <summary>
/// a log for a creature, that contains dmg taken and heal.
/// </summary>
public sealed class CreatureLog
{
    string TitleColor => "[salmon1]";
    string TextColor => "[grey]";
    string DamageColor => "[maroon]";
    string HealColor => "[green]";
    string EndTag => "[/]";
    public string[] PlaceHolders { get; set; }

    public CreatureLog(int howManyRows=5)
    {
        PlaceHolders = new string[howManyRows];
    }

    
    
    //its invoked when a creature takes damage or healing
    public void LogAction(string name, int value, bool isHarmful, bool successful=true, string reason=" defaultReason")
    {
        StringBuilder retvalBuilder = new StringBuilder();
        retvalBuilder.Append(UIOperator.ColoredStringBuilder(TextColor, name, EndTag));
        if (successful)
        {
            if (isHarmful)
            {
                retvalBuilder.Append(UIOperator.ColoredStringBuilder(TextColor, " taken ", EndTag));
                retvalBuilder.Append(UIOperator.ColoredStringBuilder(DamageColor, value.ToString(), EndTag));
                retvalBuilder.Append(UIOperator.ColoredStringBuilder(TextColor, " damage", EndTag));

            }
            else
            {
                retvalBuilder.Append(UIOperator.ColoredStringBuilder(TextColor, " healed for ", EndTag));
                retvalBuilder.Append(UIOperator.ColoredStringBuilder(HealColor, value.ToString(), EndTag));
            }
        }
        else
        {
            if (reason.Equals(" defaultReason"))
            {
                throw new LogActionReasonMissing("Reason for not taking damage was not given, it's still the default reason");
            }
            retvalBuilder.Append(UIOperator.ColoredStringBuilder(TextColor, reason, EndTag));
        }
        PlaceHolderHandler(retvalBuilder.ToString());
    }

    private void PlaceHolderHandler(string value)
    {
        string[] tmp = new string[PlaceHolders.Length];
        tmp[0] = value;
        for (int i = 1; i < PlaceHolders.Length; ++i)
        {
            tmp[i] = PlaceHolders[i - 1];
        }
        PlaceHolders = tmp;
    }
    
}
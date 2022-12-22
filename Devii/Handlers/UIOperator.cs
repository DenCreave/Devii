using System.Text;

namespace Devii.Handlers;

public sealed class UIOperator
{
    public static string IntoALineWithNewLine(string [] input)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var line in input)
        {
            sb.Append(line);
            sb.Append("\n");
        }
        sb.Length--;
        return sb.ToString();
    }
    
    public static string IntoALine(string [] input)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var line in input)
        {
            sb.Append(line);
        }
        sb.Length--;
        return sb.ToString();
    }
    
    public static string ColoredStringBuilder(string color, string choice, string endTag="[/]")
    {
        StringBuilder concatWithColor = new StringBuilder();
        concatWithColor.Append(color);
        concatWithColor.Append(choice);
        concatWithColor.Append(endTag);
        return concatWithColor.ToString();
    }

    public static string ColoredStringDemolisher(string color,string destroyMe, string endTag="[/]")
    {
        StringBuilder keywordTrimmer = new StringBuilder();
        keywordTrimmer.Append(destroyMe);
        keywordTrimmer.Replace(color, "");
        keywordTrimmer.Replace(endTag, "");
        return keywordTrimmer.ToString();
    }
    
    
    /// <summary>
    /// Untill a Console.ReadKey is done, whatever key is pressed is kept in the buffer which can
    /// stack up in an asleep thread. this method, reads them all and basically destroys them
    /// </summary>
    public static void DestroyBuffer()
    {
        while (Console.KeyAvailable)
        {
            Console.ReadKey(true);
        }
    }
}
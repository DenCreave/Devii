namespace Devii.Exceptions;

public sealed class DealDmgError : Exception
{
    public DealDmgError(string message)
        : base("No skills found in the skill lists with name: " + message)
    {
    }
}
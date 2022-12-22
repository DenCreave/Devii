namespace Devii.Exceptions;

public sealed class LogActionReasonMissing : Exception
{
    public LogActionReasonMissing(string message)
        :base(message){}
}
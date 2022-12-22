namespace Devii.Exceptions;

public sealed class BuffTakeActionError: Exception
{
    public BuffTakeActionError(string message)
        :base("Wrong TakeAction method used in class: "+message){}
}
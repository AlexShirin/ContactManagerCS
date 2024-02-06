namespace ContactManagerCS.Common.Exceptions;

public class ContactException : Exception
{
    public ContactException(string? message)
        : base(message)
    {
    }
}

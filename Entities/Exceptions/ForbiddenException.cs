namespace Entities.Exceptions;

public class ForbiddenException : Exception
{
    protected ForbiddenException(string message)
        : base(message)
    {
    }
}
namespace NexCart.Application.Common.Exceptions;

public class UnauthorizedException : Exception
{
    public UnauthorizedException()
        : base("No está autenticado")
    {
    }

    public UnauthorizedException(string message)
        : base(message)
    {
    }
}
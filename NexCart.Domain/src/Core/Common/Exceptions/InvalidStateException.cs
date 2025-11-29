namespace NexCart.Domain.Common.Exceptions;

public sealed class InvalidStateException : DomainException
{
    public InvalidStateException(string message)
        : base(message)
    {
    }

    public InvalidStateException(string entityName, string currentState, string operation)
        : base($"Entity '{entityName}' in state '{currentState}' cannot perform operation '{operation}'.")
    {
    }
}
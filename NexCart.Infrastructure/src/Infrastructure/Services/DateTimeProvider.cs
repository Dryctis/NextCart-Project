using NexCart.Application.Common.Interfaces;

namespace NexCart.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;

    public DateTime Now => DateTime.Now;

    public DateTime Today => DateTime.Today;
}
namespace NexCart.Domain.Common.ValueObjects;

public sealed class DateRange : ValueObject
{
    public DateTime Start { get; }
    public DateTime End { get; }

    private DateRange(DateTime start, DateTime end)
    {
        Start = start;
        End = end;
    }

    public static DateRange Create(DateTime start, DateTime end)
    {
        if (start > end)
            throw new ArgumentException("Start date must be before or equal to end date");

        return new DateRange(start.Date, end.Date);
    }

   
    public static DateRange SingleDay(DateTime date)
    {
        return new DateRange(date.Date, date.Date);
    }


    public int DaysCount => (End - Start).Days + 1;

    public bool Contains(DateTime date)
    {
        var dateOnly = date.Date;
        return dateOnly >= Start && dateOnly <= End;
    }

   
    public bool OverlapsWith(DateRange other)
    {
        return Start <= other.End && End >= other.Start;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public override string ToString() =>
        Start == End
            ? Start.ToString("yyyy-MM-dd")
            : $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
}
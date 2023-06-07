namespace MegaMercado.Application.Common;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTimeOffset NowOffset { get; }
}
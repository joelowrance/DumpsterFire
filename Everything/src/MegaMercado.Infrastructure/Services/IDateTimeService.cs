using MegaMercado.Application.Common;

namespace MegaMercado.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTimeOffset OffsetNow => DateTimeOffset.Now;
}
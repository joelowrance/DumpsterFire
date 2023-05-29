namespace MegaMercado.Domain.PaymentGeneration;

public abstract class PaymentDateCalculator
{
    public abstract DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousPaymentDate);

    protected DateTimeOffset LastDayOfMonthFor(DateTimeOffset forDate)
    {
        var currentDay1 = new DateTime(forDate.Year, forDate.Month, 1);
        var nextMonth = currentDay1.AddMonths(1);
        var lastOfThisMonth = nextMonth.AddDays(-1);
        return lastOfThisMonth; 
    }
}
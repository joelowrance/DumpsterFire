namespace MegaMercado.Domain.PaymentGeneration;

public class EndOfMonthCalculator : PaymentDateCalculator
{
    public override DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousPaymentDate)
    {
        return LastDayOfMonthFor(previousPaymentDate?.AddMonths(1) ?? DateTimeOffset.Now);
    }
}

public class PlusThirtyDaysCalculator : PaymentDateCalculator
{
    public override DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousPaymentDate)
    {
        return previousPaymentDate?.AddDays(30) ?? DateTimeOffset.Now.AddDays(30);
    }
}
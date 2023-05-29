namespace MegaMercado.Domain.PaymentGeneration;

public class PaymentTerm
{
    public int Length { get; set; }
    public decimal? TermAmount { get; set; }
    public decimal? TermPercentage { get; set; }

    public decimal CalculateAmount(decimal amount)
    {
        if (TermAmount != null) return TermAmount.Value;

        if (TermPercentage is null or < 0 or > 100)
        {
            throw new ArgumentException("Percentage must be an integer between 1 and 100", nameof(TermPercentage));
        }

        return Math.Round((TermPercentage.Value / 100m) * amount, 2);
    }
}
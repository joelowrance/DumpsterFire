namespace MegaMercado.Domain.PaymentGeneration;

public class Payment
{
    public DateTimeOffset Date { get; set; }
    public decimal Amount { get; set; }
    public decimal FeeAmount { get; set; } = default;
    public decimal? PaymentAddressId { get; set; }
    public string PaymentType { get; set; } = string.Empty;

    public override string ToString()
    {
        return
            $"{Date:yyyy-MM-dd} - ${Amount} - ${FeeAmount} - AddressId: {PaymentAddressId}, PaymentType {PaymentType}";
    }
}
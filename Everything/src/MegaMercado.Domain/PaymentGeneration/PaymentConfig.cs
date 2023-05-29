namespace MegaMercado.Domain.PaymentGeneration;

public class PaymentConfig
{
    public int Id { get; set; }
    public long? FirstPaymentAddressId { get; set; }
    public long? SubsequentPaymentAddressId { get; set; }
    public long? FirstPaymentId { get; set; }
    public long? SubsequentPaymentId { get; set; }
    public string FirstPaymentType { get; set; } = string.Empty;
    public string SubsequentPaymentType { get; set; } = string.Empty;
}
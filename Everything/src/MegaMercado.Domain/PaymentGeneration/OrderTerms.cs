namespace MegaMercado.Domain.PaymentGeneration;

public class OrderTerms
{
    public int Id { get; set; }
    public int Length { get; set; }
    public decimal Rate { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsPreCharge { get; set; }
    public List<PaymentTerm> PaymentTerms { get; set; } = new();

}
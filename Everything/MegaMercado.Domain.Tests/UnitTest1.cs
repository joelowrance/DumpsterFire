using MegaMercado.Domain.PaymentGeneration;

namespace MegaMercado.Domain.Tests;

public class PaymentGeneratorTest
{
    [Fact]
    public void TheEasyCase()
    {
        var terms = new OrderTerms
        {

            PaymentTerms = new List<PaymentTerm>
            {
                new PaymentTerm { Length = 5, TermPercentage = 10 },
                new PaymentTerm { Length = 2, TermPercentage = 25 }
            }
        };

        var payments = GenerateForTerms(terms, 1023.23m);
        
        Assert.Equal(1023.23m, payments.Sum(x => x.Amount));
    }
    
    [Fact]
    public void WhenTheTermsDontCoverAllPays()
    {
        var terms = new OrderTerms
        {
            Length = 10,
            PaymentTerms = new List<PaymentTerm>
            {
                new PaymentTerm { Length = 5, TermAmount = 50 },
            }
        };

        var payments = GenerateForTerms(terms, 1023.23m);
        
        Assert.Equal(1023.23m, payments.Sum(x => x.Amount));
    }

    private List<Payment> GenerateForTerms(OrderTerms terms, decimal totalAmount)
    {
        var pg = new PaymentGenerator(
            new Microsoft.Extensions.Logging.Abstractions.NullLogger<PaymentGenerator>());

        return pg.GeneratePayments(
            terms,
            new PaymentConfig
            {
                FirstPaymentId = 1, FirstPaymentAddressId = 1, FirstPaymentType = "XXX", SubsequentPaymentType = "XXX",
                SubsequentPaymentId = 12, SubsequentPaymentAddressId = 23
            },
            totalAmount
        );
    }
}
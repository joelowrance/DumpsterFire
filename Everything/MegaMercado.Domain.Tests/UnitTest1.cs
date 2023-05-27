using MegaMercado.Domain.PaymentGeneration;

namespace MegaMercado.Domain.Tests;

public class PaymentGeneratorTest
{
    [Fact]
    public void CanGeneratePaymentsBasedOnRules()
    {

        var pg = new PaymentGenerator(
            new Microsoft.Extensions.Logging.Abstractions.NullLogger<PaymentGenerator>());

        var payments = pg.GeneratePayments(
            new OrderTerms{
            
                    PaymentTerms = new List<PaymentTerm>
                    {
                        new PaymentTerm { Length = 5, Percentage = 10 },
                        new PaymentTerm { Length = 2, Percentage = 25 }
                    }
            },
                
            
            new PaymentConfig
            {
                FirstPaymentId = 1, FirstPaymentAddressId = 1, FirstPaymentType = "XXX", SubsequentPaymentType = "XXX",
                SubsequentPaymentId = 12, SubsequentPaymentAddressId = 23
            },
            1023.23m
        );
        
        
        
        payments.ForEach(Console.WriteLine);
        
        Assert.Equal(1023.23m, payments.Sum(x => x.Amount));
        
        
    }
    
    
}
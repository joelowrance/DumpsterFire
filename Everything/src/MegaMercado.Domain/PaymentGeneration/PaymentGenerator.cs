using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;

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

public class PaymentGenerator
{
    private PaymentDateCalculator _firstPaymentStrategy = null!;
    private PaymentDateCalculator _subsequentPaymentStrategy = null!;

    private ILogger<PaymentGenerator> _logger;

    public PaymentGenerator(ILogger<PaymentGenerator> logger)
    {
        _logger = logger;
        _firstPaymentStrategy = new EndOfMonthCalculator();
        _subsequentPaymentStrategy = new EndOfMonthCalculator();
    }

    public List<Payment> GeneratePayments(OrderTerms term, PaymentConfig config, decimal orderTotal)
    {
        var payments = new List<Payment>();
        
        //if payments length is 1, bulk pay
        if (term.Length == 1)
        {
            payments.Add(new Payment
            {
                PaymentAddressId = config.FirstPaymentAddressId,
                PaymentType = config.FirstPaymentType,
                Amount = orderTotal,
                Date = _firstPaymentStrategy.DeterminePaymentDate(null),
                FeeAmount = GetFeeAmount()
            } );
        }
        else
        {
            var remainingAmount = orderTotal;

            while (remainingAmount != 0)
            {
                foreach (var paymentTerm in term.PaymentTerms)
                {
                    var paymentAmount = paymentTerm.CalculateAmount(orderTotal);
                    
                    for (int i = 0; i < paymentTerm.Length; i++)
                    {
                        var payment = new Payment
                        {
                            PaymentAddressId = payments.Count == 0
                                ? config.FirstPaymentAddressId
                                : config.SubsequentPaymentAddressId,
                            PaymentType = payments.Count == 0
                                ? config.FirstPaymentType
                                : config.SubsequentPaymentType,
                            Amount = paymentAmount,
                            FeeAmount = GetFeeAmount(),
                            Date = payments.Count == 0
                                ? _firstPaymentStrategy.DeterminePaymentDate(null)
                                : _subsequentPaymentStrategy.DeterminePaymentDate(payments[^1].Date)
                        };

                        payments.Add(payment);
                        remainingAmount = orderTotal - payments.Sum(x => x.Amount);

                        if (remainingAmount < 0)
                        {
                            break;
                        }
                    }
                    
                    if (remainingAmount < 0)
                    {
                        break;
                    }
                }
                //remaining amount is more than 0
                if (remainingAmount > 0)
                {
                    var lastPaymentAmount = payments.Last().Amount;

                    if (remainingAmount > lastPaymentAmount)
                    {
                        //even pays?
                        var evenPays = Math.Round(remainingAmount / (term.Length - payments.Count), 2);
                        while (payments.Count < term.Length)
                        {
                            var payment = new Payment
                            {
                                PaymentAddressId = config.SubsequentPaymentAddressId,
                                PaymentType = config.SubsequentPaymentType,
                                Amount = evenPays,
                                FeeAmount = GetFeeAmount(),
                                Date = _subsequentPaymentStrategy.DeterminePaymentDate(payments[^1].Date)
                            };
                            
                            payments.Add(payment);
                            remainingAmount -= evenPays;
                        }
                        
                        //remaining amount is less than zero
                        if (remainingAmount < 0)
                        {
                            payments.Last().Amount -= Math.Abs(remainingAmount);
                            remainingAmount = 0;
                        }
                        else if (remainingAmount > 0)
                        {
                            payments.Last().Amount += remainingAmount;
                            remainingAmount = 0;
                        }
                        
                    }
                    else
                    {
                        payments.Last().Amount += remainingAmount;
                        remainingAmount = 0;    
                    }
                }
                
                //remaining amount is less than zero
                if (remainingAmount < 0)
                {
                    payments.Last().Amount -= remainingAmount;
                    remainingAmount = 0;
                }
                
            }
        }

        //handle any leftover amounts (gets messy here.  I want to be able to drop a dll in the directory and just have it picked up).
        
        //and done
        return payments;
    }

    private decimal GetFeeAmount()
    {
        return 0;
        //throw new NotImplementedException();
    }


    private DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousDate)
    {
        return previousDate is null
            ? _firstPaymentStrategy.DeterminePaymentDate(previousDate)
            : _subsequentPaymentStrategy.DeterminePaymentDate(previousDate);
    } 
}

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

public class EndOfMonthCalculator : PaymentDateCalculator
{
    public override DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousPaymentDate)
    {
        return LastDayOfMonthFor(previousPaymentDate?.AddMonths(1) ?? DateTimeOffset.Now);
    }
}



 

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

public class OrderTerms
{
    public int Id { get; set; }
    public int Length { get; set; }
    public decimal Rate { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
    public bool IsPreCharge { get; set; }
    public List<PaymentTerm> PaymentTerms { get; set; }
    
}

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

using System.Runtime.InteropServices.JavaScript;
using Microsoft.Extensions.Logging;

namespace MegaMercado.Domain.PaymentGeneration;

public abstract class RemainingPaysCalculator
{
    public abstract List<Payment> CalculateRemainingPays(decimal remainingAmount, int paymentsCount);
}

public class EvenRemainingPaysCalculator : RemainingPaysCalculator
{
    public override List<Payment> CalculateRemainingPays(decimal remainingAmount, int paymentsCount)
    {
        throw new NotImplementedException();
    }
}

public class PaymentGenerator
{
    private readonly PaymentDateCalculator _firstPaymentDateStrategy;
    private readonly PaymentDateCalculator _subsequentPaymentDateStrategy;

    private ILogger<PaymentGenerator> _logger;

    public PaymentGenerator(ILogger<PaymentGenerator> logger)
    {
        _logger = logger;
        _firstPaymentDateStrategy = new EndOfMonthCalculator();
        _subsequentPaymentDateStrategy = new EndOfMonthCalculator();
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
                Date = _firstPaymentDateStrategy.DeterminePaymentDate(null),
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
                                ? _firstPaymentDateStrategy.DeterminePaymentDate(null)
                                : _subsequentPaymentDateStrategy.DeterminePaymentDate(payments[^1].Date)
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
                                Date = _subsequentPaymentDateStrategy.DeterminePaymentDate(payments[^1].Date)
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


    // private DateTimeOffset DeterminePaymentDate(DateTimeOffset? previousDate)
    // {
    //     return previousDate is null
    //         ? _firstPaymentStrategy.DeterminePaymentDate(previousDate)
    //         : _subsequentPaymentStrategy.DeterminePaymentDate(previousDate);
    // } 
}
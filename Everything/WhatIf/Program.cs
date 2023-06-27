// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;

Console.WriteLine("Hello, World!");

//var summary = BenchmarkRunner.Run<WhatHappens>();

 var wh = new WhatHappens();
 wh.Setup();
 wh.ByLoop();
 wh.ByDictionary();



[MemoryDiagnoser()]
public class WhatHappens
{
    private List<Hashids> toLookup = new List<Hashids>();
    List<Customer> customers = new List<Customer>();
    private Dictionary<Hashids, List<string>> customersById = new Dictionary<Hashids, List<string>>();

    [GlobalSetup]
    public void Setup()
    {
        var customerIds = Enumerable.Range(1, 10_000)
            .Select(x => new Bogus.Hashids())
            .ToList();

        foreach (var customerId in customerIds)
        {
            var customer = new Customer
            {
                Id = customerId,
                CustomerRecords = Enumerable
                    .Range(1, Random.Shared.Next(1, 10))
                    .Select(x => new Bogus.Faker().Company.CompanyName())
                    .ToList()
            };
            
            customers.Add(customer);
            customersById.Add(customer.Id, customer.CustomerRecords);
        }

        this.toLookup = customers.OrderBy(x => Guid.NewGuid())
            .Take(500)
            .Select(x => x.Id)
            .ToList();
    }

    [Benchmark()]
    public void ByLoop()
    {
        foreach (var id in toLookup)
        {
            var customer = customers.FirstOrDefault(x => x.Id == id);
        }
    }
    
    [Benchmark()]
    public void ByDictionary()
    {
        foreach (var hash in toLookup)
        {
            var customer = customersById[hash];
        }
    }
}


public class Customer
{
    public Hashids Id { get; set; }
    public List<string> CustomerRecords { get; set; } = new List<string>();
}
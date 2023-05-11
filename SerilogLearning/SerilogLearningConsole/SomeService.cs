using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging;

namespace SerilogLearningConsole;

public class Benchboy
{
    [GlobalSetup]
    public void Setup()
    {
        SomeKindOfBenchmark.Init();
    }

    //[Benchmark()]
    public void AgainstAList()
    {
        foreach (var key in SomeKindOfBenchmark.Keys)
        {
            var f = SomeKindOfBenchmark.Data.FirstOrDefault(x => x.Id == key);
        }
    }
    
    [Benchmark()]
    public void AgainstADictionary()
    {
        foreach (var key in SomeKindOfBenchmark.Keys)
        {
            var f =  SomeKindOfBenchmark.BetterData[key];
        }
    }
    
    
}

public class SomeKindOfBenchmark
{
    public static List<Container> Data;
    public static List<Guid> Keys;
    public static Dictionary<Guid, Container> BetterData;

    public static void Init()
    {
        Data =  new List<Container>();
        BetterData = new Dictionary<Guid, Container>();
        
        var ids = Enumerable.Range(1, 50000)
            .Select(x => Guid.NewGuid());

        foreach (var id in ids)
        {
            var count = Random.Shared.Next(0, 20);
            var people = Enumerable.Range(1, count)
                .Select(x => new Bogus.Person())
                .ToList();

            var container = new Container(id, people); 
            Data.Add(container);
            BetterData.Add(id, container);
        }

        Keys = new List<Guid>();
        Keys.AddRange(
            ids.OrderBy(x => Guid.NewGuid())
                .Take(5000));
    }
}

public record Container(Guid Id, List<Bogus.Person> People);

public class SomeService
{
    private readonly ILogger<SomeService> _logger;
    private readonly InMemoryContext _context;

    public SomeService(ILogger<SomeService> logger, InMemoryContext context)
    {
        _logger = logger;
        _context = context;
    }

    public List<Person> LoadAll()
    {
        return _context.Persons.ToList();
    }

    public void AddPerson(string name)
    {
        var person = new Person
        {
            Name = name
        };

        _context.Persons.Add(person);
        _context.SaveChanges();
    }

    public void DoDomething(int x)
    {
        _logger.LogInformation("Doing something with {x}", x);
    }
}
using Microsoft.Extensions.Logging;

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
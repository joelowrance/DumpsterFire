using MediatR;
using Microsoft.Extensions.Logging;

namespace Domain;

public record LoadPersonQuery(string Name) : IRequest<Person>;

public class LoadPersonQueryHandler : IRequestHandler<LoadPersonQuery, Person>
{
    private SomeService _someService;

    public LoadPersonQueryHandler(SomeService someService)
    {
        _someService = someService;
    }

    public Task<Person> Handle(LoadPersonQuery request, CancellationToken cancellationToken)
    {
        var p = _someService.LoadAll();
        return Task.FromResult(p);
    }
}

public class SomeService
{
    private readonly ILogger<SomeService> _logger;

    public SomeService(ILogger<SomeService> logger)
    {
        _logger = logger;
    }

    public Person LoadAll()
    {
        return new Person { Name = "Name", Id = Guid.NewGuid(), DateOfBirth = new DateOnly(2000, 1, 1) };
        
    }
}
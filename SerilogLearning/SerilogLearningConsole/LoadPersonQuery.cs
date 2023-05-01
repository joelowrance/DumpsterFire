using MediatR;
using Microsoft.EntityFrameworkCore;

namespace SerilogLearningConsole;

public record LoadPersonQuery(string Name) : IRequest<Person>;

public class LoadPersonQueryHandler : IRequestHandler<LoadPersonQuery, Person>
{
    private readonly InMemoryContext _dbContext;

    public LoadPersonQueryHandler(InMemoryContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Person> Handle(LoadPersonQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Persons.SingleAsync(x => x.Name.Equals(request.Name), cancellationToken);
    }
}
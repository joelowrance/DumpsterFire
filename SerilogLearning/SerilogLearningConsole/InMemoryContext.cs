using Microsoft.EntityFrameworkCore;

namespace SerilogLearningConsole;

public class InMemoryContext : DbContext
{
    public InMemoryContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Person> Persons { get; set; }
}
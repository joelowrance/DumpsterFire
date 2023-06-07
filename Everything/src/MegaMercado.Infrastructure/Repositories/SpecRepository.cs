using Ardalis.Specification.EntityFrameworkCore;
using MegaMercado.Domain.Entities;

namespace MegaMercado.Infrastructure.Repositories;

public class SpecRepository<T> : RepositoryBase<T> where T:BaseEntity
{
    private readonly AppDbContext _appDbContext;

    public SpecRepository(AppDbContext appDbContext) : base(appDbContext)
    {
        _appDbContext = appDbContext;
    }
}
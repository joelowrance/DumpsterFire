using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Application.Common;

public interface IAppDbContext
{
    DbSet<Product> Products { get; }
}
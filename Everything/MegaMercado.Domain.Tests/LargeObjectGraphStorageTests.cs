using MegaMercado.Domain.Entities;
using MegaMercado.Infrastructure;
using MegaMercado.Infrastructure.Persistence.FileSystem;
using Microsoft.EntityFrameworkCore;

namespace MegaMercado.Domain.Tests;

public class LargeObjectGraphStorageTests
{
    private readonly int _batchId;

    public LargeObjectGraphStorageTests()
    {
        _batchId = Random.Shared.Next(int.MinValue, int.MaxValue);
    }

    [Fact]
    public void CanSaveAsBlobInDb()
    {
        var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ThingsThatDontBelongInCode.ConnectionString)
            .Options);

        var products = db.Products.ToList();
        var dbBlobPersistence = new DbBlobPersistence(ThingsThatDontBelongInCode.ConnectionString);

        dbBlobPersistence.Serialize(products, _batchId);
    }

    [Fact]
    public void CanLoadFromBlobInDb()
    {
        var fileSystem = new DbBlobPersistence(ThingsThatDontBelongInCode.ConnectionString);
        var data = fileSystem.Deserialize<List<Product>>(_batchId);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void BinarySerializerStillWorksButIsABadIdea()
    {
        var db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ThingsThatDontBelongInCode.ConnectionString)
            .Options);

        var products = db.Products.ToList();
        var fileSystem = new FileSystemPersistence();

        fileSystem.Serialize(products, @"c:\temp\products.bin");
    }
}
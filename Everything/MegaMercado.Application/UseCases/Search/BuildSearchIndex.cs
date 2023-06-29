using MediatR;
using MegaMercado.Application.Common;
using MegaMercado.Application.UseCases.Products;
using MegaMercado.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Nest;

namespace MegaMercado.Application.UseCases.Search;

public record BuildApplicationIndexNotification : INotification;

public class BuildApplicationIndexNotificationHandler : MediatR.NotificationHandler<BuildApplicationIndexNotification>
{
    private readonly IAppDbContext _dbContext;

    public BuildApplicationIndexNotificationHandler(IAppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override void Handle(BuildApplicationIndexNotification notification)
    {
        var products = _dbContext.Products
            .Include(x => x.ProductCategories)
            .ThenInclude(x => x.Category)
            .Include(x => x.Brand)
            .ToList()
            .Select(p => p.ToProductDetailsModel())
            .ToList();

        //TODO: config file
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("products");

        var client = new ElasticClient(settings);

        // var response = client.Indices.Create("products", c => c
        //     .Map<Product>(m => m
        //         .AutoMap()
        //         .Properties(ps => ps
        //             .Text(s => s
        //                 .Name(n => n.Name)
        //                 .Analyzer("autocomplete")
        //             )
        //             .Text(s => s
        //                 .Name(n => n.Description)
        //                 .Analyzer("autocomplete")
        //             )
        //             .Text(s => s
        //                 .Name(n => n.Brand.Name)
        //                 .Analyzer("autocomplete")
        //             )
        //         )
        //     )
        // );

        foreach (var product in products)
        {
            var res = client.IndexDocument(product);
        }
        
        
    }
}

 
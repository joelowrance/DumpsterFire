using MediatR;
using MegaMercado.Application.Products.Dto;
using Nest;

namespace MegaMercado.Application.UseCases.Search;

public record SearchQuery(string Query) : MediatR.IRequest<List<ProductDetailsModel>>;

public class SearchQueryHandler : IRequestHandler<SearchQuery, List<ProductDetailsModel>>
{
    public async Task<List<ProductDetailsModel>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        //TODO: config file
        var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
            .DefaultIndex("products");

        var client = new ElasticClient(settings);
        
        //keeping this here so I dont have to search again
        
        // var data = await client.SearchAsync<ProductDetailsModel>(s => s
        //     .AllIndices()
        //     .From(0)
        //     .Size(100)
        //     .Query(q =>
        //
        //         q.Match(m => m
        //             .Field(p => p.Description)
        //             .Query(request.Query)
        //         )
        //     ), cancellationToken);

        var data = await client.SearchAsync<ProductDetailsModel>(s => s
            .From(0)
            .Size(100)
            .Query(q => q
                .MultiMatch(m => m
                    .Fields(f => f
                        .Field(p => p.Name)
                        .Field(p => p.Description)
                        .Field(p => p.BrandName)
                    ).Query(request.Query)
                )
            )
            .Explain(true), cancellationToken);

        return data.Documents.ToList();
    }
}
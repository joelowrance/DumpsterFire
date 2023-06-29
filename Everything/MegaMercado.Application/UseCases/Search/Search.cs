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

        var data = await client.SearchAsync<ProductDetailsModel>(s => s
            .AllIndices()
            .From(0)
            .Size(100)
            .Query(q =>

                q.Match(m => m
                    .Field(p => p.Description)
                    .Query(request.Query)
                )
            ), cancellationToken);

        return data.Documents.ToList();
    }
}
using Application.Products.EventHandlers.Dtos;
using MediatR;

namespace Application.Products.EventHandlers.Requests.GetProduct;

public class GetProductRequest : IRequest<ProductResponse>
{
    public GetProductRequest(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
using MediatR;

namespace Application.Products.EventHandlers.Requests.CreateProduct;

public class CreateProductRequest : IRequest<Guid?>
{
    public DateTime? Date { get; set; }
}
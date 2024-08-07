using Application.Common.Persistence;
using Domain.Entities;
using Mapster;
using MediatR;

namespace Application.Products.EventHandlers.Requests.CreateProduct;

public class CreateProductRequestHandler : IRequestHandler<CreateProductRequest, Guid?>
{
    private readonly IRepository<Product> _repository;

    public CreateProductRequestHandler(IRepository<Product> repository)
    {
        (_repository) = (repository);
    }

    public async Task<Guid?> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        var sale = request.Adapt<Product>();
        sale = sale.SetCode();
        await _repository.AddAsync(sale, cancellationToken);
        return sale.Id;
    }
}
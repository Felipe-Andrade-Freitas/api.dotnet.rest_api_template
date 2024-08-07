using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Shared.Exceptions;

namespace Application.Products.EventHandlers.Requests.UpdateProduct;

public class UpdateProductRequestHandler : IRequestHandler<UpdateProductRequest, Guid?>
{
    private IRepository<Product> _repository;

    public UpdateProductRequestHandler(IRepository<Product> repository)
    {
        _repository = repository;
    }

    public async Task<Guid?> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);

        _ = sale ?? throw new CustomException(ErrorsMessages.Product.NotFound);

        var updatedProduct = sale.UpdateStatus(request.Status);

        await _repository.UpdateAsync(updatedProduct, cancellationToken);

        return request.Id;
    }
}
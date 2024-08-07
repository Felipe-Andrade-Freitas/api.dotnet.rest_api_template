using System.Net;
using Application.Common.Persistence;
using Application.Products.EventHandlers.Dtos;
using Domain.Entities;
using Mapster;
using MediatR;
using Shared.Exceptions;

namespace Application.Products.EventHandlers.Requests.GetProduct;


public class GetProductRequestHandler : IRequestHandler<GetProductRequest, ProductResponse>
{
    private readonly IRepository<Product> _repository;

    public GetProductRequestHandler(IRepository<Product> repository) =>
        (_repository) = (repository);

    public async Task<ProductResponse> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        var sale = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return sale == null
            ? throw new CustomException(ErrorsMessages.Product.NotFound, statusCode: HttpStatusCode.NotFound)
            : sale.Adapt<ProductResponse>();
    }
}
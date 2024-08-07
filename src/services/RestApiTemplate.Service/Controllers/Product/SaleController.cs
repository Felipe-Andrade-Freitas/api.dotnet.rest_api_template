using Application.Common.Controllers;
using Application.Products.EventHandlers.Dtos;
using Application.Products.EventHandlers.Requests.CreateProduct;
using Application.Products.EventHandlers.Requests.GetProduct;
using Application.Products.EventHandlers.Requests.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ProductService.Controllers.Product;

public class ProductController : VersionNeutralApiController
{
    [HttpPost]
    //[OpenApiOperation("Criar uma nova venda.", "Realiza a criação de uma nova venda.")]
    [ProducesResponseType(typeof(BaseResult<Guid>), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.BadRequest)]
    public Task<ActionResult<BaseResult<Guid>>> CreateProduct(CreateProductRequest request) =>
        Result<CreateProductRequest, Guid>(request, result => Result(HttpStatusCode.Created, result));

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BaseResult<ProductResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.BadRequest)]
    //[OpenApiOperation("Obter uma venda por um identificador", "Realiza a busca de uma venda pelo identificador único do sistema.")]
    public Task<ActionResult<BaseResult<ProductResponse>>> GetProduct([FromRoute] Guid id) =>
        Result<GetProductRequest, ProductResponse>(new GetProductRequest(id));

    /// <summary>
    /// Atualizar a venda
    /// Realiza a atualização do status da venda
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(BaseResult<Guid>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.BadRequest)]
    [ProducesResponseType(typeof(BaseResult), (int)HttpStatusCode.UnprocessableEntity)]
    public Task<ActionResult<BaseResult<Guid>>> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductRequest request)
    {
        request.Id = id;
        return Result<UpdateProductRequest, Guid>(request);
    }
}
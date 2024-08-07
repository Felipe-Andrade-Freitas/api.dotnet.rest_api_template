using System.Text.Json.Serialization;
using Domain.Enums;
using MediatR;

namespace Application.Products.EventHandlers.Requests.UpdateProduct;

/// <summary>
/// Request de atualiza��o do status da venda
/// </summary>
public class UpdateProductRequest : IRequest<Guid?>
{
    [JsonIgnore]
    public Guid Id { get; set; }

    /// <summary>
    /// Status da venda a ser atualizada
    /// <example>PaymentApproved</example>
    /// </summary>
    public StatusProductEnum Status { get; set; }
}
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using Domain.Enums;
using Shared.Exceptions;

namespace Domain.Entities;

[Table("Products")]
public class Product : AuditableEntity, IAggregateRoot
{
    public string? Code { get; set; }
    public DateTime? Date { get; set; } = DateTime.Now;
    public StatusProductEnum? Status { get; set; } = StatusProductEnum.AwaitingPayment;

    public Product SetCode()
    {
        Code = $"{Date:yyyyMMdd}-{Random.Shared.Next(0, 500)}";
        return this;
    }

    public Product UpdateStatus(StatusProductEnum status)
    {
        switch (status)
        {
            case StatusProductEnum.PaymentApproved when Status == StatusProductEnum.AwaitingPayment:
                Status = status;
                break;
            case StatusProductEnum.Canceled when Status == StatusProductEnum.AwaitingPayment:
                Status = status;
                break;
            case StatusProductEnum.SentToCarrier when Status == StatusProductEnum.PaymentApproved:
                Status = status;
                break;
            case StatusProductEnum.Canceled when Status == StatusProductEnum.PaymentApproved:
                Status = status;
                break;
            case StatusProductEnum.Delivered when Status == StatusProductEnum.SentToCarrier:
                Status = status;
                break;
            default:
                throw new CustomException(string.Format(ErrorsMessages.Product.UnableToUpdateProductStatus, Status, status), statusCode: HttpStatusCode.UnprocessableEntity);
        }
        return this;
    }
}
using System.ComponentModel;

namespace Domain.Enums;

public enum StatusProductEnum
{
    [Description("Aguardando pagamento")]
    AwaitingPayment,

    [Description("Pagamento aprovado")]
    PaymentApproved,

    [Description("Enviado para transportadora")]
    SentToCarrier,

    [Description("Entregue")]
    Delivered,

    [Description("Cancelada")]
    Canceled
}
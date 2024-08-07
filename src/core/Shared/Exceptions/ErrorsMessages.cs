namespace Shared.Exceptions;

public static class ErrorsMessages
{
    public const string InternalServerError = "Ocorreu um erro inesperado. Tente novamente em alguns mintos ou contate o administrador do sistema.";
    public const string NotFoundTable = $"Não foi encontrada a tabela solicitada.";

    public static class Product
    {
        public const string NotFound = "Venda não encontrada.";
        public const string UnableToUpdateProductStatus = "Não é possível atualizar o status da venda de {0} para {1}.";
    }

    public static class Validate
    {
        public const string BadRequest = "Requisição incorreta.";

        public static class ValidateProduct
        {
            public const string DateRequired = "A data é de preenchimento obrigatório.";
            public const string SellerRequired = "O vendedor é de preenchimento obrigatório.";
            public const string MinItemsRequired = "É necessário preencher no mínimo 1 item de venda.";
        }
    }

}
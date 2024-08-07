namespace Application.Products.EventHandlers.Validators;
public static class CpfCnpjValidator
{
    private static readonly int[] CpfMultiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] CpfMultiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] CnpjMultiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
    private static readonly int[] CnpjMultiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

    public static bool IsValidCpf(string? cpf)
    {
        cpf = cpf?.Trim().Replace(".", "").Replace("-", "");
        if (cpf is not { Length: 11 } || cpf.Distinct().Count() == 1)
            return false;

        return CalculateDigit(cpf, CpfMultiplicador1, CpfMultiplicador2);
    }

    public static bool IsValidCnpj(string? cnpj)
    {
        cnpj = cnpj?.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
        if (cnpj is not { Length: 14 } || cnpj.Distinct().Count() == 1)
            return false;

        return CalculateDigit(cnpj, CnpjMultiplicador1, CnpjMultiplicador2);
    }

    private static bool CalculateDigit(string value, int[] multiplicador1, int[] multiplicador2)
    {
        var soma = 0;
        for (var i = 0; i < multiplicador1.Length; i++)
            soma += int.Parse(value[i].ToString()) * multiplicador1[i];

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (var i = 0; i < multiplicador2.Length; i++)
            soma += int.Parse(value[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return value.EndsWith(digito1 + digito2.ToString());
    }
}

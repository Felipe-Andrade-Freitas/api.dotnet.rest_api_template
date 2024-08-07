namespace Application.Products.EventHandlers.Validators;

public class ValidatorsConstants
{
    public const int CpfLength = 11;
    public const int CnpjLength = 14;

    public static bool IsDocumentCpf(string document)
    {
        return document.Length == CpfLength;
    }
}
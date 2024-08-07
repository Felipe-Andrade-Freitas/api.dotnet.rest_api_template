using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Shared.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrWhiteSpace(this string? str)
    {
        return str == null || string.IsNullOrWhiteSpace(str);
    }

    public static string Md5Hash(this string s)
    {
        using var provider = System.Security.Cryptography.MD5.Create();
        StringBuilder builder = new StringBuilder();

        foreach (byte b in provider.ComputeHash(Encoding.UTF8.GetBytes(s)))
            builder.Append(b.ToString("x2").ToLower());

        return builder.ToString();
    }

    public static string ToJson(this object value)
    {
        return JsonSerializer.Serialize(value);
    }

    public static string RemoveSpecialCharacters(this string? str)
    {
        return str == null ? string.Empty : Regex.Replace(str, "[^0-9a-zA-Z]+", "");
    }

    public static string FormatCnpj(this string str)
    {
        return Convert.ToUInt64(str).ToString(@"00\.000\.000\/0000\-00");
    }
}
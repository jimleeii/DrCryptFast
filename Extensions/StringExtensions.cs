namespace DrCryptFast.Extensions;

public static class StringExtensions
{
    public static bool EqualsIgnoreCase(this string? str1, object str2)
    {
        return string.Equals(str1, str2.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}
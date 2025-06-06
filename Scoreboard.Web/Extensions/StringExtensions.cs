namespace Scoreboard.Web.Extensions;

public static class StringExtensions
{
    public static string FormatName(this string? value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return string.Empty;
        }

        return value.Replace("(", "")
            .Replace(")", "")
            .Replace(' ', '-')
            .ToLowerInvariant();
    }
}

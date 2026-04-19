using System.Net;

namespace UltimateERP.Application.Common.Validators;

/// <summary>
/// Utility for sanitizing user input to prevent XSS attacks.
/// </summary>
public static class InputSanitizer
{
    /// <summary>
    /// Strips potentially dangerous HTML/script tags from input and HTML-encodes the result.
    /// </summary>
    public static string Sanitize(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Remove script tags and their content
        var result = System.Text.RegularExpressions.Regex.Replace(
            input, @"<script[^>]*>[\s\S]*?</script>", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove event handler attributes (onclick, onerror, etc.)
        result = System.Text.RegularExpressions.Regex.Replace(
            result, @"\bon\w+\s*=\s*""[^""]*""", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(
            result, @"\bon\w+\s*=\s*'[^']*'", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Remove iframe, object, embed, form tags
        result = System.Text.RegularExpressions.Regex.Replace(
            result, @"<(iframe|object|embed|form)[^>]*>[\s\S]*?</\1>", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        result = System.Text.RegularExpressions.Regex.Replace(
            result, @"<(iframe|object|embed|form)[^>]*/??>", string.Empty,
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // HTML-encode the remaining content
        result = WebUtility.HtmlEncode(result);

        return result;
    }

    /// <summary>
    /// Checks whether the input contains potentially dangerous content.
    /// </summary>
    public static bool ContainsDangerousContent(string? input)
    {
        if (string.IsNullOrEmpty(input))
            return false;

        var patterns = new[]
        {
            @"<script",
            @"javascript:",
            @"vbscript:",
            @"\bon\w+\s*=",
            @"<iframe",
            @"<object",
            @"<embed"
        };

        foreach (var pattern in patterns)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern,
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                return true;
        }

        return false;
    }
}

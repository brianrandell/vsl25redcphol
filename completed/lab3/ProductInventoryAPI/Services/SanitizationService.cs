using System.Net;

namespace ProductInventoryAPI.Services;

public class SanitizationService : ISanitizationService
{
    public string SanitizeInput(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return input;

        // HTML encode to prevent XSS attacks
        var sanitized = WebUtility.HtmlEncode(input);
        
        // Remove any remaining potentially dangerous characters/sequences
        sanitized = sanitized
            .Replace("<script>", "")
            .Replace("</script>", "")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("javascript:", "")
            .Replace("onclick", "")
            .Replace("onerror", "")
            .Replace("onload", "");

        return sanitized;
    }
}
using Ganss.Xss;
using System.Reflection;

namespace AdvancedCopilotAPI.Security
{
    public class InputSanitizer
    {
        private readonly HtmlSanitizer _htmlSanitizer;
        
        public InputSanitizer()
        {
            _htmlSanitizer = new HtmlSanitizer();
            _htmlSanitizer.AllowedTags.Clear();
            _htmlSanitizer.AllowedAttributes.Clear();
        }
        
        public string SanitizeHtml(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
                
            return _htmlSanitizer.Sanitize(input);
        }
        
        public string SanitizeForSql(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input ?? string.Empty;
                
            return input.Replace("'", "''")
                       .Replace("\"", "\"\"")
                       .Replace(";", "")
                       .Replace("--", "")
                       .Replace("/*", "")
                       .Replace("*/", "")
                       .Replace("xp_", "")
                       .Replace("sp_", "");
        }
        
        public T? ValidateAndSanitize<T>(T? model) where T : class
        {
            if (model == null) return model;
            
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                     .Where(p => p.PropertyType == typeof(string) && p.CanWrite);
            
            foreach (var property in properties)
            {
                var value = property.GetValue(model) as string;
                if (!string.IsNullOrEmpty(value))
                {
                    var sanitizedValue = SanitizeHtml(value);
                    property.SetValue(model, sanitizedValue);
                }
            }
            
            return model;
        }
        
        public bool ContainsSqlInjectionPatterns(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
                
            var suspiciousPatterns = new[]
            {
                "union", "select", "insert", "update", "delete", "drop", "create", "alter",
                "--", "/*", "*/", "xp_", "sp_", "exec", "execute", "script", "javascript:",
                "vbscript:", "onload", "onerror", "onclick"
            };
            
            var lowerInput = input.ToLowerInvariant();
            return suspiciousPatterns.Any(pattern => lowerInput.Contains(pattern));
        }
        
        public bool ContainsXssPatterns(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
                
            var xssPatterns = new[]
            {
                "<script", "</script>", "javascript:", "vbscript:", "onload=", "onerror=",
                "onclick=", "onmouseover=", "onfocus=", "onblur=", "eval(", "expression(",
                "url(javascript", "style=", "background:"
            };
            
            var lowerInput = input.ToLowerInvariant();
            return xssPatterns.Any(pattern => lowerInput.Contains(pattern));
        }
    }
}
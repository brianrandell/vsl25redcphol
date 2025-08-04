using System.Reflection;

namespace AdvancedCopilotAPI.Analyzers
{
    public class NamingViolation
    {
        public string MemberName { get; set; } = string.Empty;
        public string MemberType { get; set; } = string.Empty;
        public string ExpectedPattern { get; set; } = string.Empty;
        public string ActualName { get; set; } = string.Empty;
        public string Suggestion { get; set; } = string.Empty;
        public string Severity { get; set; } = "Warning";
    }
    
    public class NamingConventionAnalyzer
    {
        private readonly ILogger<NamingConventionAnalyzer> _logger;
        
        public NamingConventionAnalyzer(ILogger<NamingConventionAnalyzer> logger)
        {
            _logger = logger;
        }
        
        public List<NamingViolation> AnalyzeAssembly(Assembly assembly)
        {
            var violations = new List<NamingViolation>();
            
            var types = assembly.GetTypes()
                .Where(t => t.IsPublic && !t.IsGenericTypeDefinition)
                .ToList();
            
            foreach (var type in types)
            {
                violations.AddRange(AnalyzeType(type));
            }
            
            return violations;
        }
        
        public List<NamingViolation> AnalyzeType(Type type)
        {
            var violations = new List<NamingViolation>();
            
            if (type.IsInterface && !type.Name.StartsWith("I"))
            {
                violations.Add(new NamingViolation
                {
                    MemberName = type.Name,
                    MemberType = "Interface",
                    ExpectedPattern = "I{Name}",
                    ActualName = type.Name,
                    Suggestion = $"I{type.Name}",
                    Severity = "Warning"
                });
            }
            
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(f => !f.IsStatic);
            
            foreach (var field in fields)
            {
                if (!field.Name.StartsWith("_"))
                {
                    violations.Add(new NamingViolation
                    {
                        MemberName = field.Name,
                        MemberType = "Private Field",
                        ExpectedPattern = "_{name}",
                        ActualName = field.Name,
                        Suggestion = $"_{ToCamelCase(field.Name)}",
                        Severity = "Warning"
                    });
                }
            }
            
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !m.IsSpecialName && m.DeclaringType == type);
            
            foreach (var method in methods)
            {
                if (IsAsyncMethod(method) && !method.Name.EndsWith("Async"))
                {
                    violations.Add(new NamingViolation
                    {
                        MemberName = method.Name,
                        MemberType = "Async Method",
                        ExpectedPattern = "{Name}Async",
                        ActualName = method.Name,
                        Suggestion = $"{method.Name}Async",
                        Severity = "Warning"
                    });
                }
            }
            
            var constants = type.GetFields(BindingFlags.Public | BindingFlags.Static)
                .Where(f => f.IsLiteral);
            
            foreach (var constant in constants)
            {
                if (!IsUpperCase(constant.Name))
                {
                    violations.Add(new NamingViolation
                    {
                        MemberName = constant.Name,
                        MemberType = "Constant",
                        ExpectedPattern = "UPPER_CASE",
                        ActualName = constant.Name,
                        Suggestion = ToUpperSnakeCase(constant.Name),
                        Severity = "Warning"
                    });
                }
            }
            
            return violations;
        }
        
        public void GenerateReport(List<NamingViolation> violations, string outputPath)
        {
            var report = new System.Text.StringBuilder();
            report.AppendLine("# Naming Convention Analysis Report");
            report.AppendLine($"Generated on: {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC");
            report.AppendLine($"Total Violations: {violations.Count}");
            report.AppendLine();
            
            var groupedViolations = violations.GroupBy(v => v.MemberType);
            
            foreach (var group in groupedViolations)
            {
                report.AppendLine($"## {group.Key} Violations ({group.Count()})");
                report.AppendLine();
                
                foreach (var violation in group)
                {
                    report.AppendLine($"- **{violation.MemberName}**");
                    report.AppendLine($"  - Expected Pattern: `{violation.ExpectedPattern}`");
                    report.AppendLine($"  - Actual: `{violation.ActualName}`");
                    report.AppendLine($"  - Suggestion: `{violation.Suggestion}`");
                    report.AppendLine($"  - Severity: {violation.Severity}");
                    report.AppendLine();
                }
            }
            
            File.WriteAllText(outputPath, report.ToString());
            _logger.LogInformation("Naming convention report generated at {OutputPath}", outputPath);
        }
        
        private static bool IsAsyncMethod(MethodInfo method)
        {
            return method.ReturnType == typeof(Task) ||
                   (method.ReturnType.IsGenericType && 
                    method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>));
        }
        
        private static bool IsUpperCase(string name)
        {
            return name.All(c => !char.IsLetter(c) || char.IsUpper(c));
        }
        
        private static string ToCamelCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            return char.ToLowerInvariant(name[0]) + name[1..];
        }
        
        private static string ToUpperSnakeCase(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            
            var result = new System.Text.StringBuilder();
            
            for (int i = 0; i < name.Length; i++)
            {
                if (i > 0 && char.IsUpper(name[i]) && char.IsLower(name[i - 1]))
                {
                    result.Append('_');
                }
                
                result.Append(char.ToUpperInvariant(name[i]));
            }
            
            return result.ToString();
        }
    }
}
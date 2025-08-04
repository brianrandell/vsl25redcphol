using System.Text.Json;
using System.Text.Json.Serialization;

namespace CSharpProjectMcpServer;

/// <summary>
/// Model Context Protocol server implementation for C# project context
/// </summary>
public class McpServer
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    /// <summary>
    /// Runs the MCP server, reading from stdin and writing to stdout
    /// </summary>
    public async Task RunAsync()
    {
        string? line;
        while ((line = await Console.In.ReadLineAsync().ConfigureAwait(false)) != null)
        {
            try
            {
                var request = JsonSerializer.Deserialize<McpRequest>(line, _jsonOptions);
                var response = await HandleRequestAsync(request).ConfigureAwait(false);
                var responseJson = JsonSerializer.Serialize(response, _jsonOptions);
                Console.WriteLine(responseJson);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing request: {ex.Message}");
                
                // Send error response
                var errorResponse = new McpResponse 
                { 
                    Error = new McpError { Message = ex.Message } 
                };
                var errorJson = JsonSerializer.Serialize(errorResponse, _jsonOptions);
                Console.WriteLine(errorJson);
            }
        }
    }

    /// <summary>
    /// Handles incoming MCP requests
    /// </summary>
    /// <param name="request">The MCP request</param>
    /// <returns>MCP response</returns>
    private async Task<McpResponse> HandleRequestAsync(McpRequest? request)
    {
        if (request == null)
        {
            return new McpResponse { Error = new McpError { Message = "Invalid request" } };
        }

        return request.Method switch
        {
            "tools/list" => GetAvailableTools(),
            "tools/call" => await HandleToolCallAsync(request).ConfigureAwait(false),
            "resources/list" => GetAvailableResources(),
            "resources/read" => await HandleResourceReadAsync(request).ConfigureAwait(false),
            _ => new McpResponse { Error = new McpError { Message = $"Unknown method: {request.Method}" } }
        };
    }

    /// <summary>
    /// Returns available tools
    /// </summary>
    /// <returns>Tools list response</returns>
    private McpResponse GetAvailableTools()
    {
        return new McpResponse
        {
            Result = new
            {
                Tools = new[]
                {
                    new
                    {
                        Name = "get_project_standards",
                        Description = "Get C# project coding standards and conventions"
                    },
                    new
                    {
                        Name = "get_api_endpoints",
                        Description = "List all API endpoints in the project"
                    },
                    new
                    {
                        Name = "get_project_info",
                        Description = "Get general project information"
                    }
                }
            }
        };
    }

    /// <summary>
    /// Handles tool call requests
    /// </summary>
    /// <param name="request">The tool call request</param>
    /// <returns>Tool response</returns>
    private async Task<McpResponse> HandleToolCallAsync(McpRequest request)
    {
        if (!request.Params.HasValue)
        {
            return new McpResponse { Error = new McpError { Message = "Missing parameters" } };
        }

        var toolName = request.Params.Value.TryGetProperty("name", out var nameElement) 
            ? nameElement.GetString() 
            : null;

        return toolName switch
        {
            "get_project_standards" => await GetProjectStandardsAsync().ConfigureAwait(false),
            "get_api_endpoints" => await GetApiEndpointsAsync().ConfigureAwait(false),
            "get_project_info" => await GetProjectInfoAsync().ConfigureAwait(false),
            _ => new McpResponse { Error = new McpError { Message = $"Unknown tool: {toolName}" } }
        };
    }

    /// <summary>
    /// Returns project coding standards
    /// </summary>
    /// <returns>Project standards response</returns>
    private async Task<McpResponse> GetProjectStandardsAsync()
    {
        await Task.Delay(1).ConfigureAwait(false); // Simulate async operation
        
        var standards = new
        {
            NamingConventions = new
            {
                Classes = "PascalCase",
                Methods = "PascalCase",
                Properties = "PascalCase",
                Fields = "camelCase with underscore prefix (_field)",
                Constants = "PascalCase",
                Interfaces = "PascalCase with 'I' prefix (IService)"
            },
            AsyncPatterns = "Always use async/await, include ConfigureAwait(false) in libraries",
            ErrorHandling = "Use Result<T> pattern or global exception handler with structured logging",
            Documentation = "XML comments for public APIs, inline comments for complex logic",
            Nullability = "Enable nullable reference types, use nullable annotations",
            DependencyInjection = "Use constructor injection, register services in Program.cs",
            Testing = "Unit tests with xUnit, integration tests with WebApplicationFactory"
        };

        return new McpResponse
        {
            Result = new
            {
                Content = new[]
                {
                    new
                    {
                        Type = "text",
                        Text = JsonSerializer.Serialize(standards, _jsonOptions)
                    }
                }
            }
        };
    }

    /// <summary>
    /// Returns API endpoints information
    /// </summary>
    /// <returns>API endpoints response</returns>
    private async Task<McpResponse> GetApiEndpointsAsync()
    {
        await Task.Delay(1).ConfigureAwait(false); // Simulate async operation
        
        var endpoints = new[]
        {
            new { Method = "GET", Path = "/api/books", Description = "Get all books with optional filtering and pagination" },
            new { Method = "GET", Path = "/api/books/{id}", Description = "Get book by ID" },
            new { Method = "POST", Path = "/api/books", Description = "Create new book" },
            new { Method = "PUT", Path = "/api/books/{id}", Description = "Update existing book" },
            new { Method = "DELETE", Path = "/api/books/{id}", Description = "Delete book" },
            new { Method = "GET", Path = "/api/books/search", Description = "Advanced book search with multiple criteria and pagination" },
            new { Method = "GET", Path = "/api/books/stats", Description = "Get library statistics including totals, genres, and popular authors" }
        };

        return new McpResponse
        {
            Result = new
            {
                Content = new[]
                {
                    new
                    {
                        Type = "text",
                        Text = JsonSerializer.Serialize(endpoints, _jsonOptions)
                    }
                }
            }
        };
    }

    /// <summary>
    /// Returns general project information
    /// </summary>
    /// <returns>Project info response</returns>
    private async Task<McpResponse> GetProjectInfoAsync()
    {
        await Task.Delay(1).ConfigureAwait(false); // Simulate async operation
        
        var projectInfo = new
        {
            Name = "BookLibraryAPI",
            Description = "A comprehensive book library management API demonstrating VS Code exclusive Copilot features",
            Framework = ".NET 9.0",
            DatabaseProvider = "SQLite with Entity Framework Core",
            Architecture = new
            {
                Pattern = "Layered Architecture",
                Layers = new[] { "Controllers", "Services", "Data", "Models", "DTOs" },
                Features = new[] { "CRUD Operations", "Search & Filtering", "Pagination", "Statistics" }
            },
            Technologies = new[]
            {
                "ASP.NET Core Web API",
                "Entity Framework Core",
                "SQLite",
                "OpenAPI/Swagger",
                "Dependency Injection",
                "Async/Await patterns"
            },
            DemoFeatures = new[]
            {
                "Agent Mode (@workspace) context awareness",
                "Next Edit Suggestions for refactoring",
                "MCP integration for real-time project context",
                "Multi-model AI support for different tasks"
            }
        };

        return new McpResponse
        {
            Result = new
            {
                Content = new[]
                {
                    new
                    {
                        Type = "text",
                        Text = JsonSerializer.Serialize(projectInfo, _jsonOptions)
                    }
                }
            }
        };
    }

    /// <summary>
    /// Returns available resources
    /// </summary>
    /// <returns>Resources list response</returns>
    private McpResponse GetAvailableResources()
    {
        return new McpResponse
        {
            Result = new
            {
                Resources = new[]
                {
                    new
                    {
                        Uri = "file://project-structure",
                        Name = "Project Structure",
                        Description = "Current project folder structure",
                        MimeType = "application/json"
                    },
                    new
                    {
                        Uri = "file://database-schema",
                        Name = "Database Schema",
                        Description = "Database tables and relationships",
                        MimeType = "application/json"
                    }
                }
            }
        };
    }

    /// <summary>
    /// Handles resource read requests
    /// </summary>
    /// <param name="request">The resource read request</param>
    /// <returns>Resource content response</returns>
    private async Task<McpResponse> HandleResourceReadAsync(McpRequest request)
    {
        if (!request.Params.HasValue)
        {
            return new McpResponse { Error = new McpError { Message = "Missing parameters" } };
        }

        var uri = request.Params.Value.TryGetProperty("uri", out var uriElement) 
            ? uriElement.GetString() 
            : null;

        return uri switch
        {
            "file://project-structure" => await GetProjectStructureAsync().ConfigureAwait(false),
            "file://database-schema" => await GetDatabaseSchemaAsync().ConfigureAwait(false),
            _ => new McpResponse { Error = new McpError { Message = $"Resource not found: {uri}" } }
        };
    }

    /// <summary>
    /// Returns project structure information
    /// </summary>
    /// <returns>Project structure response</returns>
    private async Task<McpResponse> GetProjectStructureAsync()
    {
        await Task.Delay(1).ConfigureAwait(false); // Simulate async operation
        
        var structure = new
        {
            BookLibraryAPI = new
            {
                Controllers = new[] { "BooksController.cs" },
                Models = new[] { "Book.cs" },
                DTOs = new[] 
                { 
                    "BookDto.cs", 
                    "CreateBookDto.cs", 
                    "UpdateBookDto.cs", 
                    "BookSearchDto.cs", 
                    "PaginatedResultDto.cs",
                    "BookStatsDto.cs"
                },
                Data = new[] { "LibraryContext.cs" },
                Services = new[] { "IBookService.cs", "BookService.cs" },
                Configuration = new[] { "Program.cs", "appsettings.json" }
            },
            CSharpProjectMcpServer = new
            {
                Files = new[] { "Program.cs" },
                Purpose = "Custom MCP server for project context"
            },
            Tests = new
            {
                IntegrationTests = new[] { "BooksControllerTests.cs" },
                TestUtilities = new[] { "TestWebApplicationFactory.cs" }
            }
        };

        return new McpResponse
        {
            Result = new
            {
                Contents = new[]
                {
                    new
                    {
                        Uri = "file://project-structure",
                        MimeType = "application/json",
                        Text = JsonSerializer.Serialize(structure, _jsonOptions)
                    }
                }
            }
        };
    }

    /// <summary>
    /// Returns database schema information
    /// </summary>
    /// <returns>Database schema response</returns>
    private async Task<McpResponse> GetDatabaseSchemaAsync()
    {
        await Task.Delay(1).ConfigureAwait(false); // Simulate async operation
        
        var schema = new
        {
            Tables = new
            {
                Books = new
                {
                    Columns = new object[]
                    {
                        new { Name = "Id", Type = "INTEGER", IsPrimaryKey = true, IsAutoIncrement = true },
                        new { Name = "Title", Type = "TEXT", IsRequired = true, MaxLength = 200 },
                        new { Name = "Author", Type = "TEXT", IsRequired = true, MaxLength = 100 },
                        new { Name = "ISBN", Type = "TEXT", IsRequired = false, MaxLength = 20, IsUnique = true },
                        new { Name = "PublishedDate", Type = "TEXT", IsRequired = true },
                        new { Name = "Genre", Type = "TEXT", IsRequired = false, MaxLength = 50 },
                        new { Name = "IsAvailable", Type = "INTEGER", IsRequired = true, DefaultValue = 1 },
                        new { Name = "CreatedAt", Type = "TEXT", IsRequired = true },
                        new { Name = "UpdatedAt", Type = "TEXT", IsRequired = true }
                    },
                    Indexes = new object[]
                    {
                        new { Name = "IX_Books_ISBN", Columns = new[] { "ISBN" }, IsUnique = true },
                        new { Name = "IX_Books_Title", Columns = new[] { "Title" } },
                        new { Name = "IX_Books_Author", Columns = new[] { "Author" } },
                        new { Name = "IX_Books_Genre", Columns = new[] { "Genre" } },
                        new { Name = "IX_Books_IsAvailable", Columns = new[] { "IsAvailable" } }
                    },
                    SeedData = new { RecordCount = 8, Description = "Sample books including classics and technical books" }
                }
            }
        };

        return new McpResponse
        {
            Result = new
            {
                Contents = new[]
                {
                    new
                    {
                        Uri = "file://database-schema",
                        MimeType = "application/json",
                        Text = JsonSerializer.Serialize(schema, _jsonOptions)
                    }
                }
            }
        };
    }
}

/// <summary>
/// MCP request message
/// </summary>
public record McpRequest
{
    /// <summary>
    /// Request method name
    /// </summary>
    public string? Method { get; init; }

    /// <summary>
    /// Request parameters
    /// </summary>
    public JsonElement? Params { get; init; }
}

/// <summary>
/// MCP response message
/// </summary>
public record McpResponse
{
    /// <summary>
    /// Response result (success case)
    /// </summary>
    public object? Result { get; init; }

    /// <summary>
    /// Error information (error case)
    /// </summary>
    public McpError? Error { get; init; }
}

/// <summary>
/// MCP error information
/// </summary>
public record McpError
{
    /// <summary>
    /// Error message
    /// </summary>
    public string? Message { get; init; }
}

/// <summary>
/// Main program entry point
/// </summary>
class Program
{
    /// <summary>
    /// Application entry point
    /// </summary>
    /// <param name="args">Command line arguments</param>
    static async Task Main(string[] args)
    {
        var server = new McpServer();
        await server.RunAsync().ConfigureAwait(false);
    }
}
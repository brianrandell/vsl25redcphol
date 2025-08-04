# Lab 9: VS Code Exclusive Copilot Features with C#/.NET - Completed Solution

This folder contains the complete solution for Lab 9, demonstrating VS Code exclusive
Copilot features using C# and .NET 9.0.
The solution includes a comprehensive book library management API and a custom MCP server.

## Project Structure

``` shell
lab9/
├── BookLibraryAPI/                 # Main ASP.NET Core Web API project
│   ├── Controllers/                # API controllers
│   │   └── BooksController.cs     # Books CRUD operations
│   ├── Models/                     # Entity models
│   │   └── Book.cs                # Book entity
│   ├── DTOs/                       # Data Transfer Objects
│   │   ├── BookDto.cs             # Book response DTO
│   │   ├── CreateBookDto.cs       # Book creation DTO
│   │   ├── UpdateBookDto.cs       # Book update DTO
│   │   ├── BookSearchDto.cs       # Search parameters DTO
│   │   ├── PaginatedResultDto.cs  # Pagination wrapper
│   │   └── BookStatsDto.cs        # Statistics DTO
│   ├── Data/                       # Data access layer
│   │   └── LibraryContext.cs      # Entity Framework context
│   ├── Services/                   # Business logic layer
│   │   ├── IBookService.cs        # Service interface
│   │   └── BookService.cs         # Service implementation
│   └── Program.cs                  # Application entry point
├── CSharpProjectMcpServer/         # Custom MCP server (C# implementation)
│   └── Program.cs                  # MCP server implementation
├── BookLibraryAPI.Tests/           # Integration tests
│   └── BooksControllerTests.cs    # Comprehensive API tests
└── README.md                       # This file
```

## Technologies Used

* **.NET 9.0** - Latest .NET framework
* **ASP.NET Core Web API** - RESTful API framework
* **Entity Framework Core** - ORM with SQLite provider
* **SQLite** - Lightweight database for demo purposes
* **OpenAPI/Swagger** - API documentation
* **xUnit** - Testing framework
* **FluentAssertions** - Fluent testing assertions
* **System.Text.Json** - JSON serialization

## Features Demonstrated

### VS Code Exclusive Copilot Features (August 2025)

* **Auto-Generated Custom Instructions** with `Chat: Generate Instructions` command
* **Custom Chat Modes** with specific tools and model preferences  
* **Edit/Iterate Conversation** capabilities for refining AI responses
* **Local MCP Server Integration** for development context without GitHub dependency
* **Terminal Auto-Approval** with configurable allow/deny lists
* **Extension Ecosystem Integration** with C# Dev Kit and multi-root workspaces
* **Resource Drag-and-Drop** for visual context integration
* **Advanced Conversation Management** with session tracking and history

## API Endpoints

### Book Management

* **GET /api/books** - Get all books with optional filtering
* **GET /api/books/{id}** - Get a specific book by ID
* **GET /api/books/search** - Advanced search with pagination
* **GET /api/books/stats** - Get library statistics
* **POST /api/books** - Create a new book
* **PUT /api/books/{id}** - Update an existing book
* **DELETE /api/books/{id}** - Delete a book

### Sample Requests

#### Create a Book

```json
POST /api/books
{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "isbn": "978-0132350884",
  "publishedDate": "2008-08-01",
  "genre": "Technology",
  "isAvailable": true
}
```

#### Search Books

```json
GET /api/books/search?title=clean&author=martin&page=1&pageSize=10
```

## MCP Server Implementation

The custom C# MCP server provides context-aware assistance by exposing:

### Tools

* **get_project_standards** - Returns coding standards and conventions
* **get_api_endpoints** - Lists all available API endpoints

### Resources

* **file://project-structure** - Current project folder structure

### Usage in VS Code

``` shell
@csharp-project What are the coding standards for this project?
@csharp-project Show me all API endpoints
```

## Getting Started

### Prerequisites

* **Visual Studio Code** with C# Dev Kit extension
* **.NET 9.0 SDK**
* **Node.js** (for SQLite MCP server only)
* **GitHub Copilot** extension with VS Code experimental features enabled

### Running the API

```cmd
cd BookLibraryAPI
dotnet restore
dotnet run
```

The API will be available at `https://localhost:5001/swagger`

### Running the MCP Server

```cmd
cd CSharpProjectMcpServer
dotnet run
```

### Running Tests

```cmd
cd BookLibraryAPI.Tests
dotnet test
```

## Key Learning Points

### VS Code Exclusive Features

1. **Advanced Custom Instructions**

    * Auto-generated project-specific guidance
    * Custom chat modes with tailored contexts
    * Conversation editing and refinement
    * Context preservation across sessions

2. **Local Development Excellence**

    * Local MCP servers without external dependencies
    * Terminal auto-approval for trusted commands
    * Multi-terminal coordination with AI assistance
    * Advanced debugging integration

3. **Extension Ecosystem Integration**

    * Deep integration with C# Dev Kit
    * Cross-platform development optimization
    * Multi-root workspace intelligence
    * Resource drag-and-drop analysis

### Code Quality Features

* **Comprehensive DTOs** for clean API contracts
* **Service Layer Pattern** for business logic separation
* **Entity Framework Core** for database operations
* **Integration Testing** for API reliability
* **Swagger Documentation** for API exploration

## Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=library.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### VS Code Settings for Advanced Features

```json
{
  "github.copilot.chat.mcpServers": {
    "csharp-project": {
      "command": "dotnet",
      "args": ["run", "--project", "./CSharpProjectMcpServer/CSharpProjectMcpServer.csproj"],
      "name": "C# Project Context"
    }
  },
  "github.copilot.chat.customModes": {
    "api-architect": {
      "instructions": "You are an expert .NET API architect. Focus on scalable, maintainable API design patterns.",
      "allowedTools": ["workspace", "terminal"],
      "preferredModel": "gpt-4"
    }
  },
  "terminal.agent.allowList": [
    "dotnet build",
    "dotnet run", 
    "dotnet test",
    "dotnet add package*"
  ],
  "terminal.agent.autoApproval": true
}
```

## Database Schema

### Books Table

```sql
CREATE TABLE Books (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Title TEXT NOT NULL,
    Author TEXT NOT NULL,
    ISBN TEXT,
    PublishedDate TEXT,
    Genre TEXT,
    IsAvailable INTEGER NOT NULL DEFAULT 1,
    CreatedAt TEXT NOT NULL,
    UpdatedAt TEXT NOT NULL
);
```

## Troubleshooting

### Common Issues

1. **MCP Server Not Connecting**

    * Ensure dotnet process has necessary permissions
    * Check VS Code Developer Console for errors
    * Restart VS Code after configuration changes

2. **Database Errors**

    * Delete library.db and let EF Core recreate it
    * Run `dotnet ef database update` to apply migrations

3. **Custom Chat Modes Not Working**

    * Ensure you have the latest Copilot extension (v1.102+)
    * Check settings.json syntax is valid
    * Restart VS Code after adding custom modes

## Next Steps

* Add authentication and authorization
* Implement caching for better performance
* Add GraphQL support alongside REST
* Create a frontend application
* Deploy to Azure or AWS
* Add real-time features with SignalR

---

**Note**: This project demonstrates VS Code exclusive features as of August 2025.
While the API can be opened in Visual Studio 2022, the advanced features (custom instructions,
conversation editing, terminal auto-approval, local MCP) are VS Code specific.

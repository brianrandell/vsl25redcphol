# GitHub Copilot Hands-on Labs - Completed Solutions

This folder contains all completed implementations for the GitHub Copilot training labs from VSLIVE! 2025 Redmond. Each lab demonstrates different aspects of GitHub Copilot integration with Visual Studio 2022 and various development scenarios.

## Quick Start

1. **Open Visual Studio 2022**
2. **Open Solution**: `completed\cphol25red.sln`
3. **Set Startup Project**: Right-click any project and select "Set as Startup Project"
4. **Run**: Press `F5` to launch the selected project

## Lab Overview

### [Lab 1: Copilot Fundamentals - Calculator Console App](lab1/README.md)

**Duration**: 15 minutes | **Technology**: Console Application (.NET 9)

Basic GitHub Copilot usage with a simple calculator application. Learn fundamental Copilot features like code completion, suggestions, and comment-driven development.

**Key Features**:

* Basic arithmetic operations
* Input validation and error handling
* Menu-driven console interface
* Comment-to-code generation

**Getting Started**: Set `CopilotCalculator` as startup project and press F5

---

### [Lab 2: Advanced Code Generation - Todo List Manager](lab2/README.md)

**Duration**: 20 minutes | **Technology**: Console Application (.NET 9)

Advanced code generation techniques with a feature-rich todo list manager. Demonstrates complex data structures, file I/O, and business logic generation.

**Key Features**:

* Todo item management with priorities
* JSON file persistence
* LINQ operations and filtering
* Service layer pattern

**Getting Started**: Set `TodoListManager` as startup project and press F5

---

### [Lab 3: Copilot Chat Integration - Product Inventory API](lab3/README.md)

**Duration**: 25 minutes | **Technology**: ASP.NET Core Web API (.NET 9)

RESTful API development using Copilot Chat features. Learn how to build comprehensive web APIs with proper error handling and documentation.

**Key Features**:

* Full CRUD REST API
* DTOs and mapping extensions  
* Custom exception handling
* Swagger/OpenAPI integration

**Getting Started**: Set `ProductInventoryAPI` as startup project and press F5 (opens Swagger UI)

---

### [Lab 4: Test-Driven Development - API Testing Suite](lab4/README.md)

**Duration**: 25 minutes | **Technology**: xUnit Testing (.NET 9)

Comprehensive testing strategies using GitHub Copilot for TDD. Includes unit tests, integration tests, and test data builders.

**Key Features**:

* xUnit + FluentAssertions
* Integration testing with TestServer
* Builder pattern for test data
* Full API endpoint coverage

**Getting Started**: Right-click `ProductInventoryAPI.Tests` → "Run Tests" or use Test Explorer

---

### [Lab 5: Debugging with Copilot - Order Processing System](lab5/README.md)

**Duration**: 20 minutes | **Technology**: Console Application (.NET 9)

Learn to debug and fix common issues in an order processing system using GitHub Copilot. Covers null reference exceptions, logical errors, and performance bottlenecks.

**Key Features**:

* Defensive programming and null checks
* Input validation and error handling
* Performance optimization with parallel processing
* Identifying and fixing logical bugs

**Getting Started**: Set `DebuggingWithCopilot` as startup project and press F5

---

### [Lab 6: Database Integration - EF Core API](lab6/README.md)

**Duration**: 30 minutes | **Technology**: ASP.NET Core + Entity Framework Core (.NET 9)

Database integration using Entity Framework Core with SQL Server. Demonstrates repository patterns, migrations, and database seeding.

**Key Features**:

* Entity Framework Core with SQL Server
* Code-first migrations and seeding
* Repository pattern implementation
* Relationship mapping and configurations

**Getting Started**: Set `ProductInventoryAPIwithEFC` as startup project and press F5 (auto-creates database)

---

### [Lab 7: WPF Desktop Development - Expense Tracker](lab7/README.md)

**Duration**: 30 minutes | **Technology**: WPF Application (.NET 9)

Modern desktop application development with WPF using MVVM pattern. Rich UI with data binding, validation, and local data persistence.

**Key Features**:

* MVVM architecture with data binding
* Rich WPF UI with DataGrid and forms
* Value converters and validation
* Local JSON storage

**Getting Started**: Set `ExpenseTracker` as startup project and press F5

---

### [Lab 8: Advanced Copilot Techniques - Enterprise API](lab8/README.md)

**Duration**: 35 minutes | **Technology**: ASP.NET Core 9.0 + Advanced Patterns

Enterprise-level application demonstrating advanced Copilot techniques including security, performance optimization, design patterns, and comprehensive testing.

**Key Features**:

* Security (XSS protection, input sanitization)
* Performance (caching, resilience patterns with Polly)
* Design patterns (Factory, Decorator, Strategy, Saga)
* Advanced error handling and monitoring
* Comprehensive integration testing

**Getting Started**: Set `AdvancedCopilotAPI` as startup project and press F5 (opens Swagger UI)

---

### [Lab 9: VS Code Exclusive Copilot Features - Book Library API](lab9/README.md)

**Duration**: 30 minutes | **Technology**: ASP.NET Core 9.0 Web API + VS Code Features

Comprehensive book library management API demonstrating VS Code exclusive Copilot features including Agent Mode, Next Edit Suggestions, and MCP integration using C#/.NET 9.0.

**Key Features**:

* Agent Mode for autonomous coding and project understanding
* Next Edit Suggestions for predictive refactoring
* MCP integration for enhanced context and real-time documentation access
* Full CRUD operations with advanced search and statistics
* Comprehensive integration testing

**Getting Started**: Open in VS Code, set `BookLibraryAPI` as startup project and run with `dotnet run`

---

### [Lab 10: GitHub Copilot Agent - Interactive Calculator](lab10/README.md)

**Duration**: 25 minutes | **Technology**: .NET 9.0 Console Application

Demonstrates using GitHub Copilot Agent to autonomously develop a feature-rich interactive calculator from a GitHub issue. Learn how to write effective prompts for AI-driven development and review AI-generated code.

**Key Features**:

* Interactive menu with 5 mathematical operations
* Calculation history with timestamps
* Memory functions (M+, M-, MR, MC)
* Professional error handling and input validation
* History export to text files

**Getting Started**: Set `CalculatorApp` as startup project and press F5

## Solution Structure

``` shell
completed/
├── cphol25red.sln              # Visual Studio solution file
├── lab1/                       # Console calculator
├── lab2/                       # Todo list manager  
├── lab3/                       # Basic Web API
├── lab4/                       # API testing suite
├── lab5/                       # Debugging with Copilot
├── lab6/                       # Web API with Entity Framework
├── lab7/                       # WPF desktop application
├── lab8/                       # Advanced enterprise API
│   ├── AdvancedCopilotAPI/     # Main API project
│   └── AdvancedCopilotAPI.Tests/ # Integration tests
├── lab9/                       # VS Code exclusive features (C#)
│   ├── BookLibraryAPI/         # Book library Web API
│   └── BookLibraryAPI.Tests/   # Integration tests
└── lab10/                      # GitHub Copilot Agent
    └── CalculatorApp/          # Interactive calculator console app
```

## Prerequisites

### Required Software

* **Visual Studio 2022** (any edition) with workloads:
  * .NET desktop development
  * ASP.NET and web development
* **GitHub Copilot** extension
* **SQL Server LocalDB** (for Lab 6)

### Supported Platforms

* **Windows 10/11** (all labs)

## Technology Stack Summary

| Lab | Framework | Database | UI | Testing |
|-----|-----------|----------|----| --------|
| 1 | .NET 9 Console | None | Console | None |
| 2 | .NET 9 Console | JSON Files | Console | None |
| 3 | ASP.NET Core Web API | In-Memory | Swagger UI | None |
| 4 | xUnit Testing | None | None | xUnit + FluentAssertions |
| 5 | .NET 9 Console | None | Console | None |
| 6 | ASP.NET Core + EF Core | SQL Server | Swagger UI | None |
| 7 | WPF Desktop | JSON Files | WPF/XAML | None |
| 8 | ASP.NET Core 9.0 | In-Memory | Swagger UI | xUnit Integration Tests |
| 9 | ASP.NET Core 9.0 Web API | SQLite + EF Core | Swagger UI | xUnit Integration Tests |
| 10| .NET 9.0 Console | File Export | Console | None |

## Learning Progression

The labs are designed to build upon each other:

1. **Lab 1**: Basic Copilot features and code completion
2. **Lab 2**: Advanced code generation and complex logic
3. **Lab 3**: Web API development and Copilot Chat
4. **Lab 4**: Test-driven development with Copilot
5. **Lab 5**: Debugging techniques and performance optimization
6. **Lab 6**: Database integration and data persistence
7. **Lab 7**: Desktop UI development with WPF
8. **Lab 8**: Enterprise patterns and advanced techniques
9. **Lab 9**: VS Code exclusive features and MCP integration
10. **Lab 10**: GitHub Copilot Agent and autonomous development

## Build and Test Status

All projects build successfully with:

* ✅ **0 Warnings**
* ✅ **0 Errors**  
* ✅ **All Dependencies Resolved**

### Running All Tests

**In Visual Studio 2022**: Open Test Explorer (Test → Test Explorer) and click "Run All Tests"

**Alternative (Command Line)**:
```shell
# From the completed folder
dotnet test --logger "console;verbosity=detailed"
```

## Common Troubleshooting

### Build Issues

1. Ensure all required workloads are installed in Visual Studio
2. Restore NuGet packages: `Tools` → `NuGet Package Manager` → `Restore NuGet Packages`
3. Clean and rebuild: `Build` → `Clean Solution` → `Rebuild Solution`

### Database Issues (Lab 6)

* Verify SQL Server LocalDB is installed
* Check connection strings in `appsettings.json`
* Delete database to recreate: Delete `ProductInventoryDB` from SQL Server Object Explorer

### GitHub Copilot Issues

* Ensure Copilot extension is installed and activated
* Sign in to GitHub account with active Copilot subscription
* Check Copilot status in Visual Studio status bar

## Support and Resources

### Documentation

* Each lab has its own detailed README with step-by-step instructions
* Code comments explain complex implementations
* Sample data and usage examples provided

### GitHub Copilot Resources

* [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
* [Visual Studio Integration Guide](https://docs.microsoft.com/en-us/visualstudio/ide/visual-studio-github-copilot-extension)
* [Best Practices for AI-Assisted Development](https://github.blog/2023-06-20-how-to-write-better-prompts-for-github-copilot/)

---

**Training Context**: These labs were created for VSLIVE! 2025 Redmond GitHub Copilot training session. They demonstrate practical, real-world usage of GitHub Copilot across different development scenarios and technologies.

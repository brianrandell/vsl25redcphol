# Lab 8: Advanced Copilot Techniques - Hands-on Exercise

**Duration:** 45 minutes

## Prerequisites

* Visual Studio 2022 with GitHub Copilot enabled
* Completed previous labs or equivalent experience
* Projects from previous labs available
* **Internet connection** - Required for Microsoft Learn MCP server integration exercises

## Exercise Overview

Master advanced Copilot techniques including multi-file context awareness, code reviews, security best practices, performance optimization, async patterns, design pattern implementation, and MCP integration.

---

## Part 1: Multi-File Context and Refactoring (8 minutes)

### Step 1: Open Multiple Related Files

1. **Open your Web API project** from the previous Lab 6 or use the completed version from the /completed folder. Make sure it builds with zero warnings and zero errors.

2. **Open these files in separate tabs:**

   * `ProductsController.cs`
   * `Product.cs` model
   * `IProductRepository.cs`
   * `ProductRepository.cs`

3. **Use the #<filename> synctax to add the three files that are not the active document to the chat window.** (Copilot uses the files for context)

### Step 2: Refactor Across Multiple Files

1. **In Copilot Chat, type:**

   ```
   Analyze these open files and suggest improvements for:
   1. Consistency in naming conventions
   2. Missing interface methods
   3. Potential code duplication
   4. Better separation of concerns
   ```

2. **Select specific refactoring suggestion** from Chat

3. **Ask Copilot to:**

   ```
   Show me how to implement the Repository pattern more completely across these files, including unit of work pattern
   ```

### Step 3: Generate Missing Pieces

1. **Create new interface** `IUnitOfWork.cs`

2. **Add the other files back or use the @Workspace to add the context in chat, and type the commen (or if you perfer use completings in the editor -- your choice):**

   ```csharp
   // Unit of Work interface that works with the existing repositories
   // Should coordinate transactions across multiple repositories
   // Include methods for Save, Rollback, and Dispose
   // Reference the existing repository interfaces
   ```

3. **Notice how Copilot uses context** from open files

---

## Part 2: Code Review and Security Analysis (7 minutes)

### Step 1: Security Review

1. **Make `ProductsController.cs` the active document**

2. **In Copilot Chat, ask:**

   ```
   Review this code for security vulnerabilities:
   - SQL injection risks
   - XSS vulnerabilities  
   - Authentication/authorization issues
   - Input validation gaps
   - Information disclosure
   Provide specific fixes for each issue found
   ```

3. **Implement suggested security fixes**

### Step 2: Add Security Measures

1. **Ask Copilot Chat:**

   ```
   Add comprehensive input sanitization to prevent XSS attacks in all string properties
   ```

2. **Create new class** `Security/InputSanitizer.cs`

3. **Type comment (or use Chat):**

   ```csharp
   // Input sanitizer to prevent XSS attacks
   // Methods:
   // - SanitizeHtml(string) - remove dangerous HTML
   // - SanitizeForSql(string) - prevent SQL injection
   // - ValidateAndSanitize<T>(T model) - sanitize all string properties
   // Use HtmlSanitizer library for HTML cleaning
   ```

### Step 3: Code Quality Review

1. **Select a complex method** from any class

2. **Ask Copilot Chat:**

   ```
   Review this method for:
   - Cyclomatic complexity
   - Single Responsibility Principle violations
   - Potential null reference exceptions
   - Performance issues
   Suggest refactored version
   ```

---

## Part 3: Performance Optimization (8 minutes)

### Step 1: Identify Performance Issues

1. **Open your `ProductRepository.cs`**

2. **Select a method with database queries**

3. **Ask Copilot Chat:**

   ```
   Analyze this code for performance issues:
   - N+1 query problems
   - Missing indexes
   - Inefficient LINQ queries
   - Memory leaks
   Provide optimized version
   ```

### Step 2: Implement Caching

1. **Create new class** `Services/CacheService.cs`

2. **Type comment:**

   ```csharp
   // Generic memory cache service using IMemoryCache
   // Features:
   // - Generic Get<T> and Set<T> methods
   // - Configurable expiration
   // - Cache key generation
   // - Cache invalidation by pattern
   // - Distributed cache support ready
   // Thread-safe implementation
   ```

3. **Update repository to use caching:**

   ```csharp
   // Modify GetAllProductsAsync to:
   // - Check cache first
   // - Query database if cache miss
   // - Store result in cache
   // - Include cache invalidation logic
   ```

### Step 3: Async Optimization

1. **Select methods using async/await**

2. **Ask Copilot Chat:**

   ```
   Optimize these async methods:
   - Remove unnecessary async/await
   - Use ConfigureAwait(false) where appropriate
   - Implement parallel operations where beneficial
   - Add cancellation token support
   ```

---

## Part 4: Implement Design Patterns (8 minutes)

### Step 1: Factory Pattern

1. **Create folder** `Factories`

2. **Add new interface** `Factories/INotificationFactory.cs`

3. **Type comment:**

   ```csharp
   // Factory pattern for creating notification services
   // Support multiple notification types:
   // - Email notifications
   // - SMS notifications  
   // - Push notifications
   // - In-app notifications
   // Factory should return appropriate service based on type
   ```

4. **Let Copilot generate interface and implementations**

### Step 2: Decorator Pattern

1. **Create folder** `Decorators`

2. **Add comment for logging decorator:**

   ```csharp
   // Decorator pattern to add logging to any repository
   // Should:
   // - Wrap any IRepository implementation
   // - Log method entry, exit, and exceptions
   // - Measure execution time
   // - Preserve original functionality
   // Generic implementation for any entity type
   ```

3. **Apply decorator in dependency injection**

### Step 3: Strategy Pattern

1. **Create pricing strategy example:**

   ```csharp
   // Strategy pattern for product pricing
   // Strategies:
   // - Regular pricing
   // - Bulk discount pricing (10% off for 10+ items)
   // - Member pricing (15% off for members)
   // - Seasonal pricing (custom discount percentage)
   // Context object contains quantity, customer type, season
   ```

2. **Implement with Copilot's help**

---

## Part 5: Advanced Error Handling and Resilience (6 minutes)

### Step 1: Implement Polly for Resilience

1. **Install NuGet package:** `Microsoft.Extensions.Http.Polly`

2. **Create new class** `Services/ResilientHttpClient.cs`

3. **Type comment:**

   ```csharp
   // Resilient HTTP client using Polly
   // Policies to implement:
   // - Retry policy: 3 attempts with exponential backoff
   // - Circuit breaker: Open after 5 failures
   // - Timeout policy: 30 seconds
   // - Bulkhead isolation: Max 10 concurrent calls
   // Combine policies using PolicyWrap
   // Log all retry attempts and circuit state changes
   ```

### Step 2: Advanced Exception Handling

1. **Create** `Handlers/GlobalExceptionHandler.cs`

2. **Type:**

   ```csharp
   // Advanced global exception handler that:
   // - Categorizes exceptions (business, validation, system)
   // - Generates correlation IDs for tracking
   // - Logs with structured logging
   // - Returns appropriate HTTP status codes
   // - Includes retry-after headers for rate limiting
   // - Sanitizes error messages for production
   ```

### Step 3: Implement Saga Pattern

1. **For distributed transactions, add:**

   ```csharp
   // Saga pattern for order processing
   // Steps: Reserve inventory -> Charge payment -> Send notification
   // Each step should have compensating action
   // Handle partial failures gracefully
   ```

---

## Part 6: Using Popular MCP Servers (15 minutes)

### Step 1: Verify Prerequisites and Setup

1. **Verify internet connectivity:**
   
   Ensure you can access Microsoft Learn documentation:
   * Open your browser and navigate to [https://learn.microsoft.com](https://learn.microsoft.com)
   * This verifies the HTTP-based MCP server will be accessible

2. **Open GitHub Copilot Chat**

3. **Check available MCP servers:**
   * Click the `@` symbol in Chat
   * Look for available context providers
   * Note any configured MCP servers (you might not have any)

### Step 2: Using Popular MCP Servers

1. **Understanding HTTP-based MCP servers:**

   The Microsoft Learn MCP server uses HTTP instead of local npm packages, making it more reliable and easier to configure. No need to install additional packages via npm.

2. **Configure Microsoft Learn MCP server:**

   Create or update your `%USERPROFILE%\.mcp.json` file with this configuration:
   ```json
   {
     "servers": {
       "microsoft.docs.mcp": {
         "type": "http",
         "url": "https://learn.microsoft.com/api/mcp"
       }
     }
   }
   ```

   **Note:** This uses the HTTP-based Microsoft Learn MCP server, which is more reliable than npm package-based servers.

3. **Test the configuration:**

   Restart Visual Studio, then in Copilot Chat:
   ```
   @microsoft.docs.mcp How do I implement dependency injection in .NET 9?
   ```
   
   You can also try:
   ```
   @microsoft.docs.mcp What are the best practices for ASP.NET Core Web APIs?
   ```

3. **Hands-on Microsoft Learn MCP Exercise:**

   **Practice using the Microsoft Learn MCP server with your API project:**

   a) **Ask about .NET best practices:**
      ```
      @microsoft.docs.mcp What are the latest .NET 9 performance optimizations I should implement in my Web API?
      ```

   b) **Get security guidance:**
      ```
      @microsoft.docs.mcp How do I implement proper input validation and security headers in ASP.NET Core?
      ```

   c) **Learn about testing strategies:**
      ```
      @microsoft.docs.mcp What are the best practices for integration testing in ASP.NET Core Web APIs?
      ```

   d) **Ask about caching patterns:**
      ```
      @microsoft.docs.mcp How do I implement distributed caching with Redis in .NET 9?
      ```

   **Expected Outcomes:**
   - Access to up-to-date Microsoft documentation and best practices
   - Context-aware responses about .NET and ASP.NET Core
   - Integration with your current Web API development workflow
   - Real-time access to official Microsoft guidance

### Step 4: Advanced Microsoft Learn MCP Usage

1. **Explore different types of queries:**

   ```
   @microsoft.docs.mcp What are the latest Entity Framework Core performance improvements in .NET 9?
   ```

2. **Get architecture guidance:**

   ```
   @microsoft.docs.mcp How do I implement clean architecture patterns in ASP.NET Core?
   ```

3. **Learn about deployment and DevOps:**

   ```
   @microsoft.docs.mcp What are the best practices for deploying .NET 9 applications to Azure?
   ```

4. **Combine with your current code context:**

   With your ProductsController.cs open, ask:
   ```
   @microsoft.docs.mcp How can I improve this controller's performance and security based on current .NET best practices?
   ```

---

## Part 7: Code Analysis and Metrics (5 minutes)

### Step 1: Generate Code Metrics

1. **Select your entire solution in Solution Explorer**

2. **In Copilot Chat, ask:**

   ```
   Analyze the codebase and provide:
   - Cyclomatic complexity for each method
   - Classes that violate SOLID principles
   - Potential memory leaks
   - Dead code detection
   - Suggestions for improving maintainability index
   ```

### Step 2: Create Custom Analyzers

1. **Create folder** `Analyzers`

2. **Add class** `Analyzers/NamingConventionAnalyzer.cs`

3. **Type:**

   ```csharp
   // Custom Roslyn analyzer to enforce naming conventions:
   // - Interfaces must start with 'I'
   // - Private fields must start with underscore
   // - Async methods must end with 'Async'
   // - Constants must be UPPER_CASE
   // Report violations as warnings
   ```

### Step 3: Performance Profiling Helpers

1. **Create** `Diagnostics/PerformanceMonitor.cs`

2. **Type:**

   ```csharp
   // Performance monitoring utilities:
   // - Method execution time tracker
   // - Memory usage snapshots
   // - Database query counter
   // - HTTP request duration logger
   // - Automatic slow operation detection
   // Integration with Application Insights
   ```

---

## Integration Testing Everything (3 minutes)

1. **Create comprehensive integration test:**

   ```csharp
   // Integration test that:
   // - Uses all advanced features implemented
   // - Tests resilience policies
   // - Verifies caching behavior
   // - Validates security measures
   // - Measures performance improvements
   ```

2. **Run all tests** using Test Explorer (Test → Test Explorer) and verify improvements

3. **Use Copilot Chat to generate:**

   ```
   Create a benchmark test comparing performance before and after optimizations
   ```

---

## Key Takeaways

✅ **Advanced techniques mastered:**

* Multi-file context awareness for better suggestions
* Security-focused code reviews
* Performance optimization patterns
* Design pattern implementation
* Resilience and error handling
* Code quality analysis
* MCP integration using popular servers

## Best Practices for Advanced Usage

1. **Context Management**

   * Keep related files open
   * Use clear, descriptive file names
   * Organize code logically
   * Configure MCP servers for project context

2. **Security First**

   * Always review generated code for vulnerabilities
   * Implement defense in depth
   * Use established security libraries

3. **Performance**

   * Measure before optimizing
   * Use async/await properly
   * Implement caching strategically

4. **Patterns**

   * Don't over-engineer
   * Use patterns that solve real problems
   * Keep implementations simple

## Expert-Level Challenges (Optional)

1. **Implement Event Sourcing:**

   ```csharp
   // Event sourcing for audit trail
   // Store all state changes as events
   // Rebuild state from event history
   ```

2. **Add CQRS Pattern:**

   ```csharp
   // Separate read and write models
   // Optimize queries independently
   // Use MediatR for command handling
   ```

3. **Microservices Patterns:**

   ```csharp
   // Service discovery
   // API gateway pattern
   // Distributed tracing
   ```

## Performance Tips

**Copilot Performance:**

* Close unnecessary files to reduce context
* Use specific comments for faster suggestions
* Break complex requests into smaller parts

**Code Performance:**

* Profile before optimizing
* Focus on hot paths
* Cache expensive operations

## Troubleshooting Advanced Scenarios

**Copilot suggestions too generic?**

* Provide more context in comments
* Reference specific patterns or libraries
* Include example usage

**Complex refactoring failing?**

* Break into smaller steps
* Test each change incrementally
* Use version control effectively

**Performance not improving?**

* Profile to find actual bottlenecks
* Check for hidden N+1 queries
* Verify caching is working correctly

**MCP not connecting?**

* Check server executable path
* Verify Node.js is installed for npm-based servers
* Check Visual Studio output window for errors
* Ensure firewall isn't blocking local connections

**Common MCP Server Issues:**

1. **Microsoft Learn MCP server not responding:**
   * Check internet connectivity (HTTP-based server requires online access)
   * Verify the URL is correct: `https://learn.microsoft.com/api/mcp`
   * Try restarting Visual Studio after configuration changes

2. **@microsoft.docs.mcp not recognized in Chat:**
   * Restart Visual Studio after creating/updating .mcp.json
   * Check .mcp.json syntax is valid JSON (use a JSON validator if needed)
   * Verify file is in correct location: `%USERPROFILE%\.mcp.json`
   * Ensure the server name matches exactly: `microsoft.docs.mcp`

3. **JSON configuration errors:**
   * Validate JSON syntax - missing commas or brackets are common issues
   * Use double quotes for all strings, not single quotes
   * Ensure proper nesting of objects

4. **Network/firewall issues:**
   * Corporate firewalls may block access to learn.microsoft.com
   * Check if you can access https://learn.microsoft.com in your browser
   * Contact IT if corporate proxy settings need configuration

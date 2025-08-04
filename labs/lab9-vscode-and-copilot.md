# Lab 9: VS Code Exclusive Copilot Features with C#/.NET - Hands-on Exercise

**Duration:** 50 minutes

## Prerequisites

* Visual Studio Code with GitHub Copilot extension enabled
* .NET 9.0 SDK installed
* C# Dev Kit extension for VS Code
* GitHub account with Copilot access
* Basic familiarity with ASP.NET Core

## Exercise Overview

Explore the latest VS Code exclusive Copilot features using C#/.NET 9.0, including advanced custom instructions, edit/iterate capabilities, local MCP integration, terminal auto-approval, and extension ecosystem integration while building a modern book library management API.

**Note:** As of August 2025, many features previously exclusive to VS Code (like basic workspace context awareness, Next Edit Suggestions, and MCP integration) are now available in Visual Studio 2022. This lab focuses on the advanced features that remain exclusive to VS Code: automatic instruction generation, conversation editing, terminal auto-approval, local MCP servers, and advanced extension integration.

---

## Part 1: Advanced Custom Instructions and Chat Modes (10 minutes)

### Step 1: Create New ASP.NET Core Web API Project

1. **Open VS Code and create new project:**

   ```cmd
   dotnet new webapi -n BookLibraryAPI -f net9.0
   cd BookLibraryAPI
   code .
   ```

2. **Open VS Code terminal** (Ctrl + `)

3. **Verify Copilot status:**

   * Check bottom-right status bar for Copilot icon
   * Ensure it shows "Ready" status

### Step 2: Auto-Generate Project-Specific Instructions

1. **Use VS Code's instruction generation feature:**

   * Open Command Palette (Ctrl + Shift + P)
   * Type: `Chat: Generate Instructions`
   * This VS Code exclusive feature analyzes your codebase and creates tailored instructions

2. **Review auto-generated instructions:**

   * VS Code will analyze the project structure, dependencies, and patterns
   * Creates context-aware guidance for Copilot
   * Notice how it identifies .NET 9.0 patterns and ASP.NET Core conventions

3. **Enhance the generated instructions:**

   * Open the generated `copilot-instructions.md` file (or create if not generated)
   * Add project-specific requirements:

   ```markdown
   # Copilot Instructions

   ## Project Context
   This is a .NET 9.0 ASP.NET Core Web API for a book library management system.

   ## Coding Standards
   When working with this C# project, always:
   - Use nullable reference types with proper null checking
   - Follow async/await best practices with ConfigureAwait(false)
   - Include comprehensive error handling with proper HTTP status codes
   - Use minimal APIs where appropriate for simple endpoints
   - Follow .NET 9.0 conventions and C# 12 features
   - Include XML documentation comments for public APIs
   - Use Entity Framework Core with proper DbContext patterns
   - Implement proper dependency injection patterns

   ## Architecture Preferences
   - Use service layer pattern for business logic
   - Implement repository pattern for data access
   - Use DTOs for API request/response models
   - Follow RESTful API conventions
   - Include proper validation attributes
   ```

### Step 3: Configure Custom Chat Modes

1. **Create custom chat mode for API development:**

   * Open VS Code Settings (Ctrl + ,)
   * Search for "chat modes"
   * Add custom chat mode configuration:

   ```json
   {
     "github.copilot.chat.customModes": {
       "api-architect": {
         "instructions": "You are an expert .NET API architect. Focus on scalable, maintainable API design patterns. Always consider performance, security, and best practices.",
         "allowedTools": ["workspace", "terminal"],
         "preferredModel": "gpt-4"
       },
       "code-reviewer": {
         "instructions": "You are a senior code reviewer. Focus on code quality, security vulnerabilities, performance issues, and adherence to coding standards.",
         "allowedTools": ["workspace"],
         "preferredModel": "claude"
       }
     }
   }
   ```

### Step 4: Test Advanced Chat Participants and Modes

1. **Open Copilot Chat** (Ctrl + Shift + I)

2. **Test custom chat mode:**

   ```
   /mode api-architect
   @workspace What's the optimal architecture for this book library API? Consider scalability, maintainability, and performance.
   ```

3. **Test workspace chat participant with multi-turn conversation:**

   ```
   @workspace What's the structure of this ASP.NET Core project? What files should I modify to create a book library API?
   ```

   **Follow up after Copilot responds:**

   ```
   @workspace Based on your suggestions, which approach would be better for scalability: keeping everything in controllers or implementing a service layer?
   ```

   **Continue the conversation:**

   ```
   @workspace Now show me how to implement that service layer with dependency injection, considering the existing project structure you analyzed.
   ```

4. **Switch to code review mode:**

   ```
   /mode code-reviewer
   @workspace Review the current project structure and identify any potential issues or improvements needed before we start adding features.
   ```

---

## Part 2: Multi-Model Support and Edit/Iterate Capabilities (10 minutes)

### Step 1: Understand Multi-Model Capabilities and Edit Features

1. **Open Command Palette** (Ctrl + Shift + P)

2. **Type:** `GitHub Copilot: Select Model`

   * Notice the available models (GPT-4, Claude, etc.)
   * This manual selection exists in VS Code, but automatic switching is the exclusive feature

3. **Test automatic model selection:**

   * Ask a complex architectural question and observe which model Copilot chooses
   * Ask a simple code completion question and see if a different model is used

4. **Explore edit/iterate features:**

   * VS Code exclusive: Edit previous chat requests
   * Multiple edit modes: inline, hover, input box
   * Right-click on previous chat messages to see "Edit" options

### Step 2: Create Project Structure with Iterative Refinement

1. **Create the suggested folder structure:**

   ```cmd
   mkdir Models Services Data Controllers DTOs
   ```

2. **Ask for code generation and practice editing requests:**

   ```
   @workspace Generate a Book model class for the Models folder with basic properties: Id, Title, Author.
   ```

3. **Edit your previous request (VS Code exclusive feature):**

   * Right-click on your previous message
   * Select "Edit Request"
   * Modify to: "Generate a Book model class for the Models folder with properties: Id, Title, Author, ISBN, PublishedDate, Genre, IsAvailable. Make it thread-safe and optimized for performance."
   * Observe how Copilot provides an enhanced response

4. **Practice conversation refinement:**

   * Use inline edit mode (hover over message and click edit icon)
   * Try input box edit mode from chat history
   * Notice how VS Code maintains conversation context through edits

### Step 3: Database Context with Advanced Conversation Management

1. **Add Entity Framework packages:**

   ```cmd
   dotnet add package Microsoft.EntityFrameworkCore.Sqlite
   dotnet add package Microsoft.EntityFrameworkCore.Tools
   ```

2. **Start with a basic request, then refine:**

   ```
   @workspace Create a LibraryContext class in the Data folder that works with the Book model.
   ```

3. **Edit and enhance your request (demonstrate VS Code's edit capabilities):**

   * Edit the previous request to add: "Use SQLite, include seed data for 5 sample books, implement proper indexes for search performance, and include audit trail capabilities."
   * Notice how the refined request produces a much more comprehensive response

4. **Continue with multi-turn conversation refinement:**

   **After implementation, continue the conversation:**

   ```
   @workspace The context class looks good, but I'm concerned about performance with large datasets. Can you suggest optimizations for the queries and indexing strategy?
   ```

   **Follow up with specific concerns:**

   ```
   @workspace How should I handle concurrent access and transactions in this context? Show me best practices for the specific scenarios in a library management system.
   ```

5. **Practice chat history management:**

   * Review your conversation history in the chat panel
   * Edit any previous request to see alternative responses
   * Use VS Code's chat session management to organize conversations

### Step 4: Conversation Tracking and Management (VS Code Exclusive)

1. **Explore VS Code's conversation features:**

   * View conversation history in the chat panel
   * Create new chat sessions for different topics
   * Export/import chat sessions for later reference

2. **Test conversation context preservation:**

   ```
   @workspace Based on our entire conversation about the Book model and LibraryContext, what would be the next logical step to implement?
   ```

3. **Use conversation branching:**

   * Create a new chat session
   * Reference previous conversation: "Continuing from our Book library discussion..."
   * Notice how VS Code maintains project context across sessions

---

## Part 3: Advanced Terminal Integration with Copilot (10 minutes)

### Step 1: Configure Terminal Auto-Approval (VS Code Exclusive)

1. **Open integrated terminal** (Ctrl + `)

2. **Configure terminal auto-approval settings:**

   * Open VS Code Settings (Ctrl + ,)
   * Search for "terminal agent"
   * Add configuration to settings.json:

   ```json
   {
     "terminal.agent.allowList": [
       "dotnet build",
       "dotnet run",
       "dotnet test",
       "dotnet add package*",
       "dotnet ef*"
     ],
     "terminal.agent.denyList": [
       "rm *",
       "del *",
       "format *"
     ],
     "terminal.agent.autoApproval": true
   }
   ```

3. **Test enhanced terminal integration:**

   ```cmd
   # Start typing and see Copilot suggestions with auto-approval
   dotnet add package 
   # Copilot should suggest relevant packages and auto-approve safe commands
   ```

### Step 2: Smart Workflow Integration

1. **Use terminal for intelligent scaffolding:**

   ```cmd
   # Let Copilot suggest complete commands with context awareness
   dotnet aspnet-codegenerator controller
   ```

2. **Create `Controllers/BooksController.cs` with terminal + chat integration:**

   * Start the controller file manually:

   ```csharp
   using Microsoft.AspNetCore.Mvc;
   using BookLibraryAPI.Models;
   using BookLibraryAPI.Data;

   namespace BookLibraryAPI.Controllers;

   [ApiController]
   [Route("api/[controller]")]
   public class BooksController : ControllerBase
   {
       // Use Copilot to generate CRUD methods
   ```

3. **Use terminal and chat together:**

   ```
   @workspace Generate complete CRUD methods for this BooksController with proper error handling, validation, and async patterns. Also provide the terminal commands I'll need to test each endpoint.
   ```

### Step 3: Advanced Terminal Workflows and Command Orchestration

1. **Test multi-terminal coordination:**

   * Open multiple terminals (Ctrl + Shift + `)
   * Terminal 1: Development server
   * Terminal 2: Testing commands
   * Terminal 3: Database operations

2. **Use Copilot for complex command sequences:**

   ```cmd
   # Copilot can suggest complete workflow commands
   dotnet build && dotnet ef database update && dotnet run
   ```

3. **Terminal-based API testing with AI assistance:**

   ```powershell
   # PowerShell with Copilot suggestions
   $baseUrl = "http://localhost:5000/api/books"
   # Let Copilot suggest complete test script
   ```

   **Alternative with curl:**

   ```cmd
   # Copilot suggests complete testing workflow
   curl -X POST http://localhost:5000/api/books -H "Content-Type: application/json" -d "{\"title\":\"Test Book\",\"author\":\"Test Author\"}"
   ```

### Step 4: Integrated Development Workflow

1. **Use terminal for advanced database operations:**

   ```cmd
   # Let Copilot guide through complex EF operations
   dotnet ef migrations add BookLibraryInitial
   dotnet ef database update
   # Copilot suggests meaningful migration names and checks for issues
   ```

2. **Terminal-integrated debugging:**

   * Start debugging from terminal: `dotnet run --launch-profile https`
   * Use terminal for live application monitoring
   * Ask Copilot for performance monitoring commands

3. **Automated testing workflows:**

   ```cmd
   # Copilot suggests comprehensive testing commands
   dotnet test --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults
   ```

   * Copilot provides explanation of test results
   * Suggests improvements based on coverage reports

---

## Part 4: Local MCP Integration and Workspace Intelligence (10 minutes)

### Step 1: Configure Local MCP Server for Development

1. **Set up a simple local MCP server (VS Code exclusive):**

   * Create a new file: `mcp-config.json` in your project root
   * Add local MCP server configuration:

   ```json
   {
     "mcpServers": {
       "dotnet-project": {
         "command": "node",
         "args": ["-e", "
           const server = require('child_process').spawn('dotnet', ['--info']);
           server.stdout.on('data', (data) => {
             console.log(JSON.stringify({
               type: 'tool_result',
               content: data.toString()
             }));
           });
         "],
         "name": "Local .NET Context"
       }
     }
   }
   ```

2. **Test MCP dynamic prompts (VS Code exclusive):**

   * Use slash commands to invoke MCP prompts:

   ```
   /mcp.dotnet-project.get-info
   ```

   * Notice how VS Code provides dynamic prompts based on project context

### Step 2: Advanced Workspace Intelligence

1. **Create a multi-root workspace:**
   * File → Add Folder to Workspace
   * Create a second project: `dotnet new classlib -n BookLibrary.Shared`
   * Add the shared library folder to workspace

2. **Test cross-project intelligent context:**

   ```
   @workspace Analyze the relationship between these two projects. How can I share the Book model between the API and the shared library? Show me the optimal project structure.
   ```

3. **Use workspace-wide refactoring with Copilot:**

   ```
   @workspace I want to move the Book model to the shared library and update all references. Show me the step-by-step process and identify all files that need changes.
   ```

### Step 3: Resource Interaction and Context Awareness

1. **Test VS Code's resource drag-and-drop (MCP exclusive):**

   * Take a screenshot of your current project structure
   * Drag the screenshot into the Copilot chat
   * Ask: `@workspace Based on this project structure screenshot, what improvements would you suggest?`

2. **Advanced symbol navigation and context:**

   ```
   @vscode Show me all references to the Book class across the entire workspace and help me understand the dependency graph. Include suggestions for decoupling if needed.
   ```

3. **Live project analysis:**

   ```
   @workspace Analyze all the files currently open in tabs and suggest how they should work together. Are there any missing pieces or potential issues?
   ```

### Step 4: Debugging Integration with AI Context

1. **Set up advanced debugging scenario:**

   ```csharp
   // Add this method to your BooksController
   [HttpGet("debug-info")]
   public async Task<IActionResult> GetDebugInfo()
   {
       var debugData = new
       {
           Timestamp = DateTime.UtcNow,
           MachineName = Environment.MachineName,
           ProcessId = Environment.ProcessId,
           Books = await _context.Books.Take(3).ToListAsync()
       };
       
       // Set a breakpoint here
       return Ok(debugData);
   }
   ```

2. **Use Copilot for debugging assistance:**

   ```
   @vscode Help me set up advanced debugging configuration for this API. I need to debug both the web API and understand the Entity Framework queries. Include logging configuration.
   ```

3. **Test VS Code's integrated debugging with AI:**

   * Set breakpoints and use F5 to debug
   * When stopped at breakpoint, use Debug Console
   * Type: `@vscode explain the current variable state and suggest what to check next`
   * Ask Copilot to analyze variable values and suggest debugging steps

---

## Part 5: Extension Ecosystem Integration (8 minutes)

### Step 1: C# Dev Kit Integration with Copilot

1. **Explore Solution Explorer integration:**

   * Open the Solution Explorer in VS Code
   * Notice how Copilot provides enhanced context about the entire solution structure
   * Right-click on project files to see Copilot-enhanced options

2. **Test cross-platform development assistance:**

   ```
   @workspace Help me configure this project to run optimally on both Windows and Linux. What changes do I need to make to the csproj, configuration, and deployment settings?
   ```

3. **Use C# Dev Kit specific features with Copilot:**

   * Test IntelliSense enhancements
   * Use "Go to Definition" across projects
   * Leverage enhanced debugging capabilities

### Step 2: Language-Specific Copilot Participants

1. **Test C# specific chat participants (if available):**

   ```
   @csharp How can I optimize this Entity Framework query for better performance? Show me both LINQ and raw SQL approaches.
   ```

2. **Explore extension-provided context:**

   * Install additional VS Code extensions (like REST Client)
   * Test how Copilot integrates with different extension contexts
   * Create `.http` files and ask Copilot for test requests

3. **Multi-extension workflow:**

   ```
   @workspace I'm using the REST Client extension. Generate a comprehensive set of HTTP requests to test all the API endpoints we've created, including error scenarios.
   ```

### Step 3: Advanced Code Navigation and Refactoring

1. **Use enhanced symbol navigation:**

   * Press Ctrl + T (Go to Symbol in Workspace)
   * Type symbols and see how Copilot enhances the search
   * Notice VS Code's superior cross-file navigation compared to VS 2022

2. **Multi-file editing with Copilot assistance:**

   ```
   @workspace Open all related files for the Book entity (model, controller, context) and help me refactor them to use a more modern record-based approach with init-only properties.
   ```

3. **Workspace-wide refactoring:**

   ```
   @workspace I want to implement a generic repository pattern across this project. Show me which files need to be created, modified, and how to update the dependency injection configuration.
   ```

---

## Part 6: Advanced Development Workflows and Integration (8 minutes)

### Step 1: Integrated Source Control with AI

1. **Use VS Code's Git integration with Copilot:**

   * Stage your changes in the Source Control panel
   * Click the "Generate commit message" sparkle icon
   * Compare AI-generated messages with your own descriptions

2. **Advanced branch management:**

   ```
   @vscode Help me create a feature branch for adding user authentication to this API. Include the Git commands and explain the workflow for integrating with the main branch.
   ```

3. **Code review preparation:**

   ```
   @workspace Analyze all my staged changes and create a comprehensive pull request description that explains what was implemented, why, and what should be tested.
   ```

### Step 2: Performance Analysis and Optimization

1. **Use Copilot for performance insights:**

   ```
   @workspace Analyze this API project for potential performance bottlenecks. Focus on Entity Framework queries, controller efficiency, and memory usage patterns.
   ```

2. **Profiling integration:**

   * Install a performance profiling extension
   * Ask Copilot to interpret profiling results
   * Get suggestions for optimization strategies

3. **Code quality assessment:**

   ```
   @workspace Review this codebase for security vulnerabilities, code smells, and adherence to .NET best practices. Provide a prioritized list of improvements.
   ```

### Step 3: Local Testing and Quality Assurance

1. **Create comprehensive test suite:**

   ```cmd
   # Create test project with Copilot assistance
   dotnet new xunit -n BookLibraryAPI.Tests
   cd BookLibraryAPI.Tests
   dotnet add reference ..\BookLibraryAPI.csproj
   dotnet add package Microsoft.AspNetCore.Mvc.Testing
   ```

2. **Generate tests with workspace context:**

   ```
   @workspace Generate comprehensive integration tests for the BooksController that cover all CRUD operations, error scenarios, edge cases, and performance considerations. Include setup and teardown methods.
   ```

3. **Test analysis and reporting:**

   ```cmd
   # Run tests with coverage analysis
   dotnet test --collect:"XPlat Code Coverage" --logger trx --results-directory TestResults
   ```

   ```
   @workspace Analyze these test results and coverage reports. What areas need more testing coverage and what types of tests should I add?
   ```

---

## Key Takeaways

✅ **VS Code exclusive features mastered:**

* Advanced custom instruction generation with `Chat: Generate Instructions`
* Custom chat modes with specific tools and model preferences
* Edit/iterate capabilities for refining conversations
* Local MCP server integration with dynamic prompts
* Terminal auto-approval with configurable allow/deny lists
* Extension ecosystem integration with context-aware assistance
* Multi-root workspace intelligence and cross-project analysis
* Resource drag-and-drop for visual context integration

## VS Code Specific Advantages (as of August 2025)

1. **Advanced AI Customization:**

   * Auto-generate project-specific instructions
   * Create custom chat modes with tailored contexts
   * Edit and refine previous conversations seamlessly
   * Automatic model switching based on task complexity

2. **Local Development Excellence:**

   * Local MCP servers without GitHub dependency
   * Terminal auto-approval for trusted commands
   * Multi-terminal coordination with AI assistance
   * Advanced debugging integration with AI insights

3. **Extension Ecosystem Integration:**

   * Deep integration with C# Dev Kit and other extensions
   * Language-specific chat participants
   * Cross-extension workflow optimization
   * Superior multi-root workspace management

## Best Practices for Advanced VS Code + Copilot

1. **Leverage Advanced Chat Features:**

   * Use auto-generated instructions for consistent AI behavior
   * Create custom chat modes for different development phases
   * Edit previous requests to refine and improve responses
   * Combine multiple chat participants for comprehensive solutions

2. **Optimize Terminal Workflows:**

   * Configure auto-approval for safe, repetitive commands
   * Use multi-terminal setups for complex development workflows
   * Integrate terminal and chat for seamless development
   * Let Copilot guide database and testing operations

3. **Maximize Extension Integration:**

   * Use C# Dev Kit features with Copilot enhancement
   * Install complementary extensions for richer context
   * Leverage cross-platform development optimizations
   * Utilize advanced debugging and profiling with AI assistance

## Advanced Challenges (Optional)

1. **Cross-Platform Deployment:**

   ```
   @workspace Set up this API for deployment to both Windows and Linux containers, with automated CI/CD pipeline configuration
   ```

2. **Advanced Terminal Workflows:**

   ```
   @workspace Create complex build and deployment scripts that work across different platforms, leveraging VS Code's terminal integration
   ```

3. **Multi-Root Workspace Project:**

   ```
   @workspace Help me organize this into a multi-root workspace with separate projects for API, shared libraries, and testing infrastructure
   ```

## Troubleshooting Advanced VS Code Features

**Custom instructions not being applied?**

* Ensure the `copilot-instructions.md` file is in the correct location
* Check that the file is saved and VS Code has indexed it
* Try restarting VS Code to reload instruction files
* Verify instruction format follows markdown conventions

**Chat modes not available or not working?**

* Ensure you have the latest Copilot extension (v1.102+)
* Check your settings.json syntax is valid
* Restart VS Code after adding custom chat modes
* Verify your Copilot subscription supports advanced features

**Edit/iterate features not appearing?**

* Update to VS Code v1.102 or later
* Right-click on chat messages to see edit options
* Ensure experimental features are enabled in Copilot settings
* Try hovering over messages to see inline edit icons

**Local MCP servers not connecting?**

* Check that Node.js is installed and accessible
* Verify MCP server configuration syntax in settings
* Check VS Code Developer Console for MCP error messages
* Ensure MCP server scripts have proper permissions

**Terminal auto-approval not working?**

* Verify terminal agent settings in VS Code preferences
* Check that commands match your allow list patterns
* Ensure auto-approval is explicitly enabled
* Restart terminal sessions after configuration changes

**Extension integration issues?**

* Update all extensions to latest versions
* Check for extension conflicts in the Extensions panel
* Verify C# Dev Kit is properly configured
* Restart VS Code after installing new extensions

---

**Congratulations!** You've mastered the advanced features that make VS Code unique for GitHub Copilot development with C#/.NET 9.0. These cutting-edge capabilities—including automatic instruction generation, conversation editing, local MCP integration, terminal auto-approval, and deep extension ecosystem integration—demonstrate why VS Code remains the leading choice for AI-enhanced, cross-platform development workflows.

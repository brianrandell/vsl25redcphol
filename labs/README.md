# GitHub Copilot Hands-On Lab Instructions

This folder contains all the step-by-step lab instructions for the GitHub Copilot training at VSLIVE! 2025 Redmond. Each lab provides detailed exercises to help you master different aspects of GitHub Copilot integration with Visual Studio 2022 and Visual Studio Code.

**Updated August 2025** with the latest GitHub Copilot features including:

* Models and pricing guidance (GPT-4o, Claude Sonnet 4, Claude Opus 4)
* Ask Mode vs Agent Mode demonstrations
* Next Edit Suggestions for productivity
* AI-powered debugging techniques
* MCP server integration with Microsoft Learn and other popular servers

## Table of Contents

### Core Visual Studio 2022 Labs

| Lab | Title | Duration | Focus Area | Prerequisites |
|-----|-------|----------|------------|---------------|
| [1](lab1-copilot-fundamentals.md) | **Copilot Fundamentals** | 25 minutes | Basic Copilot features, code completion, comment-driven development | VS 2022, GitHub Copilot |
| [2](lab2-code-generation.md) | **Advanced Code Generation** | 30 minutes | Complex data structures, business logic, file I/O patterns | Lab 1 complete |
| [3](lab3-copilot-chat.md) | **Copilot Chat Integration** | 25 minutes | Chat-driven development, API design, error handling | Lab 2 complete |
| [4](lab4-testing-copilot.md) | **Test-Driven Development** | 30 minutes | Unit testing, integration testing, test data builders | Lab 3 complete |
| [5](lab5-debugging-copilot.md) | **Debugging with Copilot** | 20 minutes | Exception analysis, breakpoints, performance debugging | Lab 4 complete |
| [6](lab6-database-sql.md) | **Database and SQL Development** | 35 minutes | Entity Framework Core, migrations, repository patterns | Lab 5 complete |
| [7](lab7-wpf-desktop.md) | **WPF Desktop Development** | 30 minutes | MVVM pattern, data binding, desktop UI design | Lab 6 complete |
| [8](lab8-advanced-copilot.md) | **Advanced Copilot Techniques** | 45 minutes | Enterprise patterns, security, performance optimization, MCP servers | Lab 7 complete |

### Visual Studio Code Exclusive Labs

| Lab | Title | Duration | Focus Area | Prerequisites |
|-----|-------|----------|------------|---------------|
| [9](lab9-vscode-and-copilot.md) | **VS Code Exclusive Features (C#)** | 35 minutes | Agent Mode, Next Edit Suggestions, MCP integration | VS Code, C# Dev Kit, GitHub Copilot |

### GitHub Platform Labs

| Lab | Title | Duration | Focus Area | Prerequisites |
|-----|-------|----------|------------|---------------|
| [10](lab10-github-copilot-agent.md) | **GitHub Copilot Coding Agent** | 35 minutes | Autonomous AI development, issue-driven coding | GitHub Copilot Pro, VS Code |

## Lab Descriptions

### **Lab 1: Copilot Fundamentals**

Learn the basics of GitHub Copilot in Visual Studio 2022. Create a simple calculator console application while exploring fundamental features like code completion, inline suggestions, and comment-driven development. Perfect starting point for Copilot newcomers.

**What You'll Build**: Console calculator with basic arithmetic operations  
**Key Skills**: Code completion, inline suggestions, comment-to-code generation

### **Lab 2: Advanced Code Generation**

Dive deeper into Copilot's code generation capabilities by building a feature-rich todo list manager. Learn to generate complex data structures, business logic, and file I/O operations using advanced prompting techniques.

**What You'll Build**: Todo list manager with priorities and JSON persistence  
**Key Skills**: Complex data structures, LINQ operations, service patterns

### **Lab 3: Copilot Chat Integration**

Discover the power of Copilot Chat for interactive development. Build a RESTful API with comprehensive error handling while learning to leverage chat for architecture decisions and code explanations.

**What You'll Build**: Product Inventory REST API with full CRUD operations  
**Key Skills**: API design, DTOs, error handling, Swagger integration

### **Lab 4: Test-Driven Development**

Master test-driven development with Copilot assistance. Create comprehensive test suites including unit tests, integration tests, and test data builders using xUnit and FluentAssertions.

**What You'll Build**: Complete testing suite for Product Inventory API  
**Key Skills**: Unit testing, integration testing, test data patterns

### **Lab 5: Debugging with Copilot**

Learn how to use GitHub Copilot to assist with debugging, analyze exceptions, and optimize performance issues. Master AI-powered breakpoints, stack trace analysis, and real-time debugging assistance.

**What You'll Build**: Debug and fix a buggy order processing system  
**Key Skills**: Exception analysis, AI breakpoints, performance debugging

### **Lab 6: Database and SQL Development**

Learn database integration using Entity Framework Core with Copilot. Implement repository patterns, database migrations, and data seeding while exploring Copilot's SQL generation capabilities.

**What You'll Build**: API with Entity Framework Core and SQL Server  
**Key Skills**: EF Core, migrations, repository pattern, database seeding

### **Lab 7: WPF Desktop Development**

Explore desktop application development using WPF and MVVM patterns with Copilot assistance. Build a rich desktop expense tracker with data binding, validation, and local data persistence.

**What You'll Build**: WPF expense tracking desktop application  
**Key Skills**: MVVM pattern, data binding, WPF controls, value converters

### **Lab 8: Advanced Copilot Techniques**

Master enterprise-level development with advanced Copilot techniques. Implement security patterns, performance optimization, advanced design patterns, and integrate with MCP servers for extending Copilot capabilities.

**What You'll Build**: Enterprise-grade API with advanced patterns and MCP integration  
**Key Skills**: Security, performance, design patterns, MCP server integration, monitoring

### **Lab 9: VS Code Exclusive Features**

Explore cutting-edge Copilot features available exclusively in Visual Studio Code. Learn Agent Mode, Next Edit Suggestions, and MCP (Model Context Protocol) integration for enhanced development experiences.

**What You'll Build**: Book Library API with VS Code exclusive features  
**Key Skills**: Agent Mode, Next Edit Suggestions, MCP servers

### **Lab 10: GitHub Copilot Coding Agent**

Experience the future of AI-driven development with GitHub's Copilot Coding Agent. Learn to create effective GitHub issues for autonomous AI development and review AI-generated pull requests.

**What You'll Build**: Interactive calculator enhanced by AI agent  
**Key Skills**: Issue writing, AI delegation, code review, autonomous development

## Getting Started

### Prerequisites

* **Visual Studio 2022** (any edition) with GitHub Copilot extension
* **Visual Studio Code** with C# Dev Kit and GitHub Copilot (for Labs 8-9)
* **GitHub account** with active Copilot subscription
* **GitHub Copilot Pro** (required for Lab 9)
* **Basic C# knowledge** recommended

### Recommended Lab Sequence

**For Visual Studio 2022 Focus:**

1. Complete Labs 1-8 in sequence
2. Optional: Explore Labs 9-10 for VS Code comparison

**For Multi-IDE Experience:**

1. Complete Labs 1-5 in Visual Studio 2022
2. Switch to Lab 9 in VS Code for exclusive features
3. Continue with Lab 10 for GitHub.com integration
4. Return to Labs 6-8 in Visual Studio 2022

**For Advanced Users:**

* Jump to any lab of interest
* Each lab includes prerequisite information
* Completed solutions available in `/completed` folder

## Lab Structure

Each lab follows a consistent structure:

* **Duration**: Estimated completion time
* **Prerequisites**: Required software and prior lab completion
* **Exercise Overview**: Learning objectives and outcomes
* **Step-by-step Instructions**: Detailed walkthrough with screenshots
* **Key Takeaways**: Summary of concepts learned
* **Next Steps**: Guidance for continued learning

## Support Resources

### During Labs

* **Completed Solutions**: Reference implementations in `/completed` folder
* **Presenter Notes**: Instructor guidance available in `/notes` folder
* **Troubleshooting**: Common issues and solutions included in each lab

### Additional Resources

* [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
* [Visual Studio Copilot Guide](https://docs.microsoft.com/en-us/visualstudio/ide/visual-studio-github-copilot-extension)
* [VS Code Copilot Documentation](https://code.visualstudio.com/docs/editor/github-copilot)

## Feedback and Questions

This is a hands-on training experience designed for interactive learning. Don't hesitate to:

* Experiment with different Copilot prompts
* Ask questions during the session
* Share interesting discoveries with fellow attendees
* Provide feedback on the lab content

---

**Event**: VSLIVE! 2025 Redmond  
**Session**: Getting the Most out of GitHub Copilot in Visual Studio 2022  
**Presenter**: Brian A. Randell (@brianrandell) and Microsoft Visual Studio Team  
**Date**: August 4, 2025

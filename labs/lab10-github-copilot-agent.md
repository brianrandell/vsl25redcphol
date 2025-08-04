# Lab 10: GitHub Copilot Coding Agent - Hands-on Exercise

**Duration:** 35 minutes

## Prerequisites

* Visual Studio Code (latest version) installed
* GitHub account with **Copilot Pro** subscription (required for Coding Agent features)
* GitHub Pull Requests and Issues extension for VS Code
* Basic familiarity with C# and .NET development
* .NET 9.0 SDK installed

## Exercise Overview

In this lab, you'll explore GitHub's new Copilot Coding Agent (currently in preview), which can autonomously work on GitHub issues and create pull requests. You'll start by creating a simple Hello World application, then assign enhancement tasks to Copilot and monitor its autonomous development work.

> **Important Note:** This lab focuses on the **GitHub Copilot Coding Agent**, which is a GitHub-based agent that works autonomously on issues and creates pull requests. This is different from the **"Agent Mode"** in VS Code (covered in Lab 3), which provides interactive, in-editor assistance for complex tasks. The GitHub Coding Agent operates independently on the GitHub platform, while Agent Mode is an interactive feature within your development environment.

---

## Part 1: Verify Copilot Agent Access and Create Repository (8 minutes)

### 1.1 Verify Copilot Pro and Coding Agent Access

1. **Go to GitHub.com and sign in**

   * Navigate to [github.com](https://github.com) and ensure you're logged in
   * Click on your profile picture → Settings

2. **Verify Copilot Pro subscription:**

   * In the left sidebar, click "Copilot"
   * Confirm you have "GitHub Copilot Pro" (not just the free tier)
   * If you don't have Pro, you'll need to upgrade to access the Coding Agent

3. **Check Coding Agent availability:**

   * Look for "Coding Agent" or "@copilot" features in your settings
   * Note: This feature is in preview and may require joining a wait list

### 1.2 Create a New GitHub Repository

1. **Create a new repository:**

   * Click the "+" icon in the top-right corner → "New repository"
   * Repository name: `copilot-calculator-agent`
   * Description: "Calculator app enhanced by GitHub Copilot Coding Agent"
   * Make it **Public** or **Private**
   * Check "Add a README file"
   * Add `.gitignore` template: select "VisualStudio"
   * Click "Create repository"

2. **Clone the repository locally:**

   * Click the green "Code" button and copy the HTTPS URL
   * Open VS Code
   * Open Command Palette (`Ctrl+Shift+P` or `Cmd+Shift+P`)
   * Run `Git: Clone`
   * Paste your repository URL
   * Choose a local folder to clone into
   * Click "Open" when prompted to open the cloned repository

### 1.3 Create Initial Hello World Application

1. **Create a simple console application:**

   * Open the integrated terminal in VS Code (`Ctrl+`` ` or `Cmd+`` `)
   * Run the following commands:

      ```cmd
      dotnet new console -n CalculatorApp
      cd CalculatorApp
      dotnet run
      ```

2. **Verify the application works:**

   * You should see "Hello, World!" output
   * This confirms .NET is working correctly

3. **Commit the initial code:**

   ```cmd
   cd ..
   git add .
   git commit -m "Initial Hello World console application"
   git push origin main
   ```

---

## Part 2: Create Calculator Enhancement Issue (7 minutes)

### 2.1 Create Your First Issue for Copilot

1. **Navigate to your GitHub repository** in a browser

   * Go to your `copilot-calculator-agent` repository on GitHub.com

2. **Create Issue #1 - Enhance Calculator:**

   * Click the "Issues" tab → "New issue"
   * Title: `Transform Hello World into Interactive Calculator`
   * Body (this is a block of markdown):

   ``` markdown
   ## Task Description
   Transform the existing Hello World console application into an interactive calculator with the following features:
   
   ## Required Calculator Operations
   
   *   Addition (+)
   *   Subtraction (-)
   *   Multiplication (*)
   *   Division (/)
   *   Modulus (%)
   
   ## Technical Requirements
   
   *   Update the existing `CalculatorApp/Program.cs` file
   *   Use a menu-driven interface that loops until user chooses to exit
   *   Handle invalid input gracefully with try-catch blocks
   *   Display results clearly with proper formatting
   *   Add input validation for division by zero
   *   Use modern C# 12 features where appropriate
   *   Follow .NET 9.0 conventions and best practices
   
   ## User Experience Requirements
   
   *   Welcome message explaining how to use the calculator
   *   Clear menu options (1=Add, 2=Subtract, 3=Multiply, 4=Divide, 5=Modulus, 0=Exit)
   *   Prompt for two numbers for each operation
   *   Display calculation result clearly
   *   Return to main menu after each operation
   *   Graceful exit message
   
   ## Example Interaction   
   ```

   ``` shell
   Welcome to the Interactive Calculator!

   Please choose an operation:
   1. Addition
   2. Subtraction  
   3. Multiplication
   4. Division
   5. Modulus
   0. Exit

   Enter your choice: 1
   Enter first number: 10
   Enter second number: 5
   Result: 10 + 5 = 15

   [Return to main menu...]
   ```

   ``` markdown
   ## Acceptance Criteria

   *   [ ] Console application compiles and runs without errors
   *   [ ] All five mathematical operations work correctly
   *   [ ] Input validation handles invalid entries appropriately
   *   [ ] Division by zero is handled with appropriate error message
   *   [ ] User can perform multiple calculations in one session
   *   [ ] Clean exit when user chooses option 0
   *   [ ] Code follows C# naming conventions and includes appropriate comments
   ```

   * Add labels: `enhancement`, `copilot-ready`
   * Click "Submit new issue"

---

## Part 3: Setup VS Code and Assign Issue to Copilot (8 minutes)

### 3.1 Configure VS Code for Copilot Agent

1. **Install required extensions in VS Code:**

   * Ensure you have the **GitHub Pull Requests and Issues** extension installed (if not, install it)
   * Verify **GitHub Copilot** extension is active and signed in

2. **Enable Coding Agent UI Integration:**

   * Open VS Code Settings (`Ctrl+,` or `Cmd+,`)
   * Search for "coding agent"
   * Add the following setting to your `settings.json`:

   ```json
   {
     "githubPullRequests.codingAgent.uiIntegration": true
   }
   ```

3. **Verify GitHub authentication:**

   * Open Command Palette (`Ctrl+Shift+P` or `Cmd+Shift+P`)
   * Run `GitHub: Sign in`
   * Ensure you're signed in with your Copilot Pro account

### 3.2 Assign Calculator Issue to Copilot

1. **Method 1: Assign from GitHub.com (Recommended):**

   * Go back to your issue #1 on GitHub.com
   * In the right sidebar, click on "Assignees"
   * Type `@copilot` and select it
   * Alternatively, comment on the issue: `@copilot please implement this calculator enhancement`

2. **Method 2: Assign from VS Code (if available):**

   * Open the GitHub panel in VS Code (click GitHub icon in Activity Bar)
   * Look for the Issues section
   * Find issue #1 "Transform Hello World into Interactive Calculator"
   * Right-click and select "Assign to Copilot" (if this option is available)

3. **Monitor Copilot's work:**

   * After assigning, Copilot will start working on the issue
   * Watch for Copilot's comments on the issue showing its progress
   * GitHub will show activity as Copilot analyzes and plans the work

### 3.3 Observe Copilot Agent in Action

1. **Watch the issue page:**

   * Copilot may comment with its plan or ask clarifying questions
   * You'll see status updates as it works through the implementation

2. **Wait for pull request creation** (typically 3-8 minutes):

   * Copilot will create a new branch and implement the solution
   * A pull request will be automatically created
   * The PR will include detailed descriptions and potentially screenshots

3. **Track progress in VS Code (optional):**

   * If you have the integration enabled, check the "Copilot on My Behalf" section
   * This shows all activities Copilot is performing for you

---

## Part 4: Review and Test Copilot's Work (8 minutes)

### 4.1 Review the Pull Request

1. **Navigate to the Pull Request:**

   * Go to your repository on GitHub.com
   * Click on "Pull requests" tab
   * Look for the PR created by Copilot (usually titled something like "Implement interactive calculator...")

2. **Review Copilot's implementation:**

   * Click on the PR to view details
   * Review the "Files changed" tab to see the code modifications
   * Check if Copilot:
       * Updated the `CalculatorApp/Program.cs` file correctly
       * Implemented all five mathematical operations
       * Added proper error handling and input validation
       * Followed the requirements from the issue

3. **Read Copilot's PR description:**

   * Notice how Copilot explains what it implemented
   * Look for any screenshots or detailed explanations
   * Check if it addresses all the acceptance criteria

### 4.2 Test the Implementation Locally

1. **Pull the changes to test locally:**

   * In VS Code terminal, fetch the latest changes:

   ```cmd
   git fetch origin
   git checkout [branch-name-from-PR]
   ```

2. **Run the enhanced calculator:**

   ```cmd
   cd CalculatorApp
   dotnet run
   ```

3. **Test the functionality:**

   * Try each mathematical operation (1-5)
   * Test input validation with invalid entries
   * Test division by zero handling
   * Verify the exit functionality (option 0)

### 4.3 Provide Feedback and Iterate

1. **If the implementation is good:**

   * Leave an approving comment: "Great work! This meets all requirements."
   * Merge the pull request
   * Verify the issue is automatically closed

2. **If improvements are needed:**

   * Comment on specific lines in the PR with suggestions
   * Example: "Please add more descriptive error messages for invalid input"
   * Or comment on the issue: `@copilot Please improve the error handling as discussed in the PR comments`

3. **Watch Copilot iterate:**

   * Copilot may push additional commits to address your feedback
   * It can update the same PR based on your suggestions

---

## Part 5: Create Additional Enhancement Issue (4 minutes)

### 5.1 Create a Second Issue for Advanced Features

1. **Create Issue #2 - Add Calculator History:**

   * Go back to your repository Issues page
   * Click "New issue"
   * Title: `Add Calculation History and Memory Features`
   * Body:

   ```markdown
   ## Task Description
   Enhance the interactive calculator with history and memory features:
   
   ## New Features Required
   
   *   **Calculation History**: Store and display the last 10 calculations
   *   **Memory Functions**: Add M+, M-, MR (Memory Recall), MC (Memory Clear)
   *   **History Command**: Add option 'H' to view calculation history
   *   **Clear History**: Add option 'C' to clear history
   *   **Save History**: Optionally save history to a text file on exit
   
   ## Technical Requirements
   
   *   Create a `CalculatorHistory` class to manage history
   *   Use a `List<string>` or similar collection for storing calculations
   *   Add memory variable for M+, M-, MR, MC operations
   *   Update the main menu to include new options:
       *   H - View History
       *   C - Clear History  
       *   M - Memory Menu (M+, M-, MR, MC)
   *   Preserve existing functionality from Issue #1
   
   ## Acceptance Criteria
   
   *   [ ] Calculator maintains history of last 10 calculations
   *   [ ] History displays in clear, readable format
   *   [ ] Memory functions work correctly (M+, M-, MR, MC)
   *   [ ] New menu options integrate seamlessly with existing interface
   *   [ ] All original calculator functions continue to work
   *   [ ] Code is well-organized with appropriate classes/methods
   ```

   * Add labels: `enhancement`, `feature-request`
   * Click "Submit new issue"

2. **Assign to Copilot (Optional):**

   * If time permits, assign this issue to `@copilot` as well
   * This demonstrates how Copilot can work on multiple related issues

---

## Part 6: Best Practices and Troubleshooting (4 minutes)

### 6.1 Writing Effective Issues for Copilot

**Good practices:**

* Be specific about requirements and acceptance criteria
* Include technical details (frameworks, patterns, conventions)
* Use clear, actionable language
* Break complex tasks into smaller issues
* Provide context about the existing codebase

**Example of a well-written issue:**

```markdown
## Task: Add Configuration Management
Create a configuration system for the console application using the Options pattern.

### Technical Requirements:

* Use Microsoft.Extensions.Configuration
* Support appsettings.json and environment variables
* Create strongly-typed configuration classes
* Implement in Program.cs with dependency injection

### Acceptance Criteria:

* [ ] Configuration loads from appsettings.json
* [ ] Environment variables override file settings
* [ ] Configuration is available via DI container
* [ ] Follows .NET configuration best practices
```

### 6.2 Common Issues and Solutions

1. **Copilot Agent not available:**

   * Verify you have Copilot Pro subscription
   * Check that the feature is enabled in your organization settings
   * Ensure VS Code extensions are up to date

2. **Agent taking too long:**

   * Complex issues may take 5-10 minutes
   * Check GitHub for any error messages from Copilot
   * Simplify the issue if it's too complex

3. **Poor quality output:**

   * Review your issue description for clarity
   * Add more specific requirements and examples
   * Provide feedback on PRs to improve future results

### 6.3 Integration with Development Workflow

1. **Use labels effectively:**

   * `copilot-ready` for issues suitable for agent assignment
   * `needs-human-review` for complex architectural decisions
   * `good-first-issue` for simple tasks

2. **Combine human and AI work:**

   * Use Copilot for boilerplate and implementation
   * Human review for architecture and design decisions
   * Iterate together for best results

---

## Conclusion and Next Steps

### What You've Learned

* How to set up and configure GitHub Copilot Coding Agent in VS Code
* How to create effective issues for autonomous AI development
* How to assign issues to Copilot and monitor progress
* How to review and provide feedback on AI-generated code
* Best practices for integrating AI agents into development workflows

### Key Takeaways

* The Coding Agent excels at well-defined implementation tasks
* Clear, specific issue descriptions lead to better results
* Human oversight and feedback improve AI output quality
* The feature integrates seamlessly with existing GitHub workflows

### Next Steps

1. **Experiment with different types of issues** - try documentation, refactoring, or feature additions
2. **Integrate with your team workflow** - establish guidelines for when to use the coding agent
3. **Combine with other Copilot features** - use Chat, completions, and the agent together
4. **Provide feedback to GitHub** - report bugs and suggest improvements as the feature is in preview

### Resources for Further Learning

* [GitHub Copilot Coding Agent Documentation](https://docs.github.com/en/copilot/concepts/coding-agent/coding-agent)
* [VS Code Copilot Agent Integration](https://code.visualstudio.com/blogs/2025/07/17/copilot-coding-agent)
* [GitHub Copilot Pro Features](https://docs.github.com/en/copilot/copilot-business)
* [Best Practices for AI-Assisted Development](https://docs.github.com/en/copilot/using-github-copilot)

---

**Lab Duration:** 30 minutes
**Difficulty:** Intermediate
**Prerequisites:** Copilot Pro subscription, VS Code, GitHub repository access

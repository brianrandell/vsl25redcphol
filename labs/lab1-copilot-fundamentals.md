# Lab 1: GitHub Copilot Fundamentals - Hands-on Exercise

**Duration:** 35 minutes

## Prerequisites

* Visual Studio 2022 (any edition, latest version) installed
* GitHub account with Copilot access (free tier, trial, or any paid plan)
* Basic familiarity with C# syntax

## Exercise Overview

In this lab, you'll create a simple calculator console application while learning the fundamental features of GitHub Copilot in Visual Studio 2022.

---

## Part 0: Understanding Copilot Models and Pricing (10 minutes)

### Understanding Copilot Plans

1. Navigate to [Plans for GitHub Copilot](https://docs.github.com/en/copilot/get-started/plans)

2. **Understand plan differences:**

* **Free tier**: Limited completions per month
* **Pro ($10/month)**: Unlimited completions, access to premium models
* **Pro+ ($39/month)**: Access to all models, increased usage limits
* **Business ($19/user/month)**: Team features + usage analytics
* **Enterprise ($39/user/month)**: Advanced security and compliance

### Monitoring Usage

1. **View usage statistics:**

* Click the **Copilot Badge** -> **Copilot Consumptions**
* Visit https://github.com/settings/billing for billing history.
* Visit https://github.com/settings/copilot/features to see your Premium requests usage and to control feature enablement

2. **Understand billing implications:**

* Different models may have different rate limits
* Business plans include usage analytics
* Enterprise plans offer more detailed reporting

---

## Part 1: Create a New Console Application (3 minutes)

> **TIP** When working with any-AI enabled tool, it's important to have backups. In the case of coding tools, this means using version control. While we don't explicitly cover version control in these labs, it is highly recommended to use GitHub or another version control system to track your changes and have a backup of your labs as you work. You don't have to use an online service, you can just use a local Git repository if you prefer.

1. **Open Visual Studio 2022**

2. **Verify Copilot is active:**

   * Look for the Copilot badge in the top-right corner of the IDE with a green checkmark.
   * If Copilot is not active, click the icon and sign in with your GitHub account

3. **Create a new project:**

   * Select `File | New | Project`
   * If necessary search for "Console App" in the project template search box
   * Select `Console App` (make sure it shows C# and .NET)
   * Click `Next`

4. **Configure your project:**

   * Project name: `CopilotCalculator`
   * Location: Choose your preferred directory

      > You might want to create a specific parent folder for all these labs like `c:\cplabs`.

   * Ensure "Place solution and project in the same directory" is checked
   * Click `Next`

5. **Select framework:**

   * Choose `.NET 9.0 (Standard Term Support)`
   * Click `Create`

---

## Part 2: Practice Basic Copilot Suggestions (5 minutes)

1. **Clear the default code** in `Program.cs` and start with an empty file

2. **Type the following comment and press Enter:**

   ```csharp
   // Create a simple calculator console application
   ```

3. **Observe Copilot's suggestion:**

* You should see grayed-out, 'ghost text' code suggestions appear
* Press `Tab` to accept the suggestion
* Press `Esc` to reject a suggestion

4. **Practice cycling through suggestions:**

* Type: `// Method to add two numbers`
* When Copilot shows a suggestion, press `Alt + ]` to see the next suggestion (this won't always happen)
* Press `Alt + [` to see the previous suggestion (this assumes there are multiple suggestions)
* Choose the suggestion you prefer and press `Tab`
* You most likely will get a complete program that's very simple. Try deleting and enhancing the comments to get Copilot to suggest more complex code.

5. **Try inline suggestions:**

* Start typing: `public static double Add(`
* Notice how Copilot suggests parameter names and the method body
* Accept or modify as needed

6. **Clear the code** in `Program.cs` you've added so you have an empty file

---

## Part 3: Build the Calculator with Copilot (12 minutes)

### Step 1: Create the Main Program Structure

1. **Type this comment at the top of your file:**

   ```csharp
   // Calculator console application with basic arithmetic operations
   // The program should display a menu and perform calculations based on user input
   ```

2. **Press Enter** and let Copilot suggest the initial code structure

* Accept suggestions that create a main menu loop
* You should get something similar to a menu-driven console app
* The expection is that it will not 'one-shot' the perfect app :)

### Step 2: Add Calculator Methods

1. **Add arithmetic operation methods using comments:**

   ```csharp
   // Method to add two numbers and return the result
   ```

* Accept Copilot's suggestion or modify as needed

2. **Continue with other operations:**

   Note: paste one comment at a time and let Copilot suggest the code for each method.

   ```csharp
   // Method to subtract second number from first number
   
   // Method to multiply two numbers
   
   // Method to divide first number by second number with error handling
   ```

3. **For each method:**

* Review Copilot's suggestion
* Accept with `Tab` if it looks correct
* Modify if needed (Copilot will adapt to your style)

### Step 3: Implement the Menu System

1. **Add a display menu method:**

   ```csharp
   // Method to display calculator menu options to the user
   ```

2. **Implement user input handling:**

   ```csharp
   // Method to get a valid number from user input with error handling
   ```

3. **Create the main program loop:**

   ```csharp
   // Main program loop that displays menu, gets user choice, and performs calculations
   ```

### Step 4: Enhance with Error Handling

1. **Add error handling comment before your divide method:**

   ```csharp
   // Add try-catch block to handle division by zero
   ```

2. **Let Copilot suggest the error handling code**

---

## Part 4: Explore Comment-Driven Development (5 minutes)

1. **Add a new feature using only comments first:**

   ```csharp
   // TODO: Add a method to calculate the square root of a number
   // The method should:
   // - Accept a double as input
   // - Check if the number is negative
   // - Return an error message for negative numbers
   // - Return the square root for positive numbers
   ```

2. **Press Enter** after the comments and observe how Copilot generates the entire method based on your specifications

3. **Try a more complex comment:**

   ```csharp
   // Add a method to keep history of last 5 calculations
   // Store operation type, operands, and result
   // Display history when user selects option from menu
   ```

4. **Experiment with different comment styles:**

   * Detailed multi-line comments
   * Single-line TODO comments
   * XML documentation comments (`///`)

---

## Part 5: Test and Refine (5 minutes)

1. **Run your application:**

   * Press `F5` or click the "Start" button
   * Test each calculator operation
   * Verify error handling works correctly

2. **Use Copilot to fix any issues:**

   * If you encounter an error, add a comment describing the fix needed
   * Example: `// Fix: Add input validation to prevent crash when user enters non-numeric value`

3. **Practice accepting and rejecting suggestions:**

   * Add a comment for a new feature
   * When Copilot suggests code, practice:

     * Accepting with `Tab`
     * Rejecting with `Esc`
     * Cycling through alternatives with `Alt + ]` and `Alt + [`

---

## Key Takeaways

✅ **You've learned to:**

* Enable and verify Copilot in Visual Studio 2022
* Accept, reject, and cycle through Copilot suggestions
* Use comment-driven development to generate code
* Combine Copilot with IntelliSense for efficient coding
* Let Copilot help with error handling and edge cases

## Tips for Success

1. **Be specific in your comments** - The more detailed your comment, the better Copilot's suggestion
2. **Review suggestions carefully** - Don't blindly accept; understand what the code does
3. **Use IntelliSense and Copilot together** - They complement each other
4. **Experiment with different prompting styles** - Find what works best for you

## Challenge (Optional)

If you finish early, try adding these features with Copilot's help:

* Add memory functions (M+, M-, MR, MC)
* Implement percentage calculations
* Add a scientific calculator mode with trigonometric functions
* Create unit tests for your calculator methods

---

## Troubleshooting

**Copilot not showing suggestions?**

* Check the Copilot icon in the status bar
* Ensure you're signed in with GitHub
* Try typing a comment and waiting 2-3 seconds

**Suggestions not relevant?**

* Make your comments more specific
* Include examples in your comments
* Break complex requirements into smaller parts

**Can't cycle through suggestions?**

* Ensure you're using `Alt + ]` and `Alt + [` (not Ctrl); did you change the default key bindings?
* Some suggestions may only have one option

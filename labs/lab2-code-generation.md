# Lab 2: Copilot for Code Generation & Patterns - Hands-on Exercise

**Duration:** 45 minutes

## Prerequisites

* Completed Lab 1 or basic familiarity with Copilot
* Visual Studio 2022 with GitHub Copilot enabled
* Basic understanding of C# classes and file I/O

## Exercise Overview

Build a Todo List console application using Copilot to generate entire classes, CRUD operations, and file persistence—demonstrating how Copilot recognizes and continues patterns.

---

## Part 1: Project Setup (3 minutes)

1. **Create a new Console App project:**

   * File → New → Project
   * Select "Console App" (C#)
   * Name: `TodoListManager`
   * Framework: .NET 9.0
   * Click `Create`

2. **Create project structure:**

   * Right-click on the project → Add → New Folder → Name it `Models`
   * Right-click on the project → Add → New Folder → Name it `Services`

3. **Verify Copilot is active**

---

## Part 2: Generate POCO Classes with Comments (8 minutes)

### Step 1: Create the Todo Item Model

1. **Right-click on Models folder** → Add → Class → Name: `TodoItem.cs`

2. **Delete the default code and type this detailed comment:**

   ```csharp
   // Todo item model class
   // Properties needed:
   // - Id (int) - unique identifier
   // - Title (string) - task title, required
   // - Description (string) - detailed description, optional
   // - IsCompleted (bool) - completion status, default false
   // - Priority (enum: Low, Medium, High) - task priority, default Medium
   // - DueDate (DateTime?) - optional due date
   // - CreatedDate (DateTime) - when the task was created
   // - CompletedDate (DateTime?) - when the task was completed
   // Include a constructor that sets CreatedDate to now
   ```

3. **Press Enter** and let Copilot generate the entire class

   * Review the generated code
   * Accept if it includes all properties
   * Copilot may or may not create the Priority enum

### Step 2: Create the Priority Enum

1. **If Copilot didn't create it inline, add a new file** `Priority.cs` in Models

2. **Type the comment:**

   Remove the existing class definition inside the namespace.

   ```csharp
   // Priority enum for todo items with three levels
   ```

3. **Accept Copilot's suggestion** for the enum

### Step 3: Create a Todo List Container

1. **Add new class** `TodoList.cs` in Models folder

2. **Type this comment:**

   ```csharp
   // Container class for managing a collection of todo items
   // Should include:
   // - A list of TodoItem objects
   // - A name for the todo list
   // - Methods to add, remove, and find items
   // - Method to get all incomplete items
   // - Method to get items by priority
   ```

3. **Let Copilot generate the class structure**

   You may need to add the code multiple lines at a time by typing in new lines, etc.

   > Don't forget to Build your project after generating the classes to ensure everything compiles correctly.
---

## Part 3: Implement CRUD Operations (12 minutes)

### Step 1: Create the Todo Service

1. **Add new class** `TodoService.cs` in Services folder

2. **Start with this detailed comment:**

   ```csharp
   // Service class for managing todo operations
   // This class should handle all CRUD operations for todo items
   // It should work with a TodoList instance
   ```

3. **Add method by method using comments:**

   ```csharp
   // Constructor that initializes an empty todo list with a given name
   
   // Method to add a new todo item with validation
   // Should check that title is not empty
   // Should assign a unique ID
   
   // Method to update an existing todo item
   // Should find item by ID and update its properties
   
   // Method to delete a todo item by ID
   // Should return true if deleted, false if not found
   
   // Method to mark a todo item as completed
   // Should set IsCompleted to true and CompletedDate to now
   
   // Method to get all todo items sorted by priority then due date
   
   // Method to search todo items by title or description
   // Case-insensitive search
   ```

4. **For each comment:**

   * Let Copilot suggest the implementation
   * Review for correctness
   * Accept or modify as needed

### Step 2: Add Pattern Recognition

1. **After creating a few methods, type a similar comment:**

   ```csharp
   // Method to get overdue items
   ```

2. **Notice how Copilot:**

   * Follows the pattern of your previous methods
   * Uses similar naming conventions
   * Maintains consistent error handling

---

## Part 4: Implement File Persistence (10 minutes)

### Step 1: Create File Storage Service

1. **Add new class** `FileStorageService.cs` in Services folder

2. **Type this comprehensive comment:**

   ```csharp
   // File storage service for persisting todo lists to JSON
   // Features needed:
   // - Save todo list to a JSON file
   // - Load todo list from a JSON file
   // - Handle file not found errors
   // - Create data directory if it doesn't exist
   // - Use System.Text.Json for serialization
   // File should be saved in a "Data" subfolder
   ```

3. **Add the using statement Copilot suggests:**

   ```csharp
   using System.Text.Json;
   ```

4. **Let Copilot generate the class**, then add methods:

   ```csharp
   // Method to save a TodoList to a JSON file
   // Parameters: TodoList object and filename
   // Should create Data directory if missing
   
   // Method to load a TodoList from a JSON file
   // Parameters: filename
   // Returns: TodoList or null if file doesn't exist
   
   // Method to delete a todo list file
   
   // Method to get all available todo list files
   ```

### Step 2: Add JSON Serialization Options

1. **Add a comment for better JSON handling:**

   ```csharp
   // Private method to get JsonSerializerOptions with:
   // - Pretty printing (WriteIndented = true)
   // - Case insensitive property names
   // - Handle null values
   ```

2. **Let Copilot implement the configuration**

---

## Part 5: Build the Main Program (7 minutes)

### Step 1: Create the User Interface

1. **Go to `Program.cs` and clear the default code**

2. **Type this comment structure:**

   ```csharp
   // Todo List Manager Console Application
   // Main menu options:
   // 1. Create new todo
   // 2. View all todos
   // 3. Mark todo as complete
   // 4. Delete todo
   // 5. Search todos
   // 6. Save to file
   // 7. Load from file
   // 8. Exit
   
   // The program should:
   // - Display a menu in a loop
   // - Handle invalid input gracefully
   // - Use the TodoService for all operations
   // - Provide feedback for all actions
   ```

3. **Let Copilot generate the main program structure**

### Step 2: Implement Menu Methods

1. **Add helper methods with comments:**

   ```csharp
   // Method to display the main menu and return user choice
   
   // Method to create a new todo with user input
   // Should prompt for title, description, priority, and due date
   
   // Method to display all todos in a formatted table
   // Show ID, Title, Priority, Due Date, and Status
   
   // Method to get a valid integer input from user
   
   // Method to get a valid DateTime input from user (optional)
   
   // Method to display a single todo item in detail
   ```

2. **For each method, let Copilot generate based on the pattern**

---

## Part 6: Test Pattern Recognition (5 minutes)

1. **Add a new feature to test pattern recognition:**

   ```csharp
   // Method to generate a summary report showing:
   // - Total tasks
   // - Completed tasks
   // - Overdue tasks
   // - Tasks by priority
   ```

2. **Notice how Copilot:**

   * Uses the same coding style
   * Follows established patterns
   * Reuses existing methods

3. **Run the application:**

   * Press F5
   * Test all CRUD operations
   * Save and load from file
   * Verify pattern consistency

---

## Key Takeaways

✅ **You've learned to:**

* Generate entire classes from detailed comments
* Use Copilot for CRUD operation patterns
* Implement file I/O with proper error handling
* Let Copilot recognize and continue coding patterns
* Build a complete application with minimal manual coding

## Challenge Extensions (Optional)

If you finish early, try these pattern-based additions:

1. **Add a new model**: Create a `Category` class and link it to TodoItems
2. **Extend the service**: Add bulk operations (mark multiple as complete)
3. **Enhance persistence**: Add CSV export/import alongside JSON
4. **Pattern variation**: Implement async versions of all methods

## Tips for Pattern-Based Development

1. **Establish patterns early** - Copilot learns from your initial code
2. **Be consistent** - Use similar comment structures
3. **Build incrementally** - Let each method inform the next
4. **Review pattern adherence** - Ensure Copilot maintains your style

---

## Part 7: Next Edit Suggestions (10 minutes)
You can read more and see screen shots at [Next Edit Suggestions](https://learn.microsoft.com/en-us/visualstudio/ide/copilot-next-edit-suggestions?view=vs-2022).

### Prerequisites and Setup

1. **Verify requirements:**
   * Visual Studio 2022 version 17.14 or later
   * GitHub account with Copilot access

2. **Enable the feature:**
   * Navigate to **Tools → Options → GitHub → Copilot → Copilot Completions**
   * Check **"Enable Next Edit Suggestions"**
   * Click **OK**
    -or-
   * Click the Copilot badge in the upper right corner
   * Select "Settings"
   * Place a checkmark next to "Enable Next Edit Suggestions"

3. **Understanding the feature:**
   * Suggests code edits based on your recent changes
   * Shows **arrows in the gutter** to indicate available suggestions
   * Navigate suggestions with the **Tab** key
   * Helps you stay in the logical edit flow

### Using Next Edit Suggestions

1. **Create a new validation class:**
   * Add new class `TodoValidator.cs` in Services folder

2. **Start with the first validation method:**

   ```csharp
   public bool ValidateTitle(string title)
   {
       return !string.IsNullOrWhiteSpace(title);
   }
   ```

3. **Look for gutter arrows:**
   * After completing the method, look for **arrows in the gutter**
   * Click the arrow to explore the edit suggestion menu
   * Or press **Tab** to navigate to the suggested location
   * Copilot suggests the next validation method

4. **Continue the pattern:**
   * Accept the suggestion for `ValidateDescription`
   * Tab again for `ValidateDueDate`
   * Notice how it maintains your validation pattern

### Cross-File Edit Suggestions

1. **Make a change that affects multiple files:**
   * In TodoService, rename `AddTodoItem` to `CreateTodo`
   * Save the file

2. **Navigate to related files:**
   * Open `Program.cs`
   * Look for **arrows in the gutter** where the old method was called
   * Click arrows or use **Tab** to navigate through suggestions
   * Copilot suggests updating to the new method name

3. **Types of suggestions you'll see:**
   * Typo corrections
   * Logic adjustments
   * Code refactoring updates
   * Variable renaming across files

### Working with Validation Integration

1. **Update the TodoService to use validation:**
   * Open `TodoService.cs`
   * Add at the top:

   ```csharp
   private readonly TodoValidator _validator;
   ```

2. **Look for gutter arrows in constructor:**
   * Navigate to the constructor
   * Gutter arrows suggest initializing `_validator`

3. **Update the Add method:**
   * Find your Add method
   * Start typing: `if (!_validator.`
   * Next Edit Suggestions helps complete the validation calls

### Best Practices for Next Edit Suggestions

1. **Follow the suggested workflow:**
   * Make complete, logical changes
   * Look for **gutter arrows** after each change
   * Use **Tab** or **click arrows** to navigate suggestions

2. **Ideal use cases:**
   * Refactoring variable names across multiple files
   * Implementing similar methods in different classes
   * Updating related code after interface changes
   * Fixing consistent patterns throughout the codebase

3. **Navigation tips:**
   * **Arrows pointing up/down** hint at edit locations above or below current view
   * Suggestions can span single symbols, entire lines, or multiple lines
   * Use with regular Copilot suggestions for maximum productivity

## Common Issues & Solutions

**Copilot not following patterns?**

* Ensure your first implementations are clear
* Use consistent naming conventions
* Add explicit pattern hints in comments

**Generated code too complex?**

* Break down comments into smaller steps
* Specify "simple implementation" in comments
* Guide with example usage

**File I/O errors?**

* Check file paths in generated code
* Ensure proper using statements
* Verify Data folder creation

**Next Edit Suggestions not appearing?**

* Ensure the feature is enabled: Tools → Options → GitHub → Copilot → Copilot Completions
* Verify you have Visual Studio 2022 version 17.14 or later
* Make a complete, logical edit (save the file)
* Look for **arrows in the gutter** and floating over your code.
* Try making a more significant change that would affect multiple locations

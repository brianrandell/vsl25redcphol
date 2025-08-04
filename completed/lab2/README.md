# Lab 2: Code Generation & Patterns - Todo List Manager

This folder contains the completed implementation of Lab 2 from the GitHub Copilot training at VSLIVE! 2025 Redmond. This console application demonstrates code generation techniques, pattern recognition, and validation integration with GitHub Copilot.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**

## Getting Started

### Step 1: Open the Project

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\lab2\TodoListManager\TodoListManager.csproj` and open it

### Step 2: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` or `Ctrl+F5` to run the application
3. The console application will launch with an interactive menu

## Features Demonstrated

### Core Functionality

* **Todo Item Management**: Create, update, delete, and list todo items
* **Priority System**: High, Medium, Low priority levels
* **File Persistence**: JSON serialization with FileStorageService
* **Interactive Console UI**: Menu-driven interface with user input validation
* **Search & Filtering**: Find todos by title/description, filter by priority
* **Summary Reports**: View statistics and overdue items

### Code Generation Techniques Demonstrated

* **POCO Class Generation**: TodoItem model with detailed properties
* **Service Layer Pattern**: TodoService for business logic separation
* **Validation Integration**: TodoValidator with comprehensive validation rules
* **File I/O Operations**: Async JSON persistence with error handling
* **Pattern Recognition**: Consistent method generation based on established patterns
* **LINQ Operations**: Filtering, sorting, and querying collections

## How to Use

### Menu Options

1. **Create new todo** - Add tasks with title, description, priority, and due date
2. **View all todos** - Display all items sorted by priority and due date
3. **Mark todo as complete** - Set completion status and timestamp
4. **Delete todo** - Remove items from the list
5. **Search todos** - Find items by title or description text
6. **Save to file** - Export todo list to JSON file
7. **Load from file** - Import previously saved todo lists
8. **View summary report** - Display statistics and overdue items
9. **Exit** - Close the application

### Sample Usage

``` shell
=== Todo List Manager ===

Main Menu:
1. Create new todo
2. View all todos
3. Mark todo as complete
4. Delete todo
5. Search todos
6. Save to file
7. Load from file
8. View summary report
9. Exit

Enter your choice (1-9): 1

=== Create New Todo ===

Enter title: Review Copilot Lab 2
Enter description (optional): Complete the advanced code generation exercise
Select priority:
1. Low
2. Medium
3. High
Choice (1-3): 3
Enter due date (MM/dd/yyyy) or press Enter to skip: 12/15/2025

Todo created successfully! ID: 1
```

## Project Structure

``` shell
TodoListManager/
├── Models/
│   ├── TodoItem.cs          # Todo item data model with full properties
│   ├── TodoList.cs          # Collection wrapper with management methods
│   └── Priority.cs          # Priority enumeration (Low, Medium, High)
├── Services/
│   ├── TodoService.cs       # Business logic service with validation
│   ├── TodoValidator.cs     # Comprehensive validation logic
│   └── FileStorageService.cs # Async JSON file persistence
└── Program.cs               # Main application with interactive console UI
```

## Key Learning Points

1. **Comment-Driven Development**: Generate entire classes from detailed comments
2. **Pattern Recognition**: Copilot maintains consistent coding styles and patterns
3. **Validation Integration**: Systematic approach to input validation and error handling
4. **Service Architecture**: Separation of concerns with service layer pattern
5. **LINQ & Collections**: Advanced querying and data manipulation operations
6. **Next Edit Suggestions**: Visual Studio 2022's gutter arrows for workflow guidance

## Data Persistence

* Todo lists are saved to JSON files in a `Data` subfolder
* Multiple todo lists can be saved and loaded by filename
* Async file operations with proper error handling
* JSON format with pretty printing for easy inspection

## Troubleshooting

### Build Issues

* Ensure **.NET 9.0 SDK** is installed
* Check that all file references are correct
* Clean and rebuild if necessary

### Runtime Issues

* Verify write permissions in the application directory for `Data` folder creation
* Check that JSON files are not locked by another process
* Ensure proper input validation when entering dates and priorities

## Extension Ideas

* Add todo categories or tags system
* Implement recurring tasks with due date patterns
* Add user authentication and multiple user support
* Export to CSV, XML, or other formats
* Add email reminders for overdue tasks
* Implement a web API version of the service layer

---

**Note**: This is a demonstration/training project. The console interface and local file storage are designed for learning purposes.

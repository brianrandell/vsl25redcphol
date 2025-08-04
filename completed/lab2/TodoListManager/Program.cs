using TodoListManager.Models;
using TodoListManager.Services;

namespace TodoListManager;

class Program
{
    private static TodoService _todoService = new TodoService();
    private static FileStorageService _fileStorage = new FileStorageService();

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Todo List Manager ===\n");

        while (true)
        {
            var choice = DisplayMainMenu();
            
            try
            {
                switch (choice)
                {
                    case 1:
                        CreateNewTodo();
                        break;
                    case 2:
                        DisplayAllTodos();
                        break;
                    case 3:
                        MarkTodoAsComplete();
                        break;
                    case 4:
                        DeleteTodo();
                        break;
                    case 5:
                        SearchTodos();
                        break;
                    case 6:
                        await SaveToFile();
                        break;
                    case 7:
                        await LoadFromFile();
                        break;
                    case 8:
                        DisplaySummaryReport();
                        break;
                    case 9:
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static int DisplayMainMenu()
    {
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Create new todo");
        Console.WriteLine("2. View all todos");
        Console.WriteLine("3. Mark todo as complete");
        Console.WriteLine("4. Delete todo");
        Console.WriteLine("5. Search todos");
        Console.WriteLine("6. Save to file");
        Console.WriteLine("7. Load from file");
        Console.WriteLine("8. View summary report");
        Console.WriteLine("9. Exit");
        Console.Write("\nEnter your choice (1-9): ");

        return GetValidIntegerInput(1, 9);
    }

    static void CreateNewTodo()
    {
        Console.Clear();
        Console.WriteLine("=== Create New Todo ===\n");

        Console.Write("Enter title: ");
        var title = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty!");
            return;
        }

        Console.Write("Enter description (optional): ");
        var description = Console.ReadLine() ?? "";

        Console.WriteLine("\nSelect priority:");
        Console.WriteLine("1. Low");
        Console.WriteLine("2. Medium");
        Console.WriteLine("3. High");
        Console.Write("Choice (1-3): ");
        
        var priorityChoice = GetValidIntegerInput(1, 3);
        var priority = priorityChoice switch
        {
            1 => Priority.Low,
            2 => Priority.Medium,
            3 => Priority.High,
            _ => Priority.Medium
        };

        Console.Write("Enter due date (MM/dd/yyyy) or press Enter to skip: ");
        var dueDateInput = Console.ReadLine();
        DateTime? dueDate = null;
        
        if (!string.IsNullOrWhiteSpace(dueDateInput))
        {
            if (DateTime.TryParse(dueDateInput, out var parsedDate))
            {
                dueDate = parsedDate;
            }
            else
            {
                Console.WriteLine("Invalid date format. No due date set.");
            }
        }

        var todo = _todoService.AddTodoItem(title, description, priority, dueDate);
        Console.WriteLine($"\nTodo created successfully! ID: {todo.Id}");
    }

    static void DisplayAllTodos()
    {
        Console.Clear();
        Console.WriteLine("=== All Todos ===\n");

        var todos = _todoService.GetAllTodosSorted();
        if (!todos.Any())
        {
            Console.WriteLine("No todos found.");
            return;
        }

        Console.WriteLine($"{"ID",-4} {"Title",-25} {"Priority",-10} {"Due Date",-12} {"Status",-10}");
        Console.WriteLine(new string('-', 65));

        foreach (var todo in todos)
        {
            var dueDateStr = todo.DueDate?.ToString("MM/dd/yyyy") ?? "None";
            var status = todo.IsCompleted ? "Complete" : "Pending";
            var titleDisplay = todo.Title.Length > 24 ? todo.Title.Substring(0, 21) + "..." : todo.Title;

            Console.WriteLine($"{todo.Id,-4} {titleDisplay,-25} {todo.Priority,-10} {dueDateStr,-12} {status,-10}");
        }

        Console.WriteLine($"\nTotal: {todos.Count} todos");
    }

    static void MarkTodoAsComplete()
    {
        Console.Clear();
        Console.WriteLine("=== Mark Todo as Complete ===\n");

        DisplayAllTodos();
        Console.Write("\nEnter todo ID to mark as complete: ");
        
        var id = GetValidIntegerInput();
        if (_todoService.MarkAsCompleted(id))
        {
            Console.WriteLine("Todo marked as complete!");
        }
        else
        {
            Console.WriteLine("Todo not found!");
        }
    }

    static void DeleteTodo()
    {
        Console.Clear();
        Console.WriteLine("=== Delete Todo ===\n");

        DisplayAllTodos();
        Console.Write("\nEnter todo ID to delete: ");
        
        var id = GetValidIntegerInput();
        if (_todoService.DeleteTodoItem(id))
        {
            Console.WriteLine("Todo deleted successfully!");
        }
        else
        {
            Console.WriteLine("Todo not found!");
        }
    }

    static void SearchTodos()
    {
        Console.Clear();
        Console.WriteLine("=== Search Todos ===\n");

        Console.Write("Enter search term: ");
        var searchTerm = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term cannot be empty!");
            return;
        }

        var results = _todoService.SearchTodos(searchTerm);
        if (!results.Any())
        {
            Console.WriteLine("No matching todos found.");
            return;
        }

        Console.WriteLine($"\nFound {results.Count} matching todos:\n");
        
        foreach (var todo in results)
        {
            DisplayTodoDetail(todo);
            Console.WriteLine();
        }
    }

    static async Task SaveToFile()
    {
        Console.Clear();
        Console.WriteLine("=== Save to File ===\n");

        Console.Write("Enter filename (without extension): ");
        var filename = Console.ReadLine();
        
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("Filename cannot be empty!");
            return;
        }

        await _fileStorage.SaveTodoListAsync(_todoService.GetTodoList(), filename);
        Console.WriteLine($"Todo list saved to {filename}.json!");
    }

    static async Task LoadFromFile()
    {
        Console.Clear();
        Console.WriteLine("=== Load from File ===\n");

        var availableFiles = _fileStorage.GetAvailableTodoListFiles();
        if (!availableFiles.Any())
        {
            Console.WriteLine("No saved todo lists found.");
            return;
        }

        Console.WriteLine("Available files:");
        for (int i = 0; i < availableFiles.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {availableFiles[i]}");
        }

        Console.Write($"\nSelect file (1-{availableFiles.Count}): ");
        var choice = GetValidIntegerInput(1, availableFiles.Count);
        var selectedFile = availableFiles[choice - 1];

        var todoList = await _fileStorage.LoadTodoListAsync(selectedFile);
        if (todoList != null)
        {
            _todoService.SetTodoList(todoList);
            Console.WriteLine($"Todo list loaded from {selectedFile}.json!");
        }
        else
        {
            Console.WriteLine("Failed to load todo list!");
        }
    }

    static void DisplaySummaryReport()
    {
        Console.Clear();
        Console.WriteLine("=== Summary Report ===\n");

        var report = _todoService.GenerateSummaryReport();
        
        foreach (var item in report)
        {
            Console.WriteLine($"{item.Key}: {item.Value}");
        }

        var overdueTodos = _todoService.GetOverdueItems();
        if (overdueTodos.Any())
        {
            Console.WriteLine($"\nOverdue Items ({overdueTodos.Count}):");
            foreach (var todo in overdueTodos)
            {
                Console.WriteLine($"- {todo.Title} (Due: {todo.DueDate:MM/dd/yyyy})");
            }
        }
    }

    static void DisplayTodoDetail(TodoItem todo)
    {
        Console.WriteLine($"ID: {todo.Id}");
        Console.WriteLine($"Title: {todo.Title}");
        Console.WriteLine($"Description: {todo.Description}");
        Console.WriteLine($"Priority: {todo.Priority}");
        Console.WriteLine($"Due Date: {todo.DueDate?.ToString("MM/dd/yyyy") ?? "None"}");
        Console.WriteLine($"Status: {(todo.IsCompleted ? "Complete" : "Pending")}");
        Console.WriteLine($"Created: {todo.CreatedDate:MM/dd/yyyy HH:mm}");
        if (todo.IsCompleted && todo.CompletedDate.HasValue)
        {
            Console.WriteLine($"Completed: {todo.CompletedDate:MM/dd/yyyy HH:mm}");
        }
    }

    static int GetValidIntegerInput(int min = int.MinValue, int max = int.MaxValue)
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result >= min && result <= max)
                {
                    return result;
                }
                Console.Write($"Please enter a number between {min} and {max}: ");
            }
            else
            {
                Console.Write("Please enter a valid number: ");
            }
        }
    }
}

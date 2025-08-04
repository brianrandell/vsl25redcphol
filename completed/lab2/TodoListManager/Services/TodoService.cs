using TodoListManager.Models;

namespace TodoListManager.Services;

public class TodoService
{
    private TodoList _todoList;
    private int _nextId = 1;
    private readonly TodoValidator _validator;

    public TodoService(string listName = "My Todo List")
    {
        _todoList = new TodoList(listName);
        _validator = new TodoValidator();
    }

    public TodoItem AddTodoItem(string title, string description = "", Priority priority = Priority.Medium, DateTime? dueDate = null)
    {
        var validationErrors = _validator.GetValidationErrors(title, description, priority, dueDate);
        if (validationErrors.Any())
        {
            throw new ArgumentException($"Validation failed: {string.Join(", ", validationErrors)}");
        }

        var todoItem = new TodoItem(title, description, priority, dueDate)
        {
            Id = _nextId++
        };

        _todoList.AddItem(todoItem);
        return todoItem;
    }

    public bool UpdateTodoItem(int id, string? title = null, string? description = null, Priority? priority = null, DateTime? dueDate = null)
    {
        var item = _todoList.FindItem(id);
        if (item == null) return false;

        if (title != null && !_validator.ValidateTitle(title))
            throw new ArgumentException("Title cannot be empty or whitespace");
        
        if (description != null && !_validator.ValidateDescription(description))
            throw new ArgumentException("Description cannot be null");
        
        if (priority.HasValue && !_validator.ValidatePriority(priority.Value))
            throw new ArgumentException("Priority must be a valid value");
        
        if (dueDate.HasValue && !_validator.ValidateDueDate(dueDate.Value))
            throw new ArgumentException("Due date cannot be in the past");

        if (title != null) item.Title = title;
        if (description != null) item.Description = description;
        if (priority.HasValue) item.Priority = priority.Value;
        if (dueDate.HasValue) item.DueDate = dueDate.Value;

        return true;
    }

    public bool DeleteTodoItem(int id)
    {
        return _todoList.RemoveItem(id);
    }

    public bool MarkAsCompleted(int id)
    {
        var item = _todoList.FindItem(id);
        if (item == null) return false;

        item.IsCompleted = true;
        item.CompletedDate = DateTime.Now;
        return true;
    }

    public List<TodoItem> GetAllTodosSorted()
    {
        return _todoList.GetAllItemsSorted();
    }

    public List<TodoItem> SearchTodos(string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm)) return new List<TodoItem>();

        return _todoList.Items.Where(x => 
            x.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
            x.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<TodoItem> GetOverdueItems()
    {
        return _todoList.Items.Where(x => 
            !x.IsCompleted && 
            x.DueDate.HasValue && 
            x.DueDate.Value < DateTime.Now)
            .ToList();
    }

    public TodoList GetTodoList()
    {
        return _todoList;
    }

    public void SetTodoList(TodoList todoList)
    {
        _todoList = todoList;
        _nextId = _todoList.Items.Count > 0 ? _todoList.Items.Max(x => x.Id) + 1 : 1;
    }

    public Dictionary<string, int> GenerateSummaryReport()
    {
        var totalTasks = _todoList.Items.Count;
        var completedTasks = _todoList.Items.Count(x => x.IsCompleted);
        var overdueTasks = GetOverdueItems().Count;
        var highPriorityTasks = _todoList.Items.Count(x => x.Priority == Priority.High);
        var mediumPriorityTasks = _todoList.Items.Count(x => x.Priority == Priority.Medium);
        var lowPriorityTasks = _todoList.Items.Count(x => x.Priority == Priority.Low);

        return new Dictionary<string, int>
        {
            ["Total Tasks"] = totalTasks,
            ["Completed Tasks"] = completedTasks,
            ["Overdue Tasks"] = overdueTasks,
            ["High Priority"] = highPriorityTasks,
            ["Medium Priority"] = mediumPriorityTasks,
            ["Low Priority"] = lowPriorityTasks
        };
    }
}
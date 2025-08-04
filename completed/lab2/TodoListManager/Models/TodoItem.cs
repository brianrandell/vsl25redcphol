namespace TodoListManager.Models;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public Priority Priority { get; set; } = Priority.Medium;
    public DateTime? DueDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public TodoItem()
    {
        CreatedDate = DateTime.Now;
    }

    public TodoItem(string title, string description = "", Priority priority = Priority.Medium, DateTime? dueDate = null)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        CreatedDate = DateTime.Now;
    }
}
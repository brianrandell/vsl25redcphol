using TodoListManager.Models;

namespace TodoListManager.Services;

public class TodoValidator
{
    public bool ValidateTitle(string title)
    {
        return !string.IsNullOrWhiteSpace(title);
    }

    public bool ValidateDescription(string description)
    {
        return description != null;
    }

    public bool ValidateDueDate(DateTime? dueDate)
    {
        return !dueDate.HasValue || dueDate.Value > DateTime.Now;
    }

    public bool ValidatePriority(Priority priority)
    {
        return Enum.IsDefined(typeof(Priority), priority);
    }

    public bool ValidateTodoItem(TodoItem todoItem)
    {
        if (todoItem == null)
            return false;

        return ValidateTitle(todoItem.Title) &&
               ValidateDescription(todoItem.Description) &&
               ValidateDueDate(todoItem.DueDate) &&
               ValidatePriority(todoItem.Priority);
    }

    public List<string> GetValidationErrors(TodoItem todoItem)
    {
        var errors = new List<string>();

        if (todoItem == null)
        {
            errors.Add("Todo item cannot be null");
            return errors;
        }

        if (!ValidateTitle(todoItem.Title))
            errors.Add("Title cannot be empty or whitespace");

        if (!ValidateDescription(todoItem.Description))
            errors.Add("Description cannot be null");

        if (!ValidateDueDate(todoItem.DueDate))
            errors.Add("Due date cannot be in the past");

        if (!ValidatePriority(todoItem.Priority))
            errors.Add("Priority must be a valid value");

        return errors;
    }

    public List<string> GetValidationErrors(string title, string description = "", Priority priority = Priority.Medium, DateTime? dueDate = null)
    {
        var errors = new List<string>();

        if (!ValidateTitle(title))
            errors.Add("Title cannot be empty or whitespace");

        if (!ValidateDescription(description))
            errors.Add("Description cannot be null");

        if (!ValidateDueDate(dueDate))
            errors.Add("Due date cannot be in the past");

        if (!ValidatePriority(priority))
            errors.Add("Priority must be a valid value");

        return errors;
    }
}
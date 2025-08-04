namespace TodoListManager.Models;

public class TodoList
{
    public string Name { get; set; } = string.Empty;
    public List<TodoItem> Items { get; set; } = new List<TodoItem>();

    public TodoList(string name = "My Todo List")
    {
        Name = name;
    }

    public void AddItem(TodoItem item)
    {
        Items.Add(item);
    }

    public bool RemoveItem(int id)
    {
        var item = Items.FirstOrDefault(x => x.Id == id);
        if (item != null)
        {
            Items.Remove(item);
            return true;
        }
        return false;
    }

    public TodoItem? FindItem(int id)
    {
        return Items.FirstOrDefault(x => x.Id == id);
    }

    public List<TodoItem> GetIncompleteItems()
    {
        return Items.Where(x => !x.IsCompleted).ToList();
    }

    public List<TodoItem> GetItemsByPriority(Priority priority)
    {
        return Items.Where(x => x.Priority == priority).ToList();
    }

    public List<TodoItem> GetAllItemsSorted()
    {
        return Items.OrderBy(x => x.Priority).ThenBy(x => x.DueDate ?? DateTime.MaxValue).ToList();
    }
}
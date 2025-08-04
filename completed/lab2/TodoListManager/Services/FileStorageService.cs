using System.Text.Json;
using TodoListManager.Models;

namespace TodoListManager.Services;

public class FileStorageService
{
    private const string DataDirectory = "Data";

    public FileStorageService()
    {
        if (!Directory.Exists(DataDirectory))
        {
            Directory.CreateDirectory(DataDirectory);
        }
    }

    public async Task SaveTodoListAsync(TodoList todoList, string filename)
    {
        var filePath = Path.Combine(DataDirectory, $"{filename}.json");
        var options = GetJsonSerializerOptions();
        var json = JsonSerializer.Serialize(todoList, options);
        await File.WriteAllTextAsync(filePath, json);
    }

    public async Task<TodoList?> LoadTodoListAsync(string filename)
    {
        var filePath = Path.Combine(DataDirectory, $"{filename}.json");
        
        if (!File.Exists(filePath))
        {
            return null;
        }

        try
        {
            var json = await File.ReadAllTextAsync(filePath);
            var options = GetJsonSerializerOptions();
            return JsonSerializer.Deserialize<TodoList>(json, options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    public bool DeleteTodoListFile(string filename)
    {
        var filePath = Path.Combine(DataDirectory, $"{filename}.json");
        
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        
        return false;
    }

    public List<string> GetAvailableTodoListFiles()
    {
        if (!Directory.Exists(DataDirectory))
        {
            return new List<string>();
        }

        return Directory.GetFiles(DataDirectory, "*.json")
            .Select(Path.GetFileNameWithoutExtension)
            .Where(name => !string.IsNullOrEmpty(name))
            .ToList()!;
    }

    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }
}
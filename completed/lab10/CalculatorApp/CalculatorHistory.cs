using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class CalculatorHistory
{
    private readonly List<string> _history;
    private readonly int _maxHistorySize;
    private decimal _memory;

    public CalculatorHistory(int maxHistorySize = 10)
    {
        _history = new List<string>();
        _maxHistorySize = maxHistorySize;
        _memory = 0;
    }

    public void AddCalculation(string calculation)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string entry = $"[{timestamp}] {calculation}";
        
        _history.Add(entry);
        
        // Keep only the last N calculations
        if (_history.Count > _maxHistorySize)
        {
            _history.RemoveAt(0);
        }
    }

    public void DisplayHistory()
    {
        if (_history.Count == 0)
        {
            Console.WriteLine("No calculations in history.");
            return;
        }

        Console.WriteLine("\n=== Calculation History ===");
        for (int i = 0; i < _history.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_history[i]}");
        }
        Console.WriteLine("========================\n");
    }

    public void ClearHistory()
    {
        _history.Clear();
        Console.WriteLine("History cleared successfully.");
    }

    public void SaveHistoryToFile(string filename = "calculator_history.txt")
    {
        try
        {
            File.WriteAllLines(filename, _history);
            Console.WriteLine($"History saved to {filename}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving history: {ex.Message}");
        }
    }

    // Memory functions
    public void MemoryAdd(decimal value)
    {
        _memory += value;
        Console.WriteLine($"Memory: {_memory} (Added {value})");
    }

    public void MemorySubtract(decimal value)
    {
        _memory -= value;
        Console.WriteLine($"Memory: {_memory} (Subtracted {value})");
    }

    public decimal MemoryRecall()
    {
        Console.WriteLine($"Memory recalled: {_memory}");
        return _memory;
    }

    public void MemoryClear()
    {
        _memory = 0;
        Console.WriteLine("Memory cleared.");
    }

    public decimal GetMemoryValue()
    {
        return _memory;
    }

    public List<string> GetHistoryList()
    {
        return new List<string>(_history);
    }
}
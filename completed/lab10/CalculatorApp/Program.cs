using System;

Console.WriteLine("Welcome to the Interactive Calculator!");
Console.WriteLine();

bool continueCalculating = true;

while (continueCalculating)
{
    Console.WriteLine("Please choose an operation:");
    Console.WriteLine("1. Addition");
    Console.WriteLine("2. Subtraction");
    Console.WriteLine("3. Multiplication");
    Console.WriteLine("4. Division");
    Console.WriteLine("5. Modulus");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    
    Console.Write("Enter your choice: ");
    string? choice = Console.ReadLine();
    
    if (choice == "0")
    {
        continueCalculating = false;
        Console.WriteLine("\nThank you for using the calculator. Goodbye!");
        break;
    }
    
    if (!IsValidChoice(choice))
    {
        Console.WriteLine("\nInvalid choice. Please select a number between 0 and 5.\n");
        continue;
    }
    
    Console.Write("Enter first number: ");
    string? firstInput = Console.ReadLine();
    
    if (!double.TryParse(firstInput, out double firstNumber))
    {
        Console.WriteLine("\nInvalid input. Please enter a valid number.\n");
        continue;
    }
    
    Console.Write("Enter second number: ");
    string? secondInput = Console.ReadLine();
    
    if (!double.TryParse(secondInput, out double secondNumber))
    {
        Console.WriteLine("\nInvalid input. Please enter a valid number.\n");
        continue;
    }
    
    try
    {
        double result = PerformOperation(choice!, firstNumber, secondNumber);
        string operationSymbol = GetOperationSymbol(choice!);
        Console.WriteLine($"\nResult: {firstNumber} {operationSymbol} {secondNumber} = {result}\n");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("\nError: Division by zero is not allowed.\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"\nAn error occurred: {ex.Message}\n");
    }
}

static bool IsValidChoice(string? choice)
{
    return choice == "1" || choice == "2" || choice == "3" || choice == "4" || choice == "5";
}

static double PerformOperation(string choice, double first, double second)
{
    return choice switch
    {
        "1" => first + second,
        "2" => first - second,
        "3" => first * second,
        "4" => second == 0 ? throw new DivideByZeroException() : first / second,
        "5" => second == 0 ? throw new DivideByZeroException() : first % second,
        _ => throw new InvalidOperationException("Invalid operation choice")
    };
}

static string GetOperationSymbol(string choice)
{
    return choice switch
    {
        "1" => "+",
        "2" => "-",
        "3" => "*",
        "4" => "/",
        "5" => "%",
        _ => "?"
    };
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace CopilotCalculator
{
    class Program
    {
        private static List<string> calculationHistory = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the Copilot Calculator!");
            Console.WriteLine("==================================");

            bool continueCalculating = true;
            
            while (continueCalculating)
            {
                DisplayMenu();
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1":
                        PerformAddition();
                        break;
                    case "2":
                        PerformSubtraction();
                        break;
                    case "3":
                        PerformMultiplication();
                        break;
                    case "4":
                        PerformDivision();
                        break;
                    case "5":
                        PerformSquareRoot();
                        break;
                    case "6":
                        DisplayHistory();
                        break;
                    case "7":
                        continueCalculating = false;
                        Console.WriteLine("Thank you for using Copilot Calculator!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please select a valid option.");
                        break;
                }

                if (continueCalculating)
                {
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("\nCalculator Menu:");
            Console.WriteLine("1. Addition");
            Console.WriteLine("2. Subtraction");
            Console.WriteLine("3. Multiplication");
            Console.WriteLine("4. Division");
            Console.WriteLine("5. Square Root");
            Console.WriteLine("6. View History");
            Console.WriteLine("7. Exit");
            Console.Write("\nPlease select an option (1-7): ");
        }

        static double GetValidNumber(string prompt)
        {
            double number;
            Console.Write(prompt);
            
            while (!double.TryParse(Console.ReadLine(), out number))
            {
                Console.Write("Invalid input. Please enter a valid number: ");
            }
            
            return number;
        }

        static void PerformAddition()
        {
            double num1 = GetValidNumber("Enter the first number: ");
            double num2 = GetValidNumber("Enter the second number: ");
            double result = Add(num1, num2);
            
            string calculation = $"{num1} + {num2} = {result}";
            Console.WriteLine($"Result: {calculation}");
            AddToHistory(calculation);
        }

        static void PerformSubtraction()
        {
            double num1 = GetValidNumber("Enter the first number: ");
            double num2 = GetValidNumber("Enter the second number: ");
            double result = Subtract(num1, num2);
            
            string calculation = $"{num1} - {num2} = {result}";
            Console.WriteLine($"Result: {calculation}");
            AddToHistory(calculation);
        }

        static void PerformMultiplication()
        {
            double num1 = GetValidNumber("Enter the first number: ");
            double num2 = GetValidNumber("Enter the second number: ");
            double result = Multiply(num1, num2);
            
            string calculation = $"{num1} × {num2} = {result}";
            Console.WriteLine($"Result: {calculation}");
            AddToHistory(calculation);
        }

        static void PerformDivision()
        {
            double num1 = GetValidNumber("Enter the first number: ");
            double num2 = GetValidNumber("Enter the second number: ");
            
            try
            {
                double result = Divide(num1, num2);
                string calculation = $"{num1} ÷ {num2} = {result}";
                Console.WriteLine($"Result: {calculation}");
                AddToHistory(calculation);
            }
            catch (DivideByZeroException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                AddToHistory($"{num1} ÷ {num2} = Error: Division by zero");
            }
        }

        static void PerformSquareRoot()
        {
            double number = GetValidNumber("Enter a number: ");
            
            if (number < 0)
            {
                Console.WriteLine("Error: Cannot calculate square root of a negative number.");
                AddToHistory($"√{number} = Error: Negative number");
            }
            else
            {
                double result = Math.Sqrt(number);
                string calculation = $"√{number} = {result}";
                Console.WriteLine($"Result: {calculation}");
                AddToHistory(calculation);
            }
        }

        static double Add(double a, double b)
        {
            return a + b;
        }

        static double Subtract(double a, double b)
        {
            return a - b;
        }

        static double Multiply(double a, double b)
        {
            return a * b;
        }

        static double Divide(double a, double b)
        {
            if (b == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero.");
            }
            return a / b;
        }

        static void AddToHistory(string calculation)
        {
            calculationHistory.Add($"{DateTime.Now:HH:mm:ss} - {calculation}");
            
            if (calculationHistory.Count > 5)
            {
                calculationHistory.RemoveAt(0);
            }
        }

        static void DisplayHistory()
        {
            Console.WriteLine("\nCalculation History (Last 5):");
            Console.WriteLine("===============================");
            
            if (calculationHistory.Count == 0)
            {
                Console.WriteLine("No calculations performed yet.");
            }
            else
            {
                for (int i = 0; i < calculationHistory.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {calculationHistory[i]}");
                }
            }
        }
    }
}
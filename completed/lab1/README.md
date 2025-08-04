# CopilotCalculator - Lab 1 Solution

This is the completed solution for Lab 1: GitHub Copilot Fundamentals from the VSLIVE! 2025 Redmond hands-on lab.

## Features Implemented

### Core Calculator Operations

* **Addition**: Add two numbers
* **Subtraction**: Subtract second number from first
* **Multiplication**: Multiply two numbers  
* **Division**: Divide first number by second with error handling for division by zero

### Additional Features

* **Square Root**: Calculate square root with validation for negative numbers
* **Calculation History**: Stores last 5 calculations with timestamps
* **Menu-driven Interface**: User-friendly console menu system
* **Input Validation**: Robust error handling for invalid numeric input
* **Error Handling**: Proper exception handling for mathematical errors

## How to Run

1. Navigate to the lab1 directory:

   ```shell
   cd completed\lab1
   ```

2. Build and run the application:

   ```shell
   dotnet run
   ```

## Project Structure

* `Program.cs` - Main application with all calculator functionality
* `CopilotCalculator.csproj` - .NET 9.0 project file
* `README.md` - This documentation file

## Key Learning Objectives Covered

This solution demonstrates the core concepts that would be taught in Lab 1:

1. **Comment-driven Development**: Each method follows the pattern of descriptive comments leading to implementation
2. **Menu System Implementation**: Clean, user-friendly console interface
3. **Error Handling**: Proper exception handling for edge cases
4. **Input Validation**: Robust handling of user input
5. **Code Organization**: Well-structured methods with clear responsibilities
6. **Feature Enhancement**: Additional features like history and square root calculations

## Usage Example

``` shell
Welcome to the Copilot Calculator!
==================================

Calculator Menu:
1. Addition
2. Subtraction  
3. Multiplication
4. Division
5. Square Root
6. View History
7. Exit

Please select an option (1-7): 1
Enter the first number: 15
Enter the second number: 25
Result: 15 + 25 = 40
```

This solution represents what students would create by following the lab instructions and leveraging
GitHub Copilot's suggestions throughout the development process.

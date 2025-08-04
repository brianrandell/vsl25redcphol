# Lab 10: GitHub Copilot Coding Agent - Interactive Calculator

This folder contains the completed implementation of Lab 10 from the GitHub Copilot training at VSLIVE! 2025 Redmond.
This console application demonstrates the result of using GitHub Copilot Coding Agent to autonomously transform a
simple "Hello World" application into a fully functional interactive calculator with advanced features.

## Prerequisites

- **Visual Studio 2022** (any edition) or **Visual Studio Code**
- **GitHub Copilot Pro** subscription (required for Coding Agent features)
- **.NET 9.0 SDK**
- **GitHub account** with repository access

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `CalculatorApp` project under the `lab10` folder

### Step 2: Set as Startup Project

1. In **Solution Explorer**, right-click on `CalculatorApp`
2. Select **"Set as Startup Project"**

### Step 3: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The interactive calculator will launch in the console

## Features Implemented by Copilot Agent

### Basic Calculator Operations

- **Addition (+)**: Add two numbers
- **Subtraction (-)**: Subtract second number from first
- **Multiplication (*)**: Multiply two numbers
- **Division (/)**: Divide first number by second (with zero check)
- **Modulus (%)**: Get remainder of division (with zero check)

### Enhanced Features (CalculatorHistory.cs Class)

The solution includes a `CalculatorHistory` class that provides advanced features:

- **Calculation History**: Stores last 10 calculations with timestamps
- **Memory Functions**: M+, M-, MR (Memory Recall), MC (Memory Clear)
- **History Management**: View history, clear history, save to file
- **Extensible Design**: Ready for integration into the main calculator interface

## Application Structure

``` shell
CalculatorApp/
├── Program.cs           # Main calculator implementation (basic CRUD operations)
├── CalculatorHistory.cs # Advanced history and memory management class
└── CalculatorApp.csproj # Project file
```

**Note**: The main `Program.cs` implements the core calculator functionality as requested in the first GitHub issue. The `CalculatorHistory.cs` class demonstrates additional features that could be implemented by assigning a second enhancement issue to the Copilot Agent (as shown in the lab exercise).

## Sample Interaction

```
Welcome to the Interactive Calculator!

Please choose an operation:
1. Addition
2. Subtraction
3. Multiplication
4. Division
5. Modulus
0. Exit

Enter your choice: 1
Enter first number: 10
Enter second number: 5

Result: 10 + 5 = 15

Please choose an operation:
[Menu repeats...]
```

## Code Quality Features

### Input Validation

- Validates menu choices (0-5)
- Validates numeric input with proper error messages
- Handles invalid input gracefully with clear feedback

### Error Handling

- Division by zero protection
- Modulus by zero protection
- Try-catch blocks for unexpected errors
- User-friendly error messages

### Modern C# Features

- Pattern matching with switch expressions
- Nullable reference types
- Top-level statements
- String interpolation

## Key Learning Points

### Copilot Agent Capabilities

1. **Requirement Understanding**: Interpreted detailed issue requirements
2. **Code Generation**: Created complete, working implementation
3. **Best Practices**: Applied modern C# patterns and error handling
4. **User Experience**: Implemented intuitive menu-driven interface

### Code Quality from AI

- **Clean Structure**: Well-organized, readable code
- **Defensive Programming**: Comprehensive input validation
- **Error Handling**: Proper exception handling throughout
- **Documentation**: Clear variable names and logical flow

## Testing the Application

### Valid Operations

1. Test each mathematical operation (1-5)
2. Verify correct results for various inputs
3. Test with decimal numbers
4. Test with negative numbers

### Edge Cases

1. Division by zero (should show error)
2. Modulus by zero (should show error)
3. Invalid menu choices
4. Non-numeric input
5. Exit functionality (option 0)

## Extended Functionality

The `CalculatorHistory.cs` class provides additional features:

### Memory Operations

```csharp
history.MemoryAdd(10.5m);      // M+
history.MemorySubtract(3.2m);  // M-
decimal value = history.MemoryRecall(); // MR
history.MemoryClear();         // MC
```

### History Management

```csharp
history.AddCalculation("10 + 5 = 15");
history.DisplayHistory();
history.ClearHistory();
history.SaveHistoryToFile("my_calculations.txt");
```

## Troubleshooting

### Common Issues

- **Build Errors**: Ensure .NET 9.0 SDK is installed
- **Input Issues**: Use numeric values only for calculations
- **History Not Saving**: Check file write permissions

### Enhancement Ideas

- Add scientific calculator functions
- Implement expression parsing
- Add graphical user interface
- Support for complex numbers
- Unit conversion features

## Copilot Agent Workflow

This project demonstrates the Copilot Agent workflow:

1. **Issue Creation**: Detailed requirements in GitHub issue
2. **Agent Assignment**: @copilot assigned to issue
3. **Autonomous Development**: Agent analyzes and implements
4. **Pull Request**: Agent creates PR with solution
5. **Human Review**: Developer reviews and merges

## Best Practices for Copilot Agent

1. **Clear Requirements**: Specific, detailed issue descriptions
2. **Acceptance Criteria**: Measurable success metrics
3. **Technical Specifications**: Framework versions, patterns to use
4. **Example Interactions**: Show expected behavior
5. **Edge Cases**: Specify error handling needs

---

**Note**: This implementation was created by GitHub Copilot Coding Agent based on the issue requirements.
It demonstrates how AI can autonomously transform a simple "Hello World" application into a fully functional
calculator with proper error handling and user experience.

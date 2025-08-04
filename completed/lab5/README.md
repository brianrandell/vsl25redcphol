# Lab 5: Debugging with Copilot - Order Processing System

This folder contains the completed implementation of Lab 5 from the GitHub Copilot training at VSLIVE! 2025 Redmond. This console application demonstrates debugging techniques and performance optimization using GitHub Copilot.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `DebuggingWithCopilot` project under the `lab5` folder

### Step 2: Set as Startup Project

1. In **Solution Explorer**, right-click on `DebuggingWithCopilot`
2. Select **"Set as Startup Project"

### Step 3: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The order processing system will launch in the console

## Features Demonstrated

### Debugging Scenarios

* **Null Reference Exceptions**: Handling null orders and items
* **Invalid Data**: Processing orders with negative quantities and invalid discounts
* **Edge Cases**: Empty item lists and missing customer information
* **Performance Issues**: Identifying and fixing inefficient code

### Error Handling Improvements

* **Defensive Programming**: Null checks and validation throughout
* **Graceful Degradation**: Continuing processing when individual items fail
* **Clear Error Messages**: User-friendly error reporting
* **Logging**: Detailed calculation information for debugging

## Sample Order Data

The application processes four test orders with different scenarios:

1. **Order 1** - Valid order with laptop and mouse (with discount)
2. **Order 2** - Order with null items (demonstrates null handling)
3. **Order 3** - Order with invalid discount percentage (>100%)
4. **Order 4** - Order with empty items list

## Sample Output

``` shell
Processing Orders...

Order 1: $1019.98 - Customer: john@example.com
  Laptop: $999.99 x 1 - 0% = $999.99
  Mouse: $29.99 x 2 - 10% = $53.98

Error processing order 2: Value cannot be null. (Parameter 'order')

Warning: Discount exceeds 100% (110%) for Monitor. Capping at 100%.
Order 3: $79.99 - Customer: jane@example.com
  Keyboard: $79.99 x 0 - 0% = $0.00
  Monitor: $299.99 x 1 - 100% = $0.00

Warning: Order 4 has no items
Order 4: $0.00 - Customer: bob@example.com

Performance Test (Press Y to run, any other key to skip):
```

## Key Learning Points

### Debugging with Copilot

1. **Issue Identification**: Copilot helps identify common coding issues
2. **Null Reference Prevention**: Suggests defensive programming patterns
3. **Performance Analysis**: Identifies inefficient code patterns
4. **Error Handling**: Suggests comprehensive exception handling

### Common Issues Fixed

* **Null Reference Exceptions**: Added null checks for orders and items
* **Invalid Business Logic**: Validated discount percentages (0-100%)
* **Performance Bottlenecks**: Replaced O(n) LINQ queries with O(1) dictionary lookups
* **Synchronous Bottlenecks**: Added parallel processing for large datasets

## Performance Optimization

### Before Optimization

```csharp
// Inefficient O(n) lookup
var matchingOrders = orders.Where(o => o.Id == order.Id).ToList();
Thread.Sleep(1); // Simulated slow operation
```

### After Optimization

```csharp
// Efficient O(1) dictionary lookup
var orderDict = orders.ToDictionary(o => o.Id);
Parallel.ForEach(orders, order => {
    if (orderDict.TryGetValue(order.Id, out var matchingOrder))
    {
        // Process order
    }
});
```

## Error Handling Patterns

### Null Safety

```csharp
if (order?.Items == null || !order.Items.Any())
{
    Console.WriteLine($"Warning: Order {order.Id} has no items");
    return 0;
}
```

### Data Validation

```csharp
if (discountPercent > 100)
{
    Console.WriteLine($"Warning: Discount exceeds 100%. Capping at 100%.");
    discountPercent = 100;
}
```

## Project Structure

``` shell
DebuggingWithCopilot/
├── Program.cs                 # Main application with order processing
├── DebuggingWithCopilot.csproj # Project file
└── README.md                  # This documentation
```

## Troubleshooting

### Build Issues

* Ensure .NET 9.0 SDK is installed
* Check that project references are correct
* Clean and rebuild if necessary

### Runtime Issues

* Check console output for detailed error messages
* Verify that all test data is properly initialized
* Use debugger to step through problematic code

## Testing Scenarios

### Manual Testing

1. Run the application and observe order processing
2. Note how null values are handled gracefully
3. Check that invalid discounts are corrected
4. Run the performance test to see optimization results

### Debugging Practice

1. Set breakpoints in `CalculateTotal` method
2. Step through each order to see validation logic
3. Examine variable values during null reference scenarios
4. Compare performance between optimized and unoptimized methods

## Next Steps

* Add unit tests for edge cases
* Implement logging framework
* Add configuration for validation rules
* Create custom exception types
* Add database integration for real order data

---

**Note**: This project is designed for learning defensive programming and debugging techniques. The intentional bugs and performance issues demonstrate common problems developers encounter and how GitHub Copilot can assist in identifying and fixing them.

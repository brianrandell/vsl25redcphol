# Lab 7: Advanced Desktop Development with Copilot (WPF) - Hands-on Exercise

**Duration:** 35 minutes

## Prerequisites

* Visual Studio 2022 with GitHub Copilot enabled
* .NET 9.0 or later (WPF Core)
* Basic understanding of XAML and MVVM pattern

## Exercise Overview

Master advanced Copilot techniques for desktop development by creating a WPF expense tracker application. Learn to leverage Copilot for complex XAML generation, MVVM pattern implementation with modern frameworks, advanced data binding scenarios, comprehensive validation, and professional styling techniques.

> When working with Xaml, you'll want to use Chat to generate the XAML structure and let Copilot fill in the details.

---

## Part 1: Create WPF Project Structure (3 minutes)

1. **Create new WPF project:**

   * File → New → Project
   * Search for "WPF Application"
   * Select "WPF Application" (not .NET Framework)
   * Name: `ExpenseTracker`
   * Framework: .NET 9.0
   * Click `Create`

2. **Create MVVM folder structure:**

   * Right-click project → Add folders:
       * `Models`
       * `ViewModels`
       * `Views`
       * `Commands`
       * `Converters`

3. **Install MVVM helper package:**

   * Right-click project → Manage NuGet Packages
   * Install: `CommunityToolkit.Mvvm`

---

## Part 2: Generate Models with Copilot (5 minutes)

### Step 1: Create Expense Model

1. **Add new class** `Models/Expense.cs`

2. **Type this comment in Chat:**

   ```csharp
   // Expense model for tracking personal expenses
   // Properties:
   // - Id (Guid) - unique identifier
   // - Description (string) - expense description
   // - Amount (decimal) - expense amount
   // - Category (enum) - Food, Transport, Entertainment, Utilities, Other
   // - Date (DateTime) - when expense occurred
   // - PaymentMethod (enum) - Cash, Credit, Debit
   // - IsRecurring (bool) - whether expense repeats monthly
   // - Notes (string) - optional additional notes
   // Implement INotifyPropertyChanged for data binding
   using System.ComponentModel;
   using System.Runtime.CompilerServices;
   ```

3. **Let Copilot generate the model with property change notification**

### Step 2: Create Category Enum

1. **Add new file** `Models/ExpenseCategory.cs`

2. **Type:**

   ```csharp
   // Enum for expense categories with display-friendly names
   ```

---

## Part 3: Implement ViewModels with MVVM (8 minutes)

### Step 1: Create Base ViewModel

1. **Add new class** `ViewModels/ViewModelBase.cs`

2. **Type comment:**

   ```csharp
   // Base ViewModel class using CommunityToolkit.Mvvm
   // Inherit from ObservableObject
   // Add common functionality for all ViewModels
   using CommunityToolkit.Mvvm.ComponentModel;
   ```

### Step 2: Create Main ViewModel

1. **Add new class** `ViewModels/MainViewModel.cs`

2. **Type comprehensive comment:**

   ```csharp
   // Main ViewModel for expense tracker application
   // Using CommunityToolkit.Mvvm attributes
   // Properties:
   // - ObservableCollection<Expense> for expense list
   // - Selected expense property
   // - Properties for new expense input (amount, description, etc.)
   // - Calculated properties for totals (today, this month, by category)
   // Commands:
   // - Add expense command
   // - Delete expense command
   // - Edit expense command
   // - Filter by category command
   // Use [ObservableProperty] attribute for automatic property implementation
   // Use [RelayCommand] attribute for command implementation
   using CommunityToolkit.Mvvm.ComponentModel;
   using CommunityToolkit.Mvvm.Input;
   using System.Collections.ObjectModel;
   ```

3. **Let Copilot generate the ViewModel structure**

### Step 3: Implement Commands

1. **In MainViewModel, add command methods:**

   ```csharp
   // Method to add new expense with validation
   // Clear input fields after adding
   [RelayCommand]
   private void AddExpense()
   
   // Method to delete selected expense with confirmation
   [RelayCommand]
   private void DeleteExpense()
   
   // Method to calculate total expenses for current month
   private decimal CalculateMonthlyTotal()
   
   // Method to group expenses by category
   private Dictionary<ExpenseCategory, decimal> GetExpensesByCategory()
   ```

---

## Part 4: Create XAML UI with Copilot (10 minutes)

### Step 1: Design Main Window Layout

1. **Open `MainWindow.xaml`**

2. **Replace default Grid with comment:**

   ```xml
   <!-- Create a modern expense tracker layout:
        - Header with app title and current month total
        - Left panel: Add new expense form
        - Right panel: Expense list with search/filter
        - Bottom: Statistics (total, by category)
        Use Grid for layout with proper row/column definitions -->
   ```

3. **Let Copilot generate the Grid structure**

### Step 2: Create Expense Input Form

1. **In the left panel section, add:**

   ```xml
   <!-- Expense input form with:
        - TextBox for description with watermark
        - TextBox for amount with currency formatting
        - ComboBox for category (bind to enum values)
        - DatePicker for expense date
        - ComboBox for payment method
        - CheckBox for recurring expense
        - TextBox for notes (multiline)
        - Button to add expense (bind to AddExpenseCommand)
        Use modern styling with proper spacing -->
   ```

### Step 3: Create Expense List View

1. **In the right panel, add:**

   ```xml
   <!-- DataGrid to display expenses:
        - Columns: Date, Description, Category, Amount, Payment Method
        - Enable sorting on all columns
        - Alternating row colors
        - Selection binding to SelectedExpense
        - Context menu for delete/edit
        - Group by date (Today, Yesterday, This Week, Older)
        Style with modern look -->
   ```

### Step 4: Add Statistics Panel

1. **In the bottom section, add:**

   ```xml
   <!-- Statistics panel showing:
        - Total expenses this month
        - Quick stats cards (today's total, average daily)
        Use ItemsControl with DataTemplate for category stats -->
   ```

---

## Part 5: Implement Data Binding and Validation (8 minutes)

### Step 1: Set DataContext and Bindings

1. **In `MainWindow.xaml.cs` constructor, add:**

   ```csharp
   // Set DataContext to MainViewModel instance
   ```

2. **Update XAML bindings in input form:**

   ```xml
   <!-- Update TextBox for amount:
        - Bind Text to AmountInput property
        - Add validation rule for numeric input
        - Show red border on validation error
        - Update on PropertyChanged -->
   ```

### Step 2: Create Value Converters

1. **Add new class** `Converters/CurrencyConverter.cs`

2. **Type:**

   ```csharp
   // Value converter to format decimal as currency
   // Implement IValueConverter
   // Convert: decimal to string with currency symbol
   // ConvertBack: parse string to decimal
   ```

3. **Add converter to Window.Resources:**

   ```xml
   <!-- Add converter as static resource -->
   ```

### Step 3: Implement Input Validation

1. **In Expense model, add validation:**

   ```csharp
   // Add validation attributes:
   // - Description: Required, MaxLength(100)
   // - Amount: Range(0.01, 10000)
   // Implement IDataErrorInfo for validation messages
   ```

2. **Create validation template in XAML:**

   ```xml
   <!-- Create ControlTemplate for validation errors
        - Red border around control
        - Tooltip showing error message
        - Error icon -->
   ```

---

## Part 6: Style the Application (6 minutes)

### Step 1: Create Modern Theme

1. **Add new Resource Dictionary** `Styles/ModernTheme.xaml`

2. **Add comment and let Copilot generate:**

   ```xml
   <!-- Modern dark theme for expense tracker:
        - Dark background colors (#1e1e1e, #2d2d2d)
        - Accent color (#0078d4) for buttons and selection
        - Rounded corners (CornerRadius="5")
        - Subtle shadows and hover effects
        - Custom styles for:
          - Buttons (primary and secondary)
          - TextBoxes with watermarks
          - ComboBoxes
          - DataGrid with custom row style
          - Cards for statistics -->
   ```

### Step 2: Apply Animations

1. **Add entrance animations:**

   ```xml
   <!-- Add fade-in animation for expense items
        - Storyboard with opacity animation
        - Triggered on Loaded event
        - Duration: 0.3 seconds -->
   ```

2. **Add hover effects:**

   ```xml
   <!-- Button hover effect:
        - Scale transform to 1.05
        - Background color change
        - Smooth transitions -->
   ```

### Step 3: Create Responsive Layout

1. **Update Grid definitions:**

   ```xml
   <!-- Make layout responsive:
        - MinWidth for panels
        - Star sizing for flexible columns
        - ScrollViewer for expense list
        - Viewbox for statistics if needed -->
   ```

---

## Running and Testing (3 minutes)

1. **Test the application:**

   * Press F5 to run
   * Add several test expenses
   * Verify calculations update
   * Test validation (empty description, negative amount)
   * Check delete functionality

2. **Test data binding:**

   * Change selection in DataGrid
   * Verify totals update immediately
   * Check two-way binding on edit

3. **Verify MVVM pattern:**

   * No code-behind logic except DataContext
   * All logic in ViewModels
   * Commands working properly

---

## Key Takeaways

✅ **You've learned to:**

* Generate XAML layouts with Copilot
* Implement MVVM pattern with modern helpers
* Create complex data bindings
* Add input validation
* Style WPF applications
* Use commands and converters

## Best Practices

1. **MVVM Implementation**

   * Keep Views thin (XAML only)
   * Business logic in ViewModels
   * Use commanding for all actions

2. **Data Binding**

   * Use strong typing with x:DataType
   * Implement INotifyPropertyChanged
   * Use converters for formatting

3. **Performance**

   * Virtualize large lists
   * Use async commands for long operations
   * Minimize binding updates

## Challenge Extensions (Optional)

1. **Add advanced features:**

   ```csharp
   // Export to CSV/Excel
   // Add Charts using an OSS libary
   // Monthly budget tracking
   // Expense categories management
   ```

2. **Add persistence:**

   ```csharp
   // Save to SQL Server or SQLite database
   // Auto-save functionality
   // Import from bank statements
   ```

## Troubleshooting

**Binding not working?**

* Check DataContext is set
* Verify property names match
* Check Output window for binding errors

**Commands not firing?**

* Ensure ICommand is properly implemented
* Check CanExecute returns true
* Verify Command binding syntax

**Validation not showing?**

* Implement IDataErrorInfo correctly
* Set ValidatesOnDataErrors=True
* Check validation template is applied

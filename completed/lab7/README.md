# Lab 7: Advanced Desktop Development with Copilot (WPF) - Expense Tracker

This folder contains the completed implementation of Lab 7 from the GitHub Copilot training at VSLIVE! 2025 Redmond.
This WPF desktop application demonstrates advanced Copilot techniques for desktop development,
including complex XAML generation, modern MVVM pattern implementation, sophisticated data binding scenarios,
and professional styling techniques.

## Prerequisites

* **Visual Studio 2022** (any edition)
* **GitHub Copilot** extension enabled
* **.NET 9.0 SDK**
* **Windows 10/11** (WPF requires Windows)

## Getting Started

### Step 1: Open the Solution

1. Launch **Visual Studio 2022**
2. Navigate to `File` → `Open` → `Project or Solution`
3. Browse to `completed\cphol25red.sln` and open it
4. Locate the `ExpenseTracker` project under the `lab7` folder

### Step 2: Set as Startup Project

1. In **Solution Explorer**, right-click on `ExpenseTracker`
2. Select **"Set as Startup Project"

### Step 3: Build and Run

1. Press `Ctrl+Shift+B` to build the solution
2. Press `F5` to run the application
3. The WPF Expense Tracker application will launch

## Features Demonstrated

### Core Functionality

* **Expense Management**: Add, edit, delete, and view expenses
* **Category System**: Organize expenses by categories (Food, Transportation, Entertainment, etc.)
* **Payment Methods**: Track different payment types (Cash, Credit Card, Debit Card, Bank Transfer)
* **Data Persistence**: Local storage with automatic save/load
* **Rich UI**: Modern WPF interface with data binding and validation

### WPF Technologies

* **MVVM Pattern**: Model-View-ViewModel architecture
* **Data Binding**: Two-way binding between UI and data
* **Commands**: ICommand implementation for user actions
* **Value Converters**: Custom converters for data formatting
* **Validation**: Input validation with error display
* **Styling**: Custom WPF styles and templates

## Application Interface

### Main Window Features

* **Expense List**: DataGrid showing all expenses with sorting and filtering
* **Add/Edit Form**: Input form for expense details
* **Summary Panel**: Total expenses and category breakdown
* **Filter Options**: Filter by date range, category, and payment method
* **Search Functionality**: Quick search through expense descriptions

### User Interactions

1. **Add Expense**: Click "Add Expense" button to create new entries
2. **Edit Expense**: Double-click any expense to modify
3. **Delete Expense**: Select expense and press Delete key or use context menu
4. **Filter/Sort**: Use column headers to sort, filters to narrow down results
5. **Summary View**: View total spending and category breakdowns

## Project Structure

``` shell
ExpenseTracker/
├── Models/
│   ├── Expense.cs              # Expense data model
│   ├── ExpenseCategory.cs      # Category enumeration
│   └── PaymentMethod.cs        # Payment method enumeration
├── ViewModels/
│   ├── ViewModelBase.cs        # Base class for ViewModels
│   └── MainViewModel.cs        # Main window ViewModel
├── Views/
│   ├── MainWindow.xaml         # Main window UI
│   └── MainWindow.xaml.cs      # Main window code-behind
├── Converters/
│   ├── CurrencyConverter.cs        # Currency formatting
│   ├── EnumDescriptionConverter.cs # Enum display names
│   └── NullToBooleanConverter.cs   # Null checking
├── Commands/                   # ICommand implementations
├── Resources/
│   └── expense-icon.svg        # Application icon
├── App.xaml                    # Application resources and styles
└── App.xaml.cs                 # Application startup logic
```

## Key Features in Detail

### MVVM Implementation

```csharp
public class MainViewModel : ViewModelBase
{
    private ObservableCollection<Expense> _expenses;
    private Expense _selectedExpense;
    private ICommand _addExpenseCommand;
    
    public ObservableCollection<Expense> Expenses
    {
        get => _expenses;
        set => SetProperty(ref _expenses, value);
    }
    
    public ICommand AddExpenseCommand =>
        _addExpenseCommand ??= new RelayCommand(AddExpense);
}
```

### Data Binding Examples

```xml
<!-- Two-way binding with validation -->
<TextBox Text="{Binding SelectedExpense.Amount, 
                UpdateSourceTrigger=PropertyChanged,
                ValidatesOnDataErrors=True}" />

<!-- Command binding -->
<Button Content="Add Expense" 
        Command="{Binding AddExpenseCommand}" />

<!-- Collection binding -->
<DataGrid ItemsSource="{Binding FilteredExpenses}"
          SelectedItem="{Binding SelectedExpense}" />
```

### Value Converters

```csharp
public class CurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, 
                         object parameter, CultureInfo culture)
    {
        if (value is decimal amount)
            return amount.ToString("C", culture);
        return value;
    }
}
```

## Sample Data and Usage

### Adding Expenses

1. Click **"Add Expense"** button
2. Fill in the form:

    * **Description**: "Lunch at restaurant"
    * **Amount**: 25.50
    * **Category**: Food & Dining
    * **Payment Method**: Credit Card
    * **Date**: Select date

3. Click **"Save"** to add the expense

### Expense Categories

* **Food & Dining**: Restaurant meals, groceries, coffee
* **Transportation**: Gas, public transit, rideshare
* **Entertainment**: Movies, concerts, games
* **Shopping**: Clothing, electronics, miscellaneous purchases
* **Bills & Utilities**: Rent, utilities, subscriptions
* **Healthcare**: Medical expenses, pharmacy
* **Travel**: Hotels, flights, vacation expenses
* **Other**: Miscellaneous expenses

### Payment Methods

* **Cash**: Physical cash payments
* **Credit Card**: Credit card transactions
* **Debit Card**: Debit card payments
* **Bank Transfer**: Direct bank transfers

## Data Persistence

### Local Storage

* Expenses are automatically saved to a local JSON file
* Data persists between application sessions
* File location: `%AppData%\ExpenseTracker\expenses.json`

### Automatic Backup

* Application creates backup files periodically
* Previous versions are maintained for recovery
* Backup location: `%AppData%\ExpenseTracker\Backups\`

## Key Learning Points

### Copilot WPF Development

1. **XAML Generation**: Copilot suggests appropriate XAML markup
2. **Data Binding**: Generates binding expressions and converters
3. **MVVM Patterns**: Suggests ViewModels and command implementations
4. **Event Handling**: Creates appropriate event handlers and commands
5. **Styling**: Suggests WPF styles and templates

### Modern WPF Practices

* **MVVM Architecture**: Clean separation of concerns
* **Data Binding**: Reduces code-behind and improves maintainability
* **INotifyPropertyChanged**: Proper change notification
* **ObservableCollection**: Collections that notify UI of changes
* **Value Converters**: Transform data for display purposes

## Customization Options

### Themes and Styling

* Modify `App.xaml` to change application-wide styles
* Update colors, fonts, and control templates
* Add dark/light theme switching

### Additional Features

* Export to Excel/CSV
* Import from banking applications
* Budget tracking and alerts
* Recurring expense management
* Multi-currency support

## Troubleshooting

### Build Issues

* Ensure .NET 9.0 Windows Desktop workload is installed
* Check that all XAML files compile without errors
* Verify NuGet packages are restored

### Runtime Issues

* Check for XAML binding errors in Output window
* Verify data context is set correctly
* Ensure ViewModels implement INotifyPropertyChanged

### Data Issues

* Check file permissions in %AppData% folder
* Verify JSON serialization/deserialization
* Look for data validation errors

### UI Issues

* Check XAML markup for syntax errors
* Verify binding paths match ViewModel properties
* Test with different window sizes and DPI settings

## Next Steps

* Add reporting and analytics features
* Implement cloud synchronization
* Create mobile companion app
* Add receipt scanning capabilities
* Implement advanced budgeting features
* Add multi-user support

---

**Note**: This WPF application demonstrates modern desktop development practices and is designed for Windows desktop environments.
The application uses local storage for simplicity, but can be extended to use cloud storage or databases.

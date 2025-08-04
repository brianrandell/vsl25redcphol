using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ExpenseTracker.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace ExpenseTracker.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<Expense> expenses = new();

    [ObservableProperty]
    private Expense? selectedExpense;

    // Input properties for new expense form
    [ObservableProperty]
    private string newExpenseDescription = string.Empty;

    [ObservableProperty]
    private string newExpenseAmountText = string.Empty;

    [ObservableProperty]
    private ExpenseCategory newExpenseCategory = ExpenseCategory.Other;

    [ObservableProperty]
    private DateTime newExpenseDate = DateTime.Today;

    [ObservableProperty]
    private PaymentMethod newExpensePaymentMethod = PaymentMethod.Cash;

    [ObservableProperty]
    private bool newExpenseIsRecurring;

    [ObservableProperty]
    private string newExpenseNotes = string.Empty;

    [ObservableProperty]
    private ExpenseCategory? filterCategory;

    public MainViewModel()
    {
        // Add some sample data
        LoadSampleData();
        
        // Listen for property changes to update calculations
        PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Expenses) || e.PropertyName == nameof(FilterCategory))
        {
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(TotalThisMonth));
            OnPropertyChanged(nameof(TotalToday));
            OnPropertyChanged(nameof(ExpensesByCategory));
        }
    }

    public IEnumerable<Expense> FilteredExpenses
    {
        get
        {
            var filtered = Expenses.AsEnumerable();
            
            if (FilterCategory.HasValue)
            {
                filtered = filtered.Where(e => e.Category == FilterCategory.Value);
            }
            
            return filtered.OrderByDescending(e => e.Date).ThenByDescending(e => e.Amount);
        }
    }

    public decimal TotalThisMonth
    {
        get
        {
            var now = DateTime.Now;
            return Expenses
                .Where(e => e.Date.Year == now.Year && e.Date.Month == now.Month)
                .Sum(e => e.Amount);
        }
    }

    public decimal TotalToday
    {
        get
        {
            var today = DateTime.Today;
            return Expenses
                .Where(e => e.Date.Date == today)
                .Sum(e => e.Amount);
        }
    }

    public IEnumerable<CategoryExpenseGroup> ExpensesByCategory
    {
        get
        {
            return Expenses
                .GroupBy(e => e.Category)
                .Select(g => new CategoryExpenseGroup
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount),
                    Count = g.Count(),
                    Percentage = Expenses.Any() ? (double)(g.Sum(e => e.Amount) / Expenses.Sum(e => e.Amount)) * 100 : 0
                })
                .OrderByDescending(g => g.Total);
        }
    }

    [RelayCommand]
    private void AddExpense()
    {
        if (!ValidateNewExpense())
            return;

        if (!decimal.TryParse(NewExpenseAmountText, out decimal amount))
        {
            MessageBox.Show("Please enter a valid amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var expense = new Expense
        {
            Description = NewExpenseDescription.Trim(),
            Amount = amount,
            Category = NewExpenseCategory,
            Date = NewExpenseDate,
            PaymentMethod = NewExpensePaymentMethod,
            IsRecurring = NewExpenseIsRecurring,
            Notes = NewExpenseNotes.Trim()
        };

        Expenses.Add(expense);
        ClearNewExpenseForm();
        
        OnPropertyChanged(nameof(FilteredExpenses));
        OnPropertyChanged(nameof(TotalThisMonth));
        OnPropertyChanged(nameof(TotalToday));
        OnPropertyChanged(nameof(ExpensesByCategory));
    }

    [RelayCommand]
    private void DeleteExpense()
    {
        if (SelectedExpense == null)
            return;

        var result = MessageBox.Show(
            $"Are you sure you want to delete the expense '{SelectedExpense.Description}'?",
            "Confirm Delete",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            Expenses.Remove(SelectedExpense);
            SelectedExpense = null;
            
            OnPropertyChanged(nameof(FilteredExpenses));
            OnPropertyChanged(nameof(TotalThisMonth));
            OnPropertyChanged(nameof(TotalToday));
            OnPropertyChanged(nameof(ExpensesByCategory));
        }
    }

    [RelayCommand]
    private void ClearFilter()
    {
        FilterCategory = null;
    }

    [RelayCommand]
    private void FilterByCategory(ExpenseCategory category)
    {
        FilterCategory = category;
    }

    private bool ValidateNewExpense()
    {
        if (string.IsNullOrWhiteSpace(NewExpenseDescription))
        {
            MessageBox.Show("Please enter a description.", "Missing Description", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (string.IsNullOrWhiteSpace(NewExpenseAmountText))
        {
            MessageBox.Show("Please enter an amount.", "Missing Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        if (!decimal.TryParse(NewExpenseAmountText, out decimal amount) || amount <= 0)
        {
            MessageBox.Show("Please enter a valid positive amount.", "Invalid Amount", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }

        return true;
    }

    private void ClearNewExpenseForm()
    {
        NewExpenseDescription = string.Empty;
        NewExpenseAmountText = string.Empty;
        NewExpenseCategory = ExpenseCategory.Other;
        NewExpenseDate = DateTime.Today;
        NewExpensePaymentMethod = PaymentMethod.Cash;
        NewExpenseIsRecurring = false;
        NewExpenseNotes = string.Empty;
    }

    private void LoadSampleData()
    {
        var sampleExpenses = new[]
        {
            new Expense { Description = "Grocery Shopping", Amount = 85.50m, Category = ExpenseCategory.Food, Date = DateTime.Today, PaymentMethod = PaymentMethod.Debit },
            new Expense { Description = "Gas Fill-up", Amount = 45.00m, Category = ExpenseCategory.Transport, Date = DateTime.Today.AddDays(-1), PaymentMethod = PaymentMethod.Credit },
            new Expense { Description = "Netflix Subscription", Amount = 15.99m, Category = ExpenseCategory.Entertainment, Date = DateTime.Today.AddDays(-2), PaymentMethod = PaymentMethod.Credit, IsRecurring = true },
            new Expense { Description = "Electric Bill", Amount = 120.75m, Category = ExpenseCategory.Utilities, Date = DateTime.Today.AddDays(-3), PaymentMethod = PaymentMethod.BankTransfer },
            new Expense { Description = "Coffee Shop", Amount = 4.50m, Category = ExpenseCategory.Food, Date = DateTime.Today.AddDays(-1), PaymentMethod = PaymentMethod.Cash },
            new Expense { Description = "Movie Tickets", Amount = 24.00m, Category = ExpenseCategory.Entertainment, Date = DateTime.Today.AddDays(-5), PaymentMethod = PaymentMethod.Credit },
        };

        foreach (var expense in sampleExpenses)
        {
            Expenses.Add(expense);
        }
    }
}

public class CategoryExpenseGroup
{
    public ExpenseCategory Category { get; set; }
    public decimal Total { get; set; }
    public int Count { get; set; }
    public double Percentage { get; set; }
    public string FormattedTotal => Total.ToString("C");
    public string FormattedPercentage => $"{Percentage:F1}%";
}
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models;

public partial class Expense : ObservableValidator
{
    [ObservableProperty]
    private Guid id = Guid.NewGuid();

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(100, ErrorMessage = "Description cannot exceed 100 characters")]
    private string description = string.Empty;

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Range(0.01, 10000.00, ErrorMessage = "Amount must be between $0.01 and $10,000.00")]
    private decimal amount;

    [ObservableProperty]
    private ExpenseCategory category = ExpenseCategory.Other;

    [ObservableProperty]
    private DateTime date = DateTime.Today;

    [ObservableProperty]
    private PaymentMethod paymentMethod = PaymentMethod.Cash;

    [ObservableProperty]
    private bool isRecurring;

    [ObservableProperty]
    [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
    private string notes = string.Empty;

    public string FormattedAmount => Amount.ToString("C");
    
    public string FormattedDate => Date.ToString("MMM dd, yyyy");
}
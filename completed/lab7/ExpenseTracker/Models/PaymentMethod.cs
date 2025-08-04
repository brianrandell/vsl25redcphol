using System.ComponentModel;

namespace ExpenseTracker.Models;

public enum PaymentMethod
{
    [Description("Cash")]
    Cash,
    
    [Description("Credit Card")]
    Credit,
    
    [Description("Debit Card")]
    Debit,
    
    [Description("Bank Transfer")]
    BankTransfer,
    
    [Description("Digital Wallet")]
    DigitalWallet
}
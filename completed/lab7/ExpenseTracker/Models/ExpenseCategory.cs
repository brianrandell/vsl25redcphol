using System.ComponentModel;

namespace ExpenseTracker.Models;

public enum ExpenseCategory
{
    [Description("Food & Dining")]
    Food,
    
    [Description("Transportation")]
    Transport,
    
    [Description("Entertainment")]
    Entertainment,
    
    [Description("Utilities")]
    Utilities,
    
    [Description("Healthcare")]
    Healthcare,
    
    [Description("Shopping")]
    Shopping,
    
    [Description("Other")]
    Other
}
using ExpenseTracker.ViewModels;
using System.Windows;

namespace ExpenseTracker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainViewModel();
    }
}
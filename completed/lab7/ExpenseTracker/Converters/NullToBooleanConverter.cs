using System.Globalization;
using System.Windows.Data;

namespace ExpenseTracker.Converters;

public class NullToBooleanConverter : IValueConverter
{
    public static readonly NullToBooleanConverter Instance = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
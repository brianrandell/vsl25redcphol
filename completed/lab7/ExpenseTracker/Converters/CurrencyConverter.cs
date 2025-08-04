using System.Globalization;
using System.Windows.Data;

namespace ExpenseTracker.Converters;

public class CurrencyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal decimalValue)
        {
            return decimalValue.ToString("C", culture);
        }
        
        if (value is double doubleValue)
        {
            return doubleValue.ToString("C", culture);
        }
        
        return value?.ToString() ?? string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            // Remove currency symbols and parse
            var cleanedValue = stringValue.Replace("$", "").Replace(",", "").Trim();
            
            if (decimal.TryParse(cleanedValue, out decimal result))
            {
                return result;
            }
        }
        
        return 0m;
    }
}
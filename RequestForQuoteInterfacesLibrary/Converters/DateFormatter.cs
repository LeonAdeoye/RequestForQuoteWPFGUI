using System;
using System.Windows;
using System.Windows.Data;

namespace RequestForQuoteInterfacesLibrary.Converters
{
    public class DateFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return ((DateTime) value).ToString(parameter != null ? parameter.ToString() : "dd MMM yyyy");
            }
            catch (Exception)
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

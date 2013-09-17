using System;
using System.Windows;
using System.Windows.Data;
using log4net;

namespace RequestForQuoteInterfacesLibrary.Converters
{
    public class DecimalPlaceConverter : IValueConverter
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter != null)
            {
                try
                {
                    var places = System.Convert.ToInt32(parameter);
                    return Math.Round(((decimal) value), places).ToString(culture);
                }
                catch (FormatException fe)
                {
                    log.Error("Invalid Parameter " + parameter.ToString(), fe);
                }                
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}

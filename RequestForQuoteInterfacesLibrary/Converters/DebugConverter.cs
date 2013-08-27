using System;
using System.Windows.Data;
using log4net;

namespace RequestForQuoteInterfacesLibrary.Converters
{
    public class DebugConverter : IValueConverter
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(log.IsDebugEnabled)
                log.Debug("value => " + value);

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (log.IsDebugEnabled)
                log.Debug("value => " + value);

            return value;
        }
    }
}

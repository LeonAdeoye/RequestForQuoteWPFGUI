using System;
using System.Collections.Generic;
using System.Windows.Data;

namespace RequestForQuoteInterfacesLibrary.Converters
{
    public class SeriesDataConverter : IValueConverter
    {
        /// <summary>
        /// Extracts the relevant series from the series dictionary using the series key parameter as a dicitonary key.
        /// </summary>
        /// <param name="value"> the series dictonary</param>
        /// <param name="targetType"></param>
        /// <param name="parameter"> the series key</param>
        /// <param name="culture"></param>
        /// <exception cref="ArgumentException"> thrown if the series is null</exception>
        /// <returns> a list of key value pairs corresponding to the series data for a given series key</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var seriesKey = parameter.ToString();
            var seriesData = value as Dictionary<string, List<KeyValuePair<string, decimal>>>;
            
            if (seriesData == null)
                throw new ArgumentException("value");

            return seriesData.ContainsKey(seriesKey) ? seriesData[seriesKey] : new List<KeyValuePair<string, decimal>>();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return string.Empty;
        }
    }
}

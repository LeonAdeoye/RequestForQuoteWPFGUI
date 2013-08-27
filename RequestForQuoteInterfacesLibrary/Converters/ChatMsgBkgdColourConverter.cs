using System;
using System.Windows.Data;
using System.Windows.Media;

namespace RequestForQuoteInterfacesLibrary.Converters
{
    public class ChatMsgBkgdColourConverter : IValueConverter
    {
      
        private readonly SolidColorBrush darkGray = new SolidColorBrush(Color.FromArgb(0xFF, 0x82, 0x82, 0x82));
        private readonly SolidColorBrush lightGray = new SolidColorBrush(Color.FromArgb(0xFF, 0xAF, 0xAF, 0xAF));

        public object Convert(object value,Type targetType,object parameter,System.Globalization.CultureInfo culture)
        {
            return ((int)value%2==0) ? darkGray : lightGray;
        }

        public object ConvertBack(object value,Type targetType,object parameter,System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;
using RequestForQuoteInterfacesLibrary.Constants;

namespace RequestForQuoteGridModuleLibrary
{
    /// <summary>
    /// Interaction logic for RequestForQuoteDetails.xaml
    /// </summary>
    public partial class RequestForQuoteDetails : UserControl
    {
        public RequestForQuoteDetails()
        {
            InitializeComponent();
        }

        private void ToggleTextBoxContent(TextBox textBox, string startText, string endText)
        {
            if (textBox != null && textBox.Text == startText)
                textBox.Text = endText;
        }

        private void SalesCommentary_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, RequestForQuoteConstants.ENTER_SALES_COMMENTARY, "");
            e.Handled = true;
        }

        private void SalesCommentary_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, "", RequestForQuoteConstants.ENTER_SALES_COMMENTARY);
            e.Handled = true;
        }

        private void TradersCommentary_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, RequestForQuoteConstants.ENTER_TRADER_COMMENTARY, "");
            e.Handled = true;
        }

        private void TradersCommentary_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, "", RequestForQuoteConstants.ENTER_TRADER_COMMENTARY);
            e.Handled = true;
        }

        private void ClientCommentary_OnGotFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, RequestForQuoteConstants.ENTER_CLIENT_FEEDBACK, "");
            e.Handled = true;
        }

        private void ClientCommentary_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ToggleTextBoxContent(sender as TextBox, "", RequestForQuoteConstants.ENTER_CLIENT_FEEDBACK);
            e.Handled = true;
        }
    }
}

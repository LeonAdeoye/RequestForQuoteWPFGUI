using System;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteGridModuleLibrary
{
    /// <summary>
    /// Interaction logic for RequestForQuoteLegDetails.xaml
    /// </summary>
    public partial class RequestForQuoteLegDetails : UserControl
    {
        public RequestForQuoteLegDetails()
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            InitializeComponent();
        }
    }
}

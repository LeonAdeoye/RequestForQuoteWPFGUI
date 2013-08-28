using System;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for BankHolidayMaintenanceUserControl.xaml
    /// </summary>
    public partial class BankHolidayMaintenanceUserControl : UserControl
    {
        public BankHolidayMaintenanceUserControl()
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;

            InitializeComponent();
        }
    }
}

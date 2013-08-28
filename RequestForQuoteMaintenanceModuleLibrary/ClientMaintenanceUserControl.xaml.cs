using System;
using System.Windows;
using System.Windows.Controls;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for ClientMaintenanceUserControl.xaml
    /// </summary>
    public partial class ClientMaintenanceUserControl : UserControl
    {
        public ClientMaintenanceUserControl()
        {
            this.Resources = Application.LoadComponent(
            new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            InitializeComponent();
        }
    }
}

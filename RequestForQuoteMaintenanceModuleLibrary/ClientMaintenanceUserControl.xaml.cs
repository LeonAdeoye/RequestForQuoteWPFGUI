using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Practices.Unity;

namespace RequestForQuoteMaintenanceModuleLibrary
{
    /// <summary>
    /// Interaction logic for ClientMaintenanceUserControl.xaml
    /// </summary>
    public partial class ClientMaintenanceUserControl : UserControl
    {
        public ClientMaintenanceUserControl(IUnityContainer container)
        {
            this.Resources = Application.LoadComponent(new Uri("WPFStyley_Metallic4;Component/Themes/StandardTheme.xaml", UriKind.Relative)) as ResourceDictionary;
            InitializeComponent();
            DataContext = container.Resolve<ClientMaintenanceViewModel>();
        }
    }
}
